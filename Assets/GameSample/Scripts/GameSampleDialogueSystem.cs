using PotassiumK.GabbyDialogue;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace PotassiumK.GabbyDialogueSample
{
    public class GameSampleDialogueSystem : SimpleDialogueSystem
    {
        private static GameSampleDialogueSystem _instance = null;

        public event Action DialogueStarted;
        public event Action DialogueEnded;
        public event Action DialogueLineShown;
        public event Action DialogueLineContinued;

        /// <summary>
        /// Allows advancing to the next line of dialogue when true.
        ///
        /// Set to false to block the user from moving on to the next line of dialogue.
        /// You may want to do this if another action should happen first, for example skipping the animation of a typewriter effect,
        /// where the first click skips the animation, then the second advances to the next line.
        /// </summary>
        /// <value></value>
        public bool AllowAdvancingDialogue
        {
            get => _allowAdvancingDialogue;
            set => _allowAdvancingDialogue = value;
        }
        private bool _allowAdvancingDialogue = true;

        private DialogueUI dialogueUI;
        private DialogueOptionsUI dialogueOptionsUI;

        private Dictionary<string, DialogueCharacter> characters = new Dictionary<string, DialogueCharacter>();
        private string currentCharacter = "";
        private string currentPortrait = "default";

        private ScriptVariableStorage scriptVariableStorage;

        public static GameSampleDialogueSystem Instance()
        {
            if (_instance == null)
            {
                _instance = new GameSampleDialogueSystem();
            }
            return _instance;
        }

        public void Init()
        {
            // Initialize UI
            dialogueUI = (GameObject.Instantiate(Resources.Load("UI/DialogueUI", typeof(GameObject))) as GameObject).GetComponentInChildren<DialogueUI>();
            dialogueUI.gameObject.name = "_SampleDialogueUI";
            dialogueUI.gameObject.SetActive(false);
            GameObject.DontDestroyOnLoad(dialogueUI);

            dialogueOptionsUI = (GameObject.Instantiate(Resources.Load("UI/DialogueOptionsUI", typeof(GameObject))) as GameObject).GetComponentInChildren<DialogueOptionsUI>();
            dialogueOptionsUI.gameObject.name = "_SampleDialogueOptionsUI";
            dialogueOptionsUI.gameObject.SetActive(false);
            GameObject.DontDestroyOnLoad(dialogueOptionsUI);

            // Handle UI events
            dialogueUI.OnForward += () => dialogueEngine.NextLine();

            scriptVariableStorage = new ScriptVariableStorage();

            scriptEventHandler.RegisterScriptEventHandler(new ScriptEventHandler_Common(scriptVariableStorage));
            scriptEventHandler.RegisterScriptEventHandler(new ScriptEventHandler_CookingSample(scriptVariableStorage));
            scriptEventHandler.RegisterScriptEventHandler(new ScriptEventHandler_LampQuest(scriptVariableStorage));
            scriptEventHandler.RegisterScriptEventHandler(new ScriptEventHandler_LocalizationSample(this));
            scriptEventHandler.RegisterScriptEventHandler(new ScriptEventHandler_Nim(scriptVariableStorage));
            scriptEventHandler.RegisterScriptEventHandler(new ScriptEventHandler_Tutorial());
            scriptEventHandler.RegisterScriptEventHandler(new ScriptEventHandler_VendorSample(scriptVariableStorage));
        }

        public override void PlayDialogue(string characterName, string dialogueName)
        {
            base.PlayDialogue(characterName, dialogueName);
            dialogueUI.gameObject.SetActive(true);
            DialogueStarted?.Invoke();
        }

        public void AddCharacter(DialogueCharacter character)
        {
            characters.Add(character.internalName, character);
        }

        public void SetDialogueUIVisible(bool visible)
        {
            dialogueUI.gameObject.SetActive(visible);
        }

        public override void OnDialogueStart(Dialogue dialogue)
        {
            currentCharacter = "";
            currentPortrait = "default";
        }

        public override void OnDialogueEnd()
        {
            dialogueUI.gameObject.SetActive(false);
            DialogueEnded?.Invoke();
        }

        public override void OnDialogueLine(string characterName, string dialogueText, Dictionary<string, string> tags)
        {
            // Re-show the dialogue box if hidden
            dialogueUI.gameObject.SetActive(true);

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
            DialogueLineShown?.Invoke();
        }

        public override void OnContinuedDialogue(string continuedDialogueText, Dictionary<string, string> tags)
        {
            string tagPortrait;
            if (characters.ContainsKey(currentCharacter) && GetPortraitFromTags(characters[currentCharacter], tags, out tagPortrait))
            {
                currentPortrait = tagPortrait;
            }

            dialogueUI.SetDialogueText(dialogueUI.GetDialogueText() + "\n" + continuedDialogueText);
            if (characters.ContainsKey(currentCharacter))
            {
                DialogueCharacter character = characters[currentCharacter];
                if (character.Portraits.ContainsKey(currentPortrait))
                {
                    dialogueUI.SetCharacterPortrait(character.Portraits[currentPortrait]);
                }
            }
            DialogueLineContinued?.Invoke();
        }

        public override void OnDialogueJump(Dialogue dialogue)
        {
            currentCharacter = "";
            currentPortrait = "default";
        }

        public override Task<int> OnOptionLine(string[] optionsText)
        {
            dialogueOptionsUI.gameObject.SetActive(true);

            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();

            dialogueOptionsUI.Show(optionsText, (selection) => {
                dialogueOptionsUI.Hide();
                tcs.SetResult(selection);
                dialogueEngine.NextLine();
            });

            return tcs.Task;
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
