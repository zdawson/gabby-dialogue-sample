using GabbyDialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GabbyDialogueSample.GameSample
{
    public class Charles : MonoBehaviour, Interactable
    {
        public void OnInteract()
        {
            Dialogue dialogue = SampleDialogueSystem.instance().GetDialogue("Charles", "Main");
            if (dialogue != null)
            {
                SampleDialogueSystem.instance().PlayDialogue(dialogue);
            }
        }
    }
}
