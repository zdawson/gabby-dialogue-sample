using UnityEngine;

namespace GabbyDialogueSample
{
    public class Camilla : MonoBehaviour, Interactable
    {
        public void OnInteract()
        {
            GameSampleDialogueSystem.Instance().PlayDialogue("Camilla", "Main");
        }
    }
}
