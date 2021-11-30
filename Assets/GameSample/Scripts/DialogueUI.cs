using System;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public event Action OnForward;
    public event Action OnBack;
    public event Action OnQuit;

    [SerializeField]
    private GameObject dialoguePanel = null;    
    [SerializeField]
    private TMPro.TMP_Text dialogueText = null;
    [SerializeField]
    private TMPro.TMP_Text characterText = null;
    [SerializeField]
    private UnityEngine.UI.Image characterPortrait = null;

    [SerializeField]
    private GameObject narrationPanel = null;    
    [SerializeField]
    private TMPro.TMP_Text narrationText = null;

    [SerializeField]
    private UnityEngine.UI.Button clickHandler = null;
    [SerializeField]
    private GameObject controls = null;
    [SerializeField]
    private UnityEngine.UI.Button quitButton = null;
    [SerializeField]
    private UnityEngine.UI.Button backButton = null;

    private Vector2 mouseDownPos;

    private void Awake()
    {
        clickHandler.onClick.AddListener(() => {
            if (GabbyDialogueSample.SampleDialogueSystem.instance().AllowAdvancingDialogue)
            {
                OnForward();
            }
        });
        backButton.onClick.AddListener(() => OnBack());
        quitButton.onClick.AddListener(() => OnQuit());
    }

    private void Update()
    {
        if (!GabbyDialogueSample.SampleDialogueSystem.instance().AllowAdvancingDialogue)
        {
            return;
        }

        // Allow clicking anywhere to advance dialogue, but only if the mouse isn't dragged past a certain threshold
        // The click handler UI panel will catch most clicks, but this allows for clicking the text to advance while not messing up scrolling
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 mouseUpPos = Input.mousePosition;
            if ((mouseUpPos - mouseDownPos).sqrMagnitude < 10)
            {
                OnForward();
                return;
            }
        }

        // Handle input
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            OnForward();
            return;
        }
    }

    public void SetCharacter(string name)
    {
        characterText.text = name;
        if (name.Length == 0)
        {
            dialoguePanel.SetActive(false);
            narrationPanel.SetActive(true);
        }
        else
        {
            dialoguePanel.SetActive(true);
            narrationPanel.SetActive(false);
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
        narrationText.text = dialogue;
    }

    public void SetControlsVisible(bool visible)
    {
        controls.SetActive(visible);
    }
}
