using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GabbyDialogue;

public class DialoguePreviewTool : MonoBehaviour
{

    [SerializeField]
    private GabbyDialogueAsset dialogueScript;
    [SerializeField]
    private string character;
    [SerializeField]
    private string dialogueName;
    [SerializeField]
    private uint currentLine = 1;

    private void OnValidate()
    {
        if (currentLine < 1)
        {
            currentLine = 1;
        }
        // TODO currentLine needs to track the line number of the actual script file, and validate that the line is playable when jumping to a line.
    }

}
