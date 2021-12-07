using GabbyDialogue;
using UnityEngine;

namespace GabbyDialogueSample
{
    class ScriptEventHandler_Tutorial : AbstractScriptEventHandler
    {
        private GameObject tutorialUI = null;
        private UIElements tutorialUIElements = null;

        public ScriptEventHandler_Tutorial()
        {
            tutorialUI = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("UI/TutorialUI"));
            Debug.Assert(tutorialUI != null);
            tutorialUI.SetActive(false);

            tutorialUIElements = tutorialUI.GetComponent<UIElements>();
            Debug.Assert(tutorialUIElements != null);
        }

        [ActionHandler]
        private void showSnippet(string snippetImageName)
        {
            Sprite snippetImage = Resources.Load<Sprite>(snippetImageName);

            if (snippetImage == null)
            {
                Debug.LogError($"Could not find resource for showSnippet: {snippetImageName}");
                return;
            }

            UnityEngine.UI.Image imageUI = tutorialUIElements.GetElement<UnityEngine.UI.Image>("TutorialImage");
            Debug.Assert(imageUI != null);

            imageUI.sprite = snippetImage;

            tutorialUI.SetActive(true);
        }

        [ActionHandler]
        private void hideSnippet()
        {
            tutorialUI.SetActive(false);
        }
    }
}
