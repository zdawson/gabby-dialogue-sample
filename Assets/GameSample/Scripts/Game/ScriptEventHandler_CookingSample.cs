using PotassiumK.GabbyDialogue;

namespace PotassiumK.GabbyDialogueSample
{
    class ScriptEventHandler_CookingSample : AbstractScriptEventHandler
    {
        private ScriptVariableStorage storage = null;

        public ScriptEventHandler_CookingSample(ScriptVariableStorage storage)
        {
            this.storage = storage;
        }

        [ConditionalHandler]
        private bool cookingQuality(int requiredScore)
        {
            int overallScore = 0;

            bool step1Success;
            if (storage.TryGetValue<bool>("camilla.cookingStep1Success", out step1Success) && step1Success)
            {
                ++overallScore;
            }

            bool step2Success;
            if (storage.TryGetValue("camilla.cookingStep2Success", out step2Success) && step2Success)
            {
                ++overallScore;
            }

            return overallScore == requiredScore;
        }

        [ConditionalHandler]
        private bool cookingStep1Failed()
        {
            bool success;
            if (storage.TryGetValue<bool>("camilla.cookingStep1Success", out success))
            {
                return !success;
            }
            return false;
        }

        [ConditionalHandler]
        private bool cookingStep2Failed()
        {
            bool success;
            if (storage.TryGetValue<bool>("camilla.cookingStep2Success", out success))
            {
                return !success;
            }
            return false;
        }
    }
}
