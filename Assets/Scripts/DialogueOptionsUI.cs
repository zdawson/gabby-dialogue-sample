using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOptionsUI : MonoBehaviour
{

    [SerializeField]
    private GameObject optionsContainer;
    [SerializeField]
    private UnityEngine.UI.Button optionButtonPlaceholder;

    private List<GameObject> optionButtons = new List<GameObject>();

    private void Awake()
    {
        // Hide the placeholder button
        optionButtonPlaceholder.gameObject.SetActive(false);
    }

    public void Show(string[] options, Action<int> resultCB)
    {
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
        optionsContainer.GetComponentInParent<UnityEngine.UI.ScrollRect>().verticalNormalizedPosition = 1.0f;
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
        buttonGO.GetComponentInChildren<TMPro.TMP_Text>().text = text;
        buttonGO.transform.SetParent(optionsContainer.transform, false);
        buttonGO.SetActive(true);
        return buttonGO;
    }
    
}
