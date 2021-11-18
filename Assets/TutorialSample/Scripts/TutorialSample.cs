using GabbyDialogue;
using System.Collections;
using UnityEngine;

namespace GabbyDialogueSample.TutorialSample
{
    public class TutorialSample : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(ShowIntroDialogue());
        }

        private IEnumerator ShowIntroDialogue()
        {
            yield return new WaitForSeconds(0.25f);
            
            FadeController.instance.FadeIn(3.0f, false);

            Dialogue dialogue = SampleDialogueSystem.instance().GetDialogue("Main", "GameStart");
            if (dialogue != null)
            {
                SampleDialogueSystem.instance().PlayDialogue(dialogue);
            }
        }
    }
}
