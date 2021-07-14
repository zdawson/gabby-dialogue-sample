using GabbyDialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GabbyDialogueSample
{
    public class DialogueLoader : MonoBehaviour
    {
        /// <summary>
        /// The list of Gabby files to load dialogues from.
        /// The menu is populated with the loaded dialogues, which are then run through the DialogueEngine once selected.
        /// </summary>
        [SerializeField]
        private GabbyDialogueAsset[] gabbyFiles = null; // TODO rename GabbyDialogueAsset. Maybe DialogueScript?

        void Awake()
        {
            // Add dialogue assets to the dialogue system
            foreach (GabbyDialogueAsset asset in gabbyFiles)
            {
                SampleDialogueSystem.instance().AddDialogueAsset(asset);
            }
        }
    }
}
