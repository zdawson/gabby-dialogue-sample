using GabbyDialogue;
using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace GabbyDialogueSample
{
    public class SampleScriptingHandler : AbstractScriptingHandler
    {
        public SampleScriptingHandler()
        {
            // For clarity, C# handlers have the same name as their Gabby action counterparts
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
        private void action(List<ActionParameter> parameters)
        {
            Debug.Log("action");
        }

        private void actionString(List<ActionParameter> parameters)
        {
            Debug.Log("actionString");
            foreach (ActionParameter p in parameters)
            {
                Debug.Log(Convert.ToString(p.value));
            }
        }

        private void actionNumber(List<ActionParameter> parameters)
        {
            Debug.Log("actionNumber");
            foreach (ActionParameter p in parameters)
            {
                Debug.Log(Convert.ToSingle(p.value));
            }
        }

        private void actionBool(List<ActionParameter> parameters)
        {
            Debug.Log("actionBool");
            foreach (ActionParameter p in parameters)
            {
                Debug.Log(Convert.ToBoolean(p.value));
            }
        }

        private void actionMultiple(List<ActionParameter> parameters)
        {
            Debug.Log("actionMultiple");
            foreach (ActionParameter p in parameters)
            {
                Debug.Log(Convert.ToString(p.value));
            }
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
    }
}
