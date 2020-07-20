using System.Collections.Generic;
using System.Threading.Tasks;
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
        private DialogueOptionsUI dialogueOptionsUI;

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
            // Initialize UI
            dialogueUI = (Instantiate(Resources.Load("Prefabs/DialogueUI", typeof(GameObject))) as GameObject).GetComponentInChildren<DialogueUI>();
            dialogueUI.gameObject.name = "_SampleDialogueUI";
            dialogueUI.gameObject.SetActive(false);

            dialogueOptionsUI = (Instantiate(Resources.Load("Prefabs/DialogueOptionsUI", typeof(GameObject))) as GameObject).GetComponentInChildren<DialogueOptionsUI>();
            dialogueOptionsUI.gameObject.name = "_SampleDialogueOptionsUI";
            dialogueOptionsUI.gameObject.SetActive(false);

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

            // Create the dialogue engine instance
            // You can use multiple dialogue engines to run multiple concurrent dialogues, but this sample focuses on using one.
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
            dialogueUI.SetDialogueText(dialogueText);
            if (characters.ContainsKey(characterName))
            {
                DialogueCharacter character = characters[characterName];
                dialogueUI.SetCharacter(character.displayName);
                dialogueUI.SetCharacterPortrait(character.Portraits["default"]);
            }
            else
            {
                dialogueUI.SetCharacter(characterName);
            }
            dialogueUI.SetDialogueText(dialogueText);
        }

        public void OnContinuedDialogue(string additionalDialogueText)
        {
            dialogueUI.SetDialogueText(dialogueUI.GetDialogueText() + "\n" + additionalDialogueText);
        }

        public Task<int> OnOptionLine(string[] optionsText)
        {
            string currentLine = "Placeholder Text";
            string currentCharacter = "Charles";
            DialogueCharacter character = characters[currentCharacter];

            dialogueUI.gameObject.SetActive(false);
            dialogueOptionsUI.gameObject.SetActive(true);

            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();

            dialogueOptionsUI.Show(character.displayName, currentLine, optionsText, (selection) => {
                dialogueUI.gameObject.SetActive(true);
                dialogueOptionsUI.Hide();
                tcs.SetResult(selection);
                dialogueEngine.NextLine();
            });

            return tcs.Task;
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
