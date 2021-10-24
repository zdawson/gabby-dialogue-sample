using GabbyDialogue;
using UnityEngine;

namespace GabbyDialogueSample.GameSample
{
    public class Camilla : MonoBehaviour, Interactable
    {
        NPCWander wanderComponent;

        private void Awake()
        {
            wanderComponent = GetComponent<NPCWander>();
        }

        private void OnEnable()
        {
            GabbyDialogueSample.SampleDialogueSystem.instance().DialogueStarted += OnDialogueStarted;
            GabbyDialogueSample.SampleDialogueSystem.instance().DialogueEnded += OnDialogueEnded;
        }

        private void OnDisable()
        {
            if (GabbyDialogueSample.SampleDialogueSystem.instance() != null)
            {
                GabbyDialogueSample.SampleDialogueSystem.instance().DialogueStarted -= OnDialogueStarted;
                GabbyDialogueSample.SampleDialogueSystem.instance().DialogueEnded -= OnDialogueEnded;   
            }
        }

        public void OnInteract()
        {
            Dialogue dialogue = SampleDialogueSystem.instance().GetDialogue("Camilla", "Main");
            if (dialogue != null)
            {
                SampleDialogueSystem.instance().PlayDialogue(dialogue);
            }
        }

        private void OnDialogueStarted(GabbyDialogue.Dialogue dialogue)
        {
            if (wanderComponent)
            {
                wanderComponent.enabled = false;
            }
        }

        private void OnDialogueEnded()
        {
            if (wanderComponent)
            {
                wanderComponent.enabled = true;
            }
        }
    }
}
