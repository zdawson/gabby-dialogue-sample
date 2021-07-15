using System;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public event Action OnForward;
    public event Action OnBack;
    public event Action OnQuit;

        
    [SerializeField]
    private TMPro.TMP_Text dialogueText = null;
    [SerializeField]
    private TMPro.TMP_Text characterText = null;
    [SerializeField]
    private UnityEngine.UI.Image characterPortrait = null;
    [SerializeField]
    private UnityEngine.UI.Button clickHandler = null;
    [SerializeField]
    private GameObject controls = null;
    [SerializeField]
    private UnityEngine.UI.Button quitButton = null;
    [SerializeField]
    private UnityEngine.UI.Button backButton = null;

    private void Awake()
    {
        clickHandler.onClick.AddListener(() => OnForward());
        backButton.onClick.AddListener(() => OnBack());
        quitButton.onClick.AddListener(() => OnQuit());
    }

    public void SetCharacter(string name)
    {
        characterText.text = name;
        if (name.Length == 0)
        {
            // TODO hide the character UI
        }
    }

    public void SetCharacterPortrait(Sprite portrait)
    {
        characterPortrait.sprite = portrait;
    }

    public string GetDialogueText()
    {
        return dialogueText.text;
    }

    public void SetDialogueText(string dialogue)
    {
        dialogueText.text = dialogue;
    }

    public void SetControlsVisible(bool visible)
    {
        controls.SetActive(visible);
    }
}
