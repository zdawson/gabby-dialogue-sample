using PotassiumK.GabbyDialogue;
using UnityEngine;

namespace PotassiumK.GabbyDialogueSample
{
    public class DialogueSystemInitializer : MonoBehaviour
    {
        [SerializeField]
        private DialogueCharacter[] characters = null;
        [SerializeField]
        private DialogueScript[] dialogueScripts = null;

        void Start()
        {
            GameSampleDialogueSystem.Instance().Init();
            foreach (DialogueCharacter character in characters)
            {
                GameSampleDialogueSystem.Instance().AddCharacter(character);
            }
            foreach (DialogueScript asset in dialogueScripts)
            {
                GameSampleDialogueSystem.Instance().AddScript(asset);
            }
        }
    }
}
