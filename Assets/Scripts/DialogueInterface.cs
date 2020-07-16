using System;
using UnityEngine;

public class DialogueInterface : MonoBehaviour
{
    public event Action onBack;

    [SerializeField]
    private UnityEngine.UI.Text dialogueText = null;
    [SerializeField]
    private UnityEngine.UI.Text characterText = null;
    [SerializeField]
    private UnityEngine.UI.Image characterPortrait = null;

    public void SetCharacter(string name, Sprite portrait)
    {
        characterText.text = name;
        characterPortrait.sprite = portrait;
    }

    public void SetDialogueText(string dialogue)
    {
        dialogueText.text = dialogue;
    }
}
