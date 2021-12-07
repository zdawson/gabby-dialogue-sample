using GabbyDialogue;
using UnityEngine;

namespace GabbyDialogueSample
{
    class ScriptEventHandler_VendorSample : AbstractScriptEventHandler
    {
        private ScriptVariableStorage storage = null;
        private GameObject goldUI = null;
        private UIElements goldUIElements = null;

        public ScriptEventHandler_VendorSample(ScriptVariableStorage storage)
        {
            this.storage = storage;

            goldUI = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("UI/GoldUI"));
            Debug.Assert(goldUI != null);
            goldUI.SetActive(false);

            goldUIElements = goldUI.GetComponent<UIElements>();
            Debug.Assert(goldUIElements != null);
        }

        [ConditionalHandler]
        private bool canAffordItem()
        {
            int gold = storage.GetValue<int>("camilla.vendor.gold");
            int cost = storage.GetValue<int>("camilla.vendor.cost");
            return gold >= cost;
        }

        [ActionHandler]
        private void purchaseItem()
        {
            TMPro.TMP_Text uiText = goldUIElements.GetElement<TMPro.TMP_Text>("GoldCountText");
            Debug.Assert(uiText != null);

            int gold = storage.GetValue<int>("camilla.vendor.gold");
            int cost = storage.GetValue<int>("camilla.vendor.cost");
            gold -= cost;
            storage.SetValue("camilla.vendor.gold", gold);

            string item = storage.GetValue<string>("camilla.vendor.item");
            Debug.Log($"Purchased {item} for {cost} gold.");

            uiText.text = $"{gold}";
        }

        [ActionHandler]
        private void showGold()
        {
            TMPro.TMP_Text uiText = goldUIElements.GetElement<TMPro.TMP_Text>("GoldCountText");
            Debug.Assert(uiText != null);

            int gold = storage.GetValue<int>("camilla.vendor.gold");
            uiText.text = $"{gold}";
            goldUI.SetActive(true);
        }

        [ActionHandler]
        private void hideGold()
        {
            goldUI.SetActive(false);
        }
    }
}
