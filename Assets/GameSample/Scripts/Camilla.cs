using GabbyDialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GabbyDialogueSample.GameSample
{
    public class Camilla : MonoBehaviour, Interactable
    {
        public void OnInteract()
        {
            NPCWander wanderComponent = GetComponent<NPCWander>();
            wanderComponent.enabled = false;

            Dialogue dialogue = SampleDialogueSystem.instance().GetDialogue("Camilla", "Introduction");
            if (dialogue != null)
            {
                SampleDialogueSystem.instance().PlayDialogue(dialogue);
            }

            // TODO get when dialogue ends to re-enable wander
        }
    }
}
