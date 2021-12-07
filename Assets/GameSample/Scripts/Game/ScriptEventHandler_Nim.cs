using GabbyDialogue;
using UnityEngine;

namespace GabbyDialogueSample
{
    class ScriptEventHandler_Nim : AbstractScriptEventHandler
    {
        private ScriptVariableStorage storage = null;
        private GameObject nimUI = null;
        private UIElements nimUIElements = null;

        public ScriptEventHandler_Nim(ScriptVariableStorage storage)
        {
            this.storage = storage;

            nimUI = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("UI/NimUI"));
            Debug.Assert(nimUI != null);
            nimUI.SetActive(false);

            nimUIElements = nimUI.GetComponent<UIElements>();
            Debug.Assert(nimUIElements != null);
        }

        [ActionHandler("nim_showGameUI")]
        private void ShowUI()
        {
            TMPro.TMP_Text nimText = nimUIElements.GetElement<TMPro.TMP_Text>("StoneCountText");
            Debug.Assert(nimText != null);

            nimText.text = $"{storage.GetValue<int>("nim_stoneCount", 0)}";
            nimUI.SetActive(true);
        }

        [ActionHandler("nim_hideGameUI")]
        private void HideUI()
        {
            nimUI.SetActive(false);
        }

        [ActionHandler("nim_doCamillaTurn")]
        private void CamillaTurn()
        {
            TMPro.TMP_Text nimText = nimUIElements.GetElement<TMPro.TMP_Text>("StoneCountText");
            Debug.Assert(nimText != null);

            // Decide how many to draw
            int stoneCount = storage.GetValue<int>("nim_stoneCount");

            // Try to draw the right number of stones to ensure a victory.
            // If we can't, then just draw randomly to make it more interesting.
            int camillaDrawCount = (stoneCount - 1) % 4;
            if (camillaDrawCount == 0)
            {
                camillaDrawCount = Mathf.Min(Random.Range(1, 3), stoneCount);
            }

            // Need to set nim_camillaDrawCount for the script to reference
            storage.SetValue("nim_camillaDrawCount", camillaDrawCount);

            // Remove stones from the pile
            stoneCount -= camillaDrawCount;
            storage.SetValue("nim_stoneCount", stoneCount);

            // Update the UI
            nimText.text = $"{stoneCount}";
        }

        [ActionHandler("nim_doPlayerTurn")]
        private void PlayerTurn(int drawCount)
        {
            TMPro.TMP_Text nimText = nimUIElements.GetElement<TMPro.TMP_Text>("StoneCountText");
            Debug.Assert(nimText != null);

            // Remove stones from the pile
            int stoneCount = storage.GetValue<int>("nim_stoneCount");
            stoneCount -= drawCount;
            storage.SetValue("nim_stoneCount", stoneCount);

            // Update the UI
            nimText.text = $"{stoneCount}";
        }
    }
}
