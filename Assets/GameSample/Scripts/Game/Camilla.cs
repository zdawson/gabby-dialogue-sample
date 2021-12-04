using GabbyDialogue;
using UnityEngine;

namespace GabbyDialogueSample
{
    public class Camilla : MonoBehaviour, Interactable
    {
        public void OnInteract()
        {
            Dialogue dialogue = SampleDialogueSystem.instance().GetDialogue("Camilla", "Main");
            if (dialogue != null)
            {
                SampleDialogueSystem.instance().PlayDialogue(dialogue);
            }
        }
    }
}