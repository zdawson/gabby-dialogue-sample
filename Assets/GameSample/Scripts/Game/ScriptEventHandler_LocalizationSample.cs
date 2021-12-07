using GabbyDialogue;
using UnityEngine;

namespace GabbyDialogueSample
{
    class ScriptEventHandler_LocalizationSample : AbstractScriptEventHandler
    {
        private ScriptVariableStorage storage = null;


        public ScriptEventHandler_LocalizationSample(ScriptVariableStorage storage)
        {
            this.storage = storage;
        }
    }
}
