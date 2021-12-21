using UnityEngine;

namespace PotassiumK.GabbyDialogueSample
{
    public class UIElements : MonoBehaviour
    {
        [System.Serializable]
        private struct NamedUIElement
        {
            public string elementName;
            public MonoBehaviour UIElement;
        }
        [SerializeField]
        private NamedUIElement[] uiFields = new NamedUIElement[0];

        public T GetElement<T>(string name) where T : MonoBehaviour
        {
            foreach (NamedUIElement el in uiFields)
            {
                if (el.elementName == name && el.UIElement is T)
                {
                    return el.UIElement as T;
                }
            }
            return default(T);
        }
    }
}
