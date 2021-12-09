using GabbyDialogue;

namespace GabbyDialogueSample
{
    class ScriptEventHandler_LocalizationSample : AbstractScriptEventHandler
    {
        private SimpleDialogueSystem dialogueSystem = null;

        public ScriptEventHandler_LocalizationSample(SimpleDialogueSystem dialogueSystem)
        {
            this.dialogueSystem = dialogueSystem;
        }

        [ActionHandler]
        private void setLanguage(string language)
        {
            dialogueSystem.SetLanguage(language);
        }
    }
}
