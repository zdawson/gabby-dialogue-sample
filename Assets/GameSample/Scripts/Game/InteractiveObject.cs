using UnityEngine;

namespace PotassiumK.GabbyDialogueSample
{
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
            GameSampleDialogueSystem.Instance().PlayDialogue(dialogueEntry.character, dialogueEntry.dialogue);
        }
    }
}
