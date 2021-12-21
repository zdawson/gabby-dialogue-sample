using System.Collections;
using UnityEngine;

namespace PotassiumK.GabbyDialogueSample
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
            GameSampleDialogueSystem.Instance().PlayDialogue("Charles", "Main");
        }

        private IEnumerator ShowIntroDialogue()
        {
            yield return new WaitForSeconds(0.25f);

            FadeController.instance.FadeIn(3.0f, false);

            GameSampleDialogueSystem.Instance().PlayDialogue("Charles", "GameStart");
        }
    }
}
