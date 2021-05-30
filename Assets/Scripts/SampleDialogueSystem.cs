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
        private List<GabbyDialogueAsset> dialogueAssets = new List<GabbyDialogueAsset>(); // TODO rename GabbyDialogueAsset. Maybe DialogueScript?
        private Dictionary<string, DialogueCharacter> characters = new Dictionary<string, DialogueCharacter>();
        private string currentCharacter = "";
        [HideInInspector]
        public string currentPortrait = "default";

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
            dialogueEngine = new DialogueEngine(this, new SampleScriptingHandler());
        }

        public void PlayDialogue(Dialogue dialogue)
        {
            currentCharacter = "";
            currentPortrait = "default";
            dialogueEngine.StartDialogue(dialogue);
            dialogueUI.gameObject.SetActive(true);
            OnDialogueStarted?.Invoke(dialogue);
        }

        public void OnDialogueLine(string characterName, string dialogueText, Dictionary<string, string> tags)
        {
            if (currentCharacter != characterName)
            {
                currentCharacter = characterName;
                currentPortrait = "default";
            }
            
            string tagPortrait;
            if (characters.ContainsKey(currentCharacter) && GetPortraitFromTags(characters[currentCharacter], tags, out tagPortrait))
            {
                currentPortrait = tagPortrait;
            }

            dialogueUI.SetDialogueText(dialogueText);
            if (characters.ContainsKey(characterName))
            {
                DialogueCharacter character = characters[characterName];
                dialogueUI.SetCharacter(character.displayName);
                if (!character.Portraits.ContainsKey(currentPortrait))
                {
                    currentPortrait = "default";
                }
                dialogueUI.SetCharacterPortrait(character.Portraits[currentPortrait]);
            }
            else
            {
                dialogueUI.SetCharacter(characterName);
            }
            dialogueUI.SetDialogueText(dialogueText);
        }

        public void OnContinuedDialogue(string additionalDialogueText, Dictionary<string, string> tags)
        {
            string tagPortrait;
            if (characters.ContainsKey(currentCharacter) && GetPortraitFromTags(characters[currentCharacter], tags, out tagPortrait))
            {
                currentPortrait = tagPortrait;
            }
            
            dialogueUI.SetDialogueText(dialogueUI.GetDialogueText() + "\n" + additionalDialogueText);
            if (characters.ContainsKey(currentCharacter))
            {
                DialogueCharacter character = characters[currentCharacter];
                if (character.Portraits.ContainsKey(currentPortrait))
                {
                    dialogueUI.SetCharacterPortrait(character.Portraits[currentPortrait]);
                }
            }
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

        public void OnDialogueEnd()
        {
            dialogueUI.gameObject.SetActive(false);
            OnDialogueEnded?.Invoke(true);
        }

        public Dialogue GetDialogue(string characterName, string dialogueName)
        {
            foreach (GabbyDialogueAsset asset in dialogueAssets)
            {
                foreach (Dialogue dialogue in asset.dialogues)
                {
                    if (dialogue.CharacterName == characterName && dialogue.DialogueName == dialogueName)
                    {
                        return dialogue;
                    }
                }
            }
            return null;
        }

        public void AddDialogueAsset(GabbyDialogueAsset asset)
        {
            dialogueAssets.Add(asset);
        }

        /// <summary>
        /// Gets the portrait for the given character from the provided tags. Looks for values tagged "portrait", or any tags from the character's defined portraits.
        /// </summary>
        private static bool GetPortraitFromTags(DialogueCharacter character, Dictionary<string, string> tags, out string outPortrait)
        {
            string portrait;
            if (tags.TryGetValue("portrait", out portrait))
            {
                outPortrait = portrait;
                return true;
            }

            foreach (string portraitName in character.Portraits.Keys)
            {
                if (tags.ContainsKey(portraitName))
                {
                    outPortrait = portraitName;
                    return true;
                }
            }

            outPortrait = "";
            return false;
        }
    }
}
