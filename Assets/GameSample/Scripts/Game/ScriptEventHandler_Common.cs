using PotassiumK.GabbyDialogue;
using UnityEngine;

namespace PotassiumK.GabbyDialogueSample
{
    class ScriptEventHandler_Common : AbstractScriptEventHandler
    {
        private ScriptVariableStorage storage = null;

        public ScriptEventHandler_Common(ScriptVariableStorage storage)
        {
            this.storage = storage;

            Debug.Assert(FadeController.instance != null);
        }

        [ActionHandler("set")]
        private void SetValue(string key, string value)
        {
            int intResult;
            float floatResult;
            bool boolResult;
            if (System.Int32.TryParse(value, out intResult))
            {
                storage.SetValue(key, intResult);
            }
            else if (System.Single.TryParse(value, out floatResult))
            {
                storage.SetValue(key, floatResult);
            }
            else if (System.Boolean.TryParse(value, out boolResult))
            {
                storage.SetValue(key, boolResult);
            }
            else
            {
                storage.SetValue(key, value);
            }
        }

        /// <summary>
        /// Compares the given variable with the expected value. If the variable is not defined, this always returns false.
        /// </summary>
        [ConditionalHandler]
        private bool isEqual(string key, string expected)
        {
            object actual;
            if (!storage.TryGetValue(key, out actual))
            {
                return false;
            }

            if (actual.GetType() == typeof(int))
            {
                int compareValue;
                if (!System.Int32.TryParse(expected, out compareValue))
                {
                    return false;
                }
                return System.Convert.ToInt32(actual) == compareValue;
            }
            else if (actual.GetType() == typeof(float))
            {
                float compareValue;
                if (!System.Single.TryParse(expected, out compareValue))
                {
                    return false;
                }
                return System.Convert.ToSingle(actual) == compareValue;
            }
            else if (actual.GetType() == typeof(bool))
            {
                bool compareValue;
                if (!System.Boolean.TryParse(expected, out compareValue))
                {
                    return false;
                }
                return System.Convert.ToBoolean(actual) == compareValue;
            }
            else if (actual.GetType() == typeof(string))
            {
                return System.Convert.ToString(actual) == expected;
            }

            Debug.LogError($"isEqual - unhandled type: {actual.GetType()}");
            return false;
        }

        [ConditionalHandler]
        private bool isNotEqual(string key, string expected)
        {
            return !isEqual(key, expected);
        }

        /// <summary>
        /// Checks if the given bool is true. If the variable is not defined, it is treated as false.
        /// </summary>
        [ConditionalHandler]
        private bool isTrue(string key)
        {
            return storage.GetValue<bool>(key, false);
        }

        /// <summary>
        /// Checks if the given bool is false. If the variable is not defined, it is treated as false.
        /// </summary>
        [ConditionalHandler]
        private bool isFalse(string key)
        {
            return !storage.GetValue<bool>(key, false);
        }

        [ConditionalHandler]
        private bool showRandomly(float probability)
        {
            return Random.value <= Mathf.Clamp01(probability);
        }

        [ActionHandler(autoAdvance = false)]
        private void setChatSeen(string chatName)
        {
            storage.SetValue($"ChatSeen_{chatName}", true);
        }

        [ActionHandler(autoAdvance = false)]
        private void fadeOut(float fadeTime = 0.75f)
        {
            FadeController.instance.FadeOut(fadeTime);
        }

        [ActionHandler]
        private void fadeIn(float fadeTime = 0.75f)
        {
            FadeController.instance.FadeIn(fadeTime);
        }

        [ActionHandler(autoAdvance = false)]
        private void fadeOutAndIn(float fadeTime = 0.75f, float delayTime = 1.5f)
        {
            FadeController.instance.FadeOutAndIn(fadeTime, delayTime);
        }
    }
}
