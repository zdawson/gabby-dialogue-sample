using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GabbyDialogue;

namespace GabbyDialogueSample
{
    public class SampleDialogueSystem : MonoBehaviour
    {
        private static SampleDialogueSystem _instance = null;

        private DialogueInterface dialogueUI;

        private DialogueEngine dialogueEngine;
        private Dictionary<string, DialogueCharacter> characters = new Dictionary<string, DialogueCharacter>();

        public static SampleDialogueSystem instance()
        {
            if (!_instance)
            {
                _instance = new GameObject("_SampleDialogueSystem").AddComponent<SampleDialogueSystem>();
            }
            return _instance;
        }

        private void Awake()
        {
            dialogueUI = (Instantiate(Resources.Load("Prefabs/DialogueUI", typeof(GameObject))) as GameObject).GetComponentInChildren<DialogueInterface>();
            dialogueUI.gameObject.name = "_SampleDialogueUI";
            dialogueUI.gameObject.SetActive(false);

            // Load character definitions
            Object[] resources = Resources.LoadAll("Characters/");
            foreach (Object resource in resources)
            {
                if (resource is DialogueCharacter)
                {
                    DialogueCharacter character = (DialogueCharacter)resource;
                    character.InitPortraits();
                    characters.Add(character.internalName, character);
                }
            }
        }

        public void PlayDialogue(Dialogue dialogue)
        {
            DialogueCharacter character = characters[dialogue.CharacterName];
            dialogueUI.SetCharacter(character.displayName, character.portraits["default"]);
            dialogueUI.SetDialogueText(dialogue.DialogueName);
            dialogueUI.gameObject.SetActive(true);
        }
    }
}
