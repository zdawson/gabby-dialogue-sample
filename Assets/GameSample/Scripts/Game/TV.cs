using GabbyDialogue;
using System.Collections;
using UnityEngine;

namespace GabbyDialogueSample
{
    public class TV : MonoBehaviour, Interactable
    {
        [SerializeField]
        private SpriteRenderer screenOn = null;

        [SerializeField]
        private Renderer lightingOverlay = null;

        private float screenValue = 0.5f;
        private float lightingOverlayOpacity = 0.0f;
        private bool isTVActive = false;

        private void Start()
        {
            Color.RGBToHSV(screenOn.color, out _, out _, out screenValue);
        }

        public void OnInteract()
        {
            Dialogue dialogue = SampleDialogueSystem.instance().GetDialogue("Gabby", "TutorialStart");
            if (dialogue != null)
            {
                isTVActive = true;

                screenOn.gameObject.SetActive(true);

                lightingOverlayOpacity = 0.0f;
                SetOverlayOpacity(lightingOverlayOpacity);
                lightingOverlay.gameObject.SetActive(true);
                lightingOverlay.material.renderQueue = 3001; // This will ensure the overlay renders after all the room sprites
                screenOn.material.renderQueue = 3002; // This will make the TV render over the overlay

                SampleDialogueSystem.instance().PlayDialogue(dialogue);

                SampleDialogueSystem.instance().DialogueLineShown += OnDialogueLineShown;
                SampleDialogueSystem.instance().DialogueEnded += OnDialogueEnd;
            }
        }

        private void OnDialogueEnd()
        {
            isTVActive = false;

            SampleDialogueSystem.instance().DialogueEnded -= OnDialogueEnd;
            SampleDialogueSystem.instance().DialogueLineShown -= OnDialogueLineShown;
            screenOn.gameObject.SetActive(false);

            lightingOverlayOpacity = 0.0f;
            SetOverlayOpacity(lightingOverlayOpacity);
            lightingOverlay.gameObject.SetActive(false);
        }

        private void OnDialogueLineShown(LineType lineType)
        {
            float s, v;
            Color.RGBToHSV(screenOn.color, out _, out s, out v);
            screenOn.color = Color.HSVToRGB(Random.value, s, v);
        }
        
        private void Update()
        {
            if (!isTVActive)
            {
                return;
            }

            float h, s;
            Color.RGBToHSV(screenOn.color, out h, out s, out _);
            screenOn.color = Color.HSVToRGB(h, s, screenValue + (Random.value - 0.5f) * 0.15f);

            lightingOverlayOpacity = Mathf.Min(1.0f, lightingOverlayOpacity + Time.deltaTime * 0.3f);
            SetOverlayOpacity(Mathf.SmoothStep(0.0f, 1.0f, lightingOverlayOpacity));
        }

        private void SetOverlayOpacity(float opacity)
        {
            Color overlayColor = lightingOverlay.material.color;
            overlayColor.a = opacity;
            lightingOverlay.material.color = overlayColor;
        }
    }
}
