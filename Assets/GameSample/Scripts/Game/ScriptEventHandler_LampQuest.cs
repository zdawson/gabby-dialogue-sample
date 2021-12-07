using GabbyDialogue;
using UnityEngine;

namespace GabbyDialogueSample
{
    class ScriptEventHandler_LampQuest : AbstractScriptEventHandler
    {
        private ScriptVariableStorage storage = null;
        private GameObject questUI = null;
        private UIElements questUIElements = null;
        private GameObject lampOn = null;
        private GameObject lampOff = null;

        public ScriptEventHandler_LampQuest(ScriptVariableStorage storage)
        {
            this.storage = storage;

            questUI = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("UI/QuestUI"));
            Debug.Assert(questUI != null);
            questUI.SetActive(false);

            questUIElements = questUI.GetComponent<UIElements>();
            Debug.Assert(questUIElements != null);

            lampOn = GameObject.Find("LampOn") as GameObject;
            Debug.Assert(lampOn != null);
            lampOff = GameObject.Find("LampOff") as GameObject;
            Debug.Assert(lampOff != null);

            Debug.Assert(FadeController.instance != null);

            lampOff.SetActive(false);
        }

        [ActionHandler]
        private void setQuestStage(string questName, string questStage)
        {
            if (questName != "lampQuest")
            {
                return;
            }

            switch (questStage)
            {
                case "lightBurntOut":
                {
                    // Update the scene
                    lampOn.SetActive(false);
                    lampOff.SetActive(true);
                    FadeController.instance.Flash();

                    break;
                }
                case "returnToCamilla":
                {
                    // Update the scene
                    lampOn.SetActive(true);
                    lampOff.SetActive(false);

                    break;
                }
            }

            storage.SetValue($"quest.{questName}.currentStage", questStage);
        }

        [ActionHandler]
        private void setQuestObjectiveText(string objectiveText)
        {
            TMPro.TMP_Text uiText = questUIElements.GetElement<TMPro.TMP_Text>("ObjectiveText");
            Debug.Assert(uiText != null);

            uiText.text = objectiveText;
            questUI.SetActive(objectiveText.Length > 0);
        }

        [ConditionalHandler]
        private bool isQuestStageEqual(string questName, string targetQuestStage)
        {
            string currentQuestStage;
            if (!storage.TryGetValue<string>($"quest.{questName}.currentStage", out currentQuestStage))
            {
                return false;
            }

            return currentQuestStage == targetQuestStage;
        }

        [ConditionalHandler]
        private bool isQuestActive(string questName)
        {
            string currentQuestStage;
            if (!storage.TryGetValue<string>($"quest.{questName}.currentStage", out currentQuestStage))
            {
                return false;
            }

            return currentQuestStage != "" && currentQuestStage != "complete";
        }
    }
}
