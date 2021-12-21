using System.Collections;
using UnityEngine;

namespace PotassiumK.GabbyDialogueSample
{
    public class FadeController : MonoBehaviour
    {
        public static FadeController instance = null;

        [SerializeField]
        private UnityEngine.UI.Image fadeOverlay = null;
        [SerializeField]
        private UnityEngine.UI.Image flashOverlay = null;
        private UnityEngine.UI.Image activeOverlay = null;

        private int fadeDirection = 0;
        private float fadeOpacity = 0.0f;
        private float fadeDuration = 0.0f;

        private bool waitForInput = false;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            activeOverlay = fadeOverlay;
            if (activeOverlay == null)
            {
                this.enabled = false;
                return;
            }

            // Start from black
            fadeOpacity = 1.0f;
            SetOverlayOpacity(fadeOverlay, fadeOpacity);
        }

        void Update()
        {
            // Note: You could simplify this greatly with DOTween, if your project uses it.
            if (fadeDirection != 0)
            {
                fadeOpacity += fadeDirection * (Time.smoothDeltaTime / fadeDuration);
                if (fadeOpacity >= 1.0f && fadeDirection == 1)
                {
                    fadeDirection = 0;
                }
                else if (fadeOpacity <= 0.0f && fadeDirection == -1)
                {
                    fadeDirection = 0;
                }

                SetOverlayOpacity(activeOverlay, fadeOpacity);
            }

            if (waitForInput && Input.GetMouseButtonDown(0))
            {
                GameSampleDialogueSystem.Instance().AllowAdvancingDialogue = true;
                GameSampleDialogueSystem.Instance().NextLine();
                waitForInput = false;
            }
        }

        public void FadeOut(float fadeTime = 0.75f, bool shouldContinueAfterInput = true)
        {
            StartCoroutine(_FadeOut(fadeTime, shouldContinueAfterInput));
        }

        IEnumerator _FadeOut(float fadeTime, bool shouldContinueAfterInput)
        {
            activeOverlay = fadeOverlay;

            if (shouldContinueAfterInput)
            {
                GameSampleDialogueSystem.Instance().SetDialogueUIVisible(false);
                GameSampleDialogueSystem.Instance().AllowAdvancingDialogue = false;
            }

            // Fade out
            fadeDirection = 1;
            fadeDuration = fadeTime;

            yield return new WaitForSeconds(fadeTime);

            if (shouldContinueAfterInput)
            {
                waitForInput = true;
            }
        }

        public void FadeIn(float fadeTime = 0.75f, bool shouldContinueAfterInput = true)
        {
            StartCoroutine(_FadeIn(fadeTime, shouldContinueAfterInput));
        }

        IEnumerator _FadeIn(float fadeTime, bool shouldContinueAfterInput)
        {
            activeOverlay = fadeOverlay;

            if (shouldContinueAfterInput)
            {
                GameSampleDialogueSystem.Instance().AllowAdvancingDialogue = false;
            }

            // Fade in
            fadeDirection = -1;
            fadeDuration = fadeTime;

            yield return new WaitForSeconds(fadeTime);

            if (shouldContinueAfterInput)
            {
                waitForInput = true;
            }
        }

        public void FadeOutAndIn(float fadeTime = 0.75f, float fadeInDelay = 1.5f, bool shouldContinueAfterInput = true)
        {
            StartCoroutine(_FadeOutAndIn(fadeTime, fadeInDelay, shouldContinueAfterInput));
        }

        IEnumerator _FadeOutAndIn(float fadeTime, float delayTime, bool shouldContinueAfterInput)
        {
            activeOverlay = fadeOverlay;

            if (shouldContinueAfterInput)
            {
                GameSampleDialogueSystem.Instance().SetDialogueUIVisible(false);
                GameSampleDialogueSystem.Instance().AllowAdvancingDialogue = false;
            }

            // Fade out
            fadeDirection = 1;
            fadeDuration = fadeTime;

            yield return new WaitForSeconds(fadeTime + delayTime);

            // Fade in
            fadeDirection = -1;
            fadeDuration = fadeTime;

            yield return new WaitForSeconds(fadeTime);

            if (shouldContinueAfterInput)
            {
                waitForInput = true;
            }
        }

        public void Flash()
        {
            StartCoroutine(_Flash());
        }

        IEnumerator _Flash()
        {
            activeOverlay = flashOverlay;
            GameSampleDialogueSystem.Instance().AllowAdvancingDialogue = false;

            // Show flash
            fadeDirection = 1;
            fadeDuration = 0.05f;

            yield return new WaitForSeconds(0.2f);

            // Slowly hide flash
            fadeDirection = -1;
            fadeDuration = 0.5f;

            yield return new WaitForSeconds(0.5f);

            GameSampleDialogueSystem.Instance().AllowAdvancingDialogue = true;
        }

        private void SetOverlayOpacity(UnityEngine.UI.Image overlay, float opacity)
        {
            Color overlayColor = overlay.color;
            overlayColor.a = opacity;
            overlay.color = overlayColor;
        }
    }
}
