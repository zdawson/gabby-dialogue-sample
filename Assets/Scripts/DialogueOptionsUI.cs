using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOptionsUI : MonoBehaviour
{

    [SerializeField]
    private GameObject optionsContainer;
    [SerializeField]
    private UnityEngine.UI.Text lastLineText;
    [SerializeField]
    private UnityEngine.UI.Button optionButtonPlaceholder;

    private List<GameObject> optionButtons = new List<GameObject>();

    private void Awake()
    {
        // Hide the placeholder button
        optionButtonPlaceholder.gameObject.SetActive(false);
    }

    public void Show(string lastLineCharacter, string lastLineText, string[] options, Action<int> resultCB)
    {
        this.lastLineText.text = $"{lastLineCharacter}: {lastLineText}";
        for (int i = 0; i < options.Length; ++i)
        {
            GameObject buttonGO = CreateOptionButton(options[i]);
            optionButtons.Add(buttonGO);

            UnityEngine.UI.Button button = buttonGO.GetComponent<UnityEngine.UI.Button>();
            int buttonIndex = i;
            button.onClick.AddListener(() => {
                resultCB(buttonIndex);
            });
        }
    }

    public void Hide()
    {
        foreach (GameObject button in optionButtons) {
            GameObject.Destroy(button);
        }
        optionButtons.Clear();
        this.gameObject.SetActive(false);
    }

    private GameObject CreateOptionButton(string text)
    {
        GameObject buttonGO = Instantiate(optionButtonPlaceholder.gameObject);
        buttonGO.GetComponentInChildren<UnityEngine.UI.Text>().text = text;
        buttonGO.transform.SetParent(optionsContainer.transform);
        buttonGO.SetActive(true);
        return buttonGO;
    }
    
}
