using PotassiumK.GabbyDialogue;
using TMPro;
using UnityEngine;

namespace PotassiumK.GabbyDialogueSample
{
    public class TypewriterEffect : MonoBehaviour
    {
        [SerializeField]
        private float characterRevealDelay = 0.0125f;
        private float timeSinceLastReveal = 0.0f;
        private TMP_Text textMesh;

        private void Awake()
        {
            textMesh = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            GameSampleDialogueSystem.Instance().DialogueLineShown += () =>
            {
                textMesh.maxVisibleCharacters = 0;
                this.enabled = true;
            };
            GameSampleDialogueSystem.Instance().DialogueLineContinued += () =>
            {
                this.enabled = true;
            };
            textMesh.maxVisibleCharacters = 0;
        }

        private void OnEnable()
        {
            GameSampleDialogueSystem.Instance().AllowAdvancingDialogue = false;
        }

        private void Update()
        {
            // Update the animation
            timeSinceLastReveal += Time.deltaTime;
            if (timeSinceLastReveal >= characterRevealDelay)
            {
                ++textMesh.maxVisibleCharacters;
                timeSinceLastReveal = 0.0f;
                if (textMesh.maxVisibleCharacters > textMesh.textInfo.characterCount)
                {
                    this.enabled = false;
                    Invoke(nameof(AllowSkip), 0.1f); // Allow skipping dialogue after a short delay
                    return;
                }
            }

            // Check for skip input
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                textMesh.maxVisibleCharacters = textMesh.textInfo.characterCount;
                timeSinceLastReveal = 0.0f;
                this.enabled = false;
                Invoke(nameof(AllowSkip), 0.2f); // Allow skipping dialogue after a short delay
            }
        }

        private void AllowSkip()
        {
            GameSampleDialogueSystem.Instance().AllowAdvancingDialogue = true;
        }
    }
}
