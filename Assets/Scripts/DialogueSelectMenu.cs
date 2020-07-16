using System.Collections.Generic;
using UnityEngine;
using GabbyDialogue;

namespace GabbyDialogueSample
{
    public class DialogueSelectMenu : MonoBehaviour
    {
        /// <summary>
        /// The list of Gabby files to load dialogues from.
        /// The menu is populated with the loaded dialogues, which are then run through the DialogueEngine once selected.
        /// </summary>
        [SerializeField]
        private GabbyDialogueAsset[] gabbyFiles = null; // TODO rename GabbyDialogueAsset. Maybe DialogueScript?

        [SerializeField]
        private UnityEngine.UI.ScrollRect characterList = null;
        [SerializeField]
        private UnityEngine.UI.ScrollRect dialogueList = null;
        [SerializeField]
        private UnityEngine.UI.Toggle characterButtonPlaceholder = null;
        [SerializeField]
        private UnityEngine.UI.Button dialogueButtonPlaceholder = null;

        private Dictionary<string, List<GameObject>> dialogueButtonMap = new Dictionary<string, List<GameObject>>();
        private string currentCharacter = "";

        void Start()
        {
            // Initialize the list of characters and their dialogues
            UnityEngine.UI.Toggle firstCharacterButton = null;
            HashSet<string> loadedCharacters = new HashSet<string>();
            foreach (GabbyDialogueAsset dialogueAsset in gabbyFiles)
            {
                foreach (Dialogue dialogue in dialogueAsset.dialogues)
                {
                    if (!loadedCharacters.Contains(dialogue.CharacterName))
                    {
                        GameObject characterButton = CreateCharacterButton(dialogue.CharacterName);
                        dialogueButtonMap.Add(dialogue.CharacterName, new List<GameObject>());
                        loadedCharacters.Add(dialogue.CharacterName);
                        if (!firstCharacterButton)
                        {
                            firstCharacterButton = characterButton.GetComponent<UnityEngine.UI.Toggle>();
                        }
                    }

                    dialogueButtonMap[dialogue.CharacterName].Add(CreateDialogueButton(dialogue));
                }
            }

            // Hide the placeholder buttons
            characterButtonPlaceholder.gameObject.SetActive(false);
            dialogueButtonPlaceholder.gameObject.SetActive(false);

            // Select the first character
            if (firstCharacterButton)
            {
                firstCharacterButton.isOn = false;
                firstCharacterButton.isOn = true;
                firstCharacterButton.Select();
                firstCharacterButton.OnSelect(null);
            }
        }

        private GameObject CreateCharacterButton(string name)
        {
            GameObject buttonGO = Instantiate(characterButtonPlaceholder.gameObject);
            UnityEngine.UI.Toggle button = buttonGO.GetComponent<UnityEngine.UI.Toggle>();
            buttonGO.GetComponentInChildren<UnityEngine.UI.Text>().text = name;
            buttonGO.transform.SetParent(characterList.content);
            button.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    ShowDialoguesForCharacter(name);
                }
            });
            return buttonGO;
        }

        private GameObject CreateDialogueButton(Dialogue dialogue)
        {
            GameObject buttonGO = Instantiate(dialogueButtonPlaceholder.gameObject);
            UnityEngine.UI.Button button = buttonGO.GetComponent<UnityEngine.UI.Button>();
            buttonGO.GetComponentInChildren<UnityEngine.UI.Text>().text = dialogue.DialogueName;
            buttonGO.transform.SetParent(dialogueList.content);
            buttonGO.SetActive(false);
            button.onClick.AddListener(() => PlayDialogue(dialogue));
            return buttonGO;
        }

        private void ShowDialoguesForCharacter(string character)
        {
            if (character == currentCharacter)
            {
                return;
            }

            if (dialogueButtonMap.ContainsKey(currentCharacter))
            {
                List<GameObject> dialogueButtons = dialogueButtonMap[currentCharacter];
                foreach (GameObject dialogueButton in dialogueButtons)
                {
                    dialogueButton.SetActive(false);
                }
            }

            if (dialogueButtonMap.ContainsKey(character))
            {
                List<GameObject> dialogueButtons = dialogueButtonMap[character];
                foreach (GameObject dialogueButton in dialogueButtons)
                {
                    dialogueButton.SetActive(true);
                }
            }

            currentCharacter = character;
        }

        private void PlayDialogue(Dialogue dialogue)
        {
            gameObject.SetActive(false);
            SampleDialogueSystem.instance().PlayDialogue(dialogue); // TODO return an async operation?
        }
    }
}
