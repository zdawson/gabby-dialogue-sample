using System.Collections.Generic;

namespace GabbyDialogueSample
{
    public class ScriptVariableStorage
    {
        Dictionary<string, object> scriptData = new Dictionary<string, object>();

        public bool HasValue(string key)
        {
            return scriptData.ContainsKey(key);
        }

        public bool HasValue<T>(string key)
        {
            return scriptData.ContainsKey(key) && scriptData[key] is T;
        }

        public T GetValue<T>(string key)
        {
            return (T) scriptData[key];
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            object result;
            if (!scriptData.TryGetValue(key, out result))
            {
                return defaultValue;
            }
            return (T) scriptData[key];
        }

        public bool TryGetValue<T>(string key, out T result, T defaultValue = default(T))
        {
            object value;
            if (scriptData.TryGetValue(key, out value))
            {
                if (value is T)
                {
                    result = (T) value;
                    return true;
                }
            }
            result = defaultValue;
            return false;
        }

        public void SetValue(string key, object value)
        {
            scriptData[key] = value;
        }
    }
}
