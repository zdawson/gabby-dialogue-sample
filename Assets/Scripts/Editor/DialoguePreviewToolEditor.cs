using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GabbyDialogue;

[CustomEditor(typeof(DialoguePreviewTool))]
public class DialoguePreviewToolEditor : Editor
{
    private SerializedProperty dialogueScript;
    private SerializedProperty character;
    private SerializedProperty dialogueName;
    private SerializedProperty currentLine;

    private List<string> characterList;
    private Dictionary<string, List<string>> dialogueMap;
    private int selectedCharacterIndex;
    private int selectedDialogueIndex;

    void OnEnable()
    {
        dialogueScript = serializedObject.FindProperty("dialogueScript");
        character = serializedObject.FindProperty("character");
        dialogueName = serializedObject.FindProperty("dialogueName");
        currentLine = serializedObject.FindProperty("currentLine");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        characterList = null;
        dialogueMap = null;
        UpdateDropdowns();

        EditorGUILayout.PropertyField(dialogueScript);
        if (characterList != null)
        {
            selectedCharacterIndex = EditorGUILayout.Popup(selectedCharacterIndex, characterList.ToArray());
            string selectedCharacter = characterList[selectedCharacterIndex];
            character.stringValue = selectedCharacter;

            selectedDialogueIndex = EditorGUILayout.Popup(selectedDialogueIndex, dialogueMap[selectedCharacter].ToArray());
            string selectedDialogue = dialogueMap[selectedCharacter][selectedDialogueIndex];
            dialogueName.stringValue = selectedDialogue;
        }
        EditorGUILayout.PropertyField(currentLine);

        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateDropdowns()
    {
        string currentCharacter = character.stringValue;
        string currentDialogue = dialogueName.stringValue;

        // TODO find indices of these values in the script, and if they're missing, set them to index 0

        dialogueMap = new Dictionary<string, List<string>>();
        GabbyDialogueAsset dialogueAsset = dialogueScript.objectReferenceValue as GabbyDialogueAsset;

        if (!dialogueAsset)
        {
            return;
        }

        foreach (Dialogue dialogue in dialogueAsset.dialogues)
        {
            if (!dialogueMap.ContainsKey(dialogue.CharacterName))
            {
                dialogueMap[dialogue.CharacterName] = new List<string>();
            }
            dialogueMap[dialogue.CharacterName].Add(dialogue.DialogueName);
        }

        characterList = new List<string>(dialogueMap.Keys);
    }
}
