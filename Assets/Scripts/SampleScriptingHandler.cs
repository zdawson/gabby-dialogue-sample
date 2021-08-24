using GabbyDialogue;
using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace GabbyDialogueSample
{
    public class SampleScriptingHandler : AbstractScriptingHandler
    {
        Dictionary<string, ActionParameter> scriptData = new Dictionary<string, ActionParameter>();

        public SampleScriptingHandler()
        {
            // For clarity, C# handler action implementations have been named the same name as they appear in the Gabby scripts

            // General actions
            // ===============
            this.AddActionHandler(nameof(fadeOut), fadeOut);
            this.AddActionHandler(nameof(fadeIn), fadeIn);
            this.AddActionHandler(nameof(fadeOutAndIn), fadeOutAndIn);

            this.AddActionHandler(nameof(set), set);
            this.AddConditionalHandler(nameof(isEqual), isEqual);
            this.AddConditionalHandler(nameof(isNot), isNot);

            // Cooking sample actions
            // ======================
            this.AddConditionalHandler("cookingQuality", (List<ActionParameter> parameters) =>
            {
                int overallScore = 0;

                ActionParameter step1Success;
                if (scriptData.TryGetValue("camilla.cookingStep1Success", out step1Success) && Convert.ToBoolean(step1Success.value))
                {
                    ++overallScore;
                }

                ActionParameter step2Success;
                if (scriptData.TryGetValue("camilla.cookingStep2Success", out step2Success) && Convert.ToBoolean(step2Success.value))
                {
                    ++overallScore;
                }

                return overallScore == Convert.ToInt32(parameters[0].value);
            });
            this.AddConditionalHandler("cookingStep1Failed", (List<ActionParameter> parameters) =>
            {
                ActionParameter step1Success;
                if (scriptData.TryGetValue("camilla.cookingStep1Success", out step1Success))
                {
                    return !Convert.ToBoolean(step1Success.value);
                }
                return true;
            });
            this.AddConditionalHandler("cookingStep2Failed", (List<ActionParameter> parameters) =>
            {
                ActionParameter step2Success;
                if (scriptData.TryGetValue("camilla.cookingStep2Success", out step2Success))
                {
                    return !Convert.ToBoolean(step2Success.value);
                }
                return true;
            });

            // Nim sample actions
            // ==================
            GameObject gameUI = GameObject.Instantiate(Resources.Load("Prefabs/NimUI", typeof(GameObject))) as GameObject;
            TMPro.TMP_Text nimText = gameUI.GetComponentInChildren<TMPro.TMP_Text>();
            gameUI.SetActive(false);

            this.AddActionHandler("nim_showGameUI", (parameters) =>
            {
                nimText.text = $"{Convert.ToInt32(scriptData["nim_stoneCount"].value)}";
                gameUI.SetActive(true);
                return new ActionResult {handled = true, autoAdvance = true};
            });

            this.AddActionHandler("nim_hideGameUI", (parameters) =>
            {
                gameUI.SetActive(false);
                return new ActionResult {handled = true, autoAdvance = true};
            });

            this.AddActionHandler("nim_doCamillaTurn", (List<ActionParameter> parameters) =>
            {
                // Decide how many to draw
                int stoneCount = Convert.ToInt32(scriptData["nim_stoneCount"].value);

                // Try to draw the right number of stones to ensure a victory.
                // If we can't, then just draw randomly to make it more interesting.
                int camillaDrawCount = (stoneCount - 1) % 4;
                if (camillaDrawCount == 0)
                {
                    camillaDrawCount = Math.Min(Random.Range(1, 3), stoneCount);
                }

                // Need to set nim_camillaDrawCount for the script to reference
                if (!scriptData.ContainsKey("nim_camillaDrawCount"))
                {
                    scriptData["nim_camillaDrawCount"] = new ActionParameter {type = ParameterType.Number, value = camillaDrawCount};
                }
                else
                {
                    scriptData["nim_camillaDrawCount"].value = camillaDrawCount;
                }

                // Remove stones from the pile
                stoneCount -= camillaDrawCount;
                scriptData["nim_stoneCount"].value = stoneCount;

                // Update the UI
                nimText.text = $"{stoneCount}";

                return new ActionResult {handled = true, autoAdvance = true};
            });

            this.AddActionHandler("nim_doPlayerTurn", (List<ActionParameter> parameters) =>
            {
                // Remove stones from the pile
                int stoneCount = Convert.ToInt32(scriptData["nim_stoneCount"].value);
                stoneCount -= Convert.ToInt32(parameters[0].value);
                scriptData["nim_stoneCount"].value = stoneCount;

                // Update the UI
                nimText.text = $"{stoneCount}";

                return new ActionResult {handled = true, autoAdvance = true};
            });

            // Vendor scripts
            // ==============
            GameObject itemUI = GameObject.Instantiate(Resources.Load("Prefabs/ItemPopupUI", typeof(GameObject))) as GameObject;
            GameObject goldUI = GameObject.Instantiate(Resources.Load("Prefabs/GoldUI", typeof(GameObject))) as GameObject;
            goldUI.SetActive(false);
            itemUI.SetActive(false);

            this.AddConditionalHandler("canAffordItem", (parameters) =>
            {
                int gold = Convert.ToInt32(scriptData["camilla.vendor.gold"].value);
                int cost = Convert.ToInt32(scriptData["camilla.vendor.cost"].value);
                
                return gold >= cost;
            });

            this.AddActionHandler("purchaseItem", (parameters) =>
            {
                int gold = Convert.ToInt32(scriptData["camilla.vendor.gold"].value);
                int cost = Convert.ToInt32(scriptData["camilla.vendor.cost"].value);
                gold -= cost;
                scriptData["camilla.vendor.gold"].value = gold;

                string item = Convert.ToString(scriptData["camilla.vendor.item"].value);
                Debug.Log($"Purchased {item} for {cost} gold.");

                TMPro.TMP_Text uiText = goldUI.GetComponentInChildren<TMPro.TMP_Text>();
                uiText.text = $"{gold}";

                RectTransform rt = itemUI.GetComponent<RectTransform>();
                float offset = rt.rect.height;
                rt.Translate(new Vector3(0.0f, -offset, 0.0f));
                itemUI.transform.localPosition = new Vector3();

                return new ActionResult {handled = true, autoAdvance = true};
            });

            this.AddActionHandler("showGold", (parameters) =>
            {
                int gold = Convert.ToInt32(scriptData["camilla.vendor.gold"].value);
                TMPro.TMP_Text uiText = goldUI.GetComponentInChildren<TMPro.TMP_Text>();
                uiText.text = $"{gold}";
                goldUI.SetActive(true);
                return new ActionResult {handled = true, autoAdvance = true};
            });

            this.AddActionHandler("hideGold", (parameters) =>
            {
                goldUI.SetActive(false);
                return new ActionResult {handled = true, autoAdvance = true};
            });

            // Test stuff to remove eventually
            // ===============================
            this.AddActionHandler(nameof(action), action);
            this.AddActionHandler(nameof(actionString), actionString);
            this.AddActionHandler(nameof(actionNumber), actionNumber);
            this.AddActionHandler(nameof(actionBool), actionBool);
            this.AddActionHandler(nameof(actionMultiple), actionMultiple);

            this.AddConditionalHandler(nameof(showRandomly), showRandomly);
        }

        // TODO add an attribute like:
        // [GabbyAction]
        // And maybe it can do the parameters -> proper parameter list conversion behind the scenes
        private ActionResult action(List<ActionParameter> parameters)
        {
            Debug.Log("action");
            return new ActionResult {handled = true, autoAdvance = true};
        }

        private ActionResult actionString(List<ActionParameter> parameters)
        {
            Debug.Log("actionString");
            foreach (ActionParameter p in parameters)
            {
                Debug.Log(Convert.ToString(p.value));
            }
            return new ActionResult {handled = true, autoAdvance = true};
        }

        private ActionResult actionNumber(List<ActionParameter> parameters)
        {
            Debug.Log("actionNumber");
            foreach (ActionParameter p in parameters)
            {
                Debug.Log(Convert.ToSingle(p.value));
            }
            return new ActionResult {handled = true, autoAdvance = true};
        }

        private ActionResult actionBool(List<ActionParameter> parameters)
        {
            Debug.Log("actionBool");
            foreach (ActionParameter p in parameters)
            {
                Debug.Log(Convert.ToBoolean(p.value));
            }
            return new ActionResult {handled = true, autoAdvance = true};
        }

        private ActionResult actionMultiple(List<ActionParameter> parameters)
        {
            Debug.Log("actionMultiple");
            foreach (ActionParameter p in parameters)
            {
                Debug.Log(Convert.ToString(p.value));
            }
            return new ActionResult {handled = true, autoAdvance = true};
        }

        private ActionResult set(List<ActionParameter> parameters)
        {
            if (parameters.Count != 2)
            {
                Debug.LogError($"Incorrect number of parameters in call to set: expected 2, got {parameters.Count}");
            }
            ActionParameter parameterKey = parameters[0];
            ActionParameter parameterValue = parameters[1];

            string key = Convert.ToString(parameterKey.value);
            scriptData[key] = parameterValue;

            return new ActionResult {handled = true, autoAdvance = true};
        }

        private bool isEqual(List<ActionParameter> parameters)
        {
            if (parameters.Count != 2)
            {
                Debug.LogError($"Incorrect number of parameters in call to is: expected 2, got {parameters.Count}");
            }
            ActionParameter parameterKey = parameters[0];
            ActionParameter parameterExpected = parameters[1];
            ActionParameter parameterActual;

            string key = Convert.ToString(parameterKey.value);
            if (!scriptData.TryGetValue(key, out parameterActual))
            {
                return false;
            }
            if (parameterExpected.type != parameterActual.type)
            {
                Debug.LogError($"Call to `isEqual({key}, {parameterExpected.value.ToString()})` compares values of different types: {parameterExpected.type}, {parameterActual.type}");
            }
            switch (parameterExpected.type)
            {
                case ParameterType.String:
                {
                    return Convert.ToString(parameterExpected.value).Equals(Convert.ToString(parameterActual.value));
                }
                case ParameterType.Number:
                {
                    float e = Convert.ToSingle(parameterExpected.value);
                    float a = Convert.ToSingle(parameterActual.value);
                    return Mathf.Approximately(a, e);
                }
                case ParameterType.Boolean:
                {
                    return Convert.ToBoolean(parameterExpected.value) == Convert.ToBoolean(parameterActual.value);
                }
            }
            return false;
        }

        private bool isNot(List<ActionParameter> parameters)
        {
            return !isEqual(parameters);
        }

        private bool showRandomly(List<ActionParameter> parameters)
        {
            if (parameters.Count != 1)
            {
                Debug.LogError("Invalid call to showRandomly: Must provide a single parameter for probability as a number from 0 to 1.");
            }
            float probability = Mathf.Clamp01(Convert.ToSingle(parameters[0].value));
            return Random.value <= probability;
        }

        private ActionResult fadeOut(List<ActionParameter> parameters)
        {
            // TODO do something with inputs
            if (FadeController.instance == null)
            {
                return new ActionResult {handled = false};
            }
            FadeController.instance.FadeOut();

            return new ActionResult {handled = true, autoAdvance = false};
        }

        private ActionResult fadeIn(List<ActionParameter> parameters)
        {
            // TODO do something with inputs
            if (FadeController.instance == null)
            {
                return new ActionResult {handled = false};
            }
            FadeController.instance.FadeIn();

            return new ActionResult {handled = true, autoAdvance = true};
        }

        private ActionResult fadeOutAndIn(List<ActionParameter> parameters)
        {
            // TODO do something with inputs
            if (FadeController.instance == null)
            {
                return new ActionResult {handled = false};
            }
            FadeController.instance.FadeOutAndIn();

            return new ActionResult {handled = true, autoAdvance = false};
        }
    }
}
