using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GabbyDialogueSample
{

    /// <summary>
    /// A simple object that can be interacted with, triggering a dialogue to run.
    /// </summary>
    public class InteractiveObject : MonoBehaviour, Interactable
    {
        [System.Serializable]
        private class DialogueEntry
        {
            public string character;
            public string dialogue;
        }

        [SerializeField]
        private DialogueEntry[] interactDialogues;

        public void OnInteract()
        {
            DialogueEntry dialogueEntry = interactDialogues[Random.Range(0, interactDialogues.Length)];
            GabbyDialogue.Dialogue dialogue = SampleDialogueSystem.instance().GetDialogue(dialogueEntry.character, dialogueEntry.dialogue);
            if (dialogue == null)
            {
                Debug.Log($"Dialogue for {nameof(InteractiveObject)} not found: {dialogueEntry.character}.{dialogueEntry.dialogue}");
                return;
            }

            SampleDialogueSystem.instance().PlayDialogue(dialogue);
        }
    }
}
