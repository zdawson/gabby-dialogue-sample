﻿using System;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public event Action OnForward;
    public event Action OnBack;
    public event Action OnQuit;
    
    [SerializeField]
    private UnityEngine.UI.Text dialogueText = null;
    [SerializeField]
    private UnityEngine.UI.Text characterText = null;
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
    }

    public void SetCharacter(string name)
    {
        characterText.text = name;
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
