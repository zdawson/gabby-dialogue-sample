using GabbyDialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GabbyDialogueSample.GameSample
{
    public class Charles : MonoBehaviour, Interactable
    {

        private void Start()
        {
            // At the start of the game, show Charles' introduction dialogue
            StartCoroutine(ShowIntroDialogue());
        }

        public void OnInteract()
        {
            Dialogue dialogue = SampleDialogueSystem.instance().GetDialogue("Charles", "Main");
            if (dialogue != null)
            {
                SampleDialogueSystem.instance().PlayDialogue(dialogue);
            }
        }

        private IEnumerator ShowIntroDialogue()
        {
            yield return new WaitForSeconds(0.25f);
            
            FadeController.instance.FadeIn(3.0f, false);

            Dialogue dialogue = SampleDialogueSystem.instance().GetDialogue("Charles", "GameStart");
            if (dialogue != null)
            {
                SampleDialogueSystem.instance().PlayDialogue(dialogue);
            }
        }
    }
}
