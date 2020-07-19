using System.Collections.Generic;
using UnityEngine;
using GabbyDialogue;

namespace GabbyDialogueSample
{
    public class SampleDialogueSystem : MonoBehaviour, IDialogueHandler
    {
        private static SampleDialogueSystem _instance = null;

        public event System.Action<Dialogue> OnDialogueStarted;
        public event System.Action<bool> OnDialogueEnded;

        private DialogueUI dialogueUI;

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
            dialogueUI = (Instantiate(Resources.Load("Prefabs/DialogueUI", typeof(GameObject))) as GameObject).GetComponentInChildren<DialogueUI>();
            dialogueUI.gameObject.name = "_SampleDialogueUI";
            dialogueUI.gameObject.SetActive(false);

            // Handle UI events
            dialogueUI.OnForward += () => dialogueEngine.NextLine();        

            // Load character definitions
            Object[] resources = Resources.LoadAll("Characters/");
            foreach (Object resource in resources)
            {
                if (resource is DialogueCharacter)
                {
                    DialogueCharacter character = (DialogueCharacter)resource;
                    characters.Add(character.internalName, character);
                }
            }

            dialogueEngine = new DialogueEngine(this);
        }

        public void PlayDialogue(Dialogue dialogue)
        {
            dialogueEngine.StartDialogue(dialogue);
            dialogueUI.gameObject.SetActive(true);
            OnDialogueStarted?.Invoke(dialogue);
        }

        public void OnDialogueLine(string characterName, string dialogueText)
        {
            DialogueCharacter character = characters[characterName];
            dialogueUI.SetCharacter(character.displayName);
            dialogueUI.SetCharacterPortrait(character.Portraits["default"]);
            dialogueUI.SetDialogueText(dialogueText);
        }

        public void OnContinuedDialogue(string additionalDialogueText)
        {
            dialogueUI.SetDialogueText(dialogueUI.GetDialogueText() + "\n" + additionalDialogueText);
        }

        public void OnOptionLine(string[] optionsText)
        {
            throw new System.NotImplementedException();
        }

        public void OnSpeakingCharacterChanged(string characterName)
        {
            throw new System.NotImplementedException();
        }

        public void OnAction(string actionName, string[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public void OnDialogueEnd()
        {
            dialogueUI.gameObject.SetActive(false);
            OnDialogueEnded?.Invoke(true);
        }

        public void GetVariable(string[] variablePath)
        {
            throw new System.NotImplementedException();
        }

        public void SetVariable(string[] variablePath, dynamic value)
        {
            throw new System.NotImplementedException();
        }
    }
}
