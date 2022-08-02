using System.Collections.Generic;
using UnityEngine;

namespace CM.Debugging
{
    public static class CM_Debug
    {
        public static string logFormat = "[{0}] {1}";

        private static Dictionary<string, bool> _categories = new Dictionary<string, bool>();

        public static void Log(object category, object message)
        {
            if (!IsCategoryEnabled(category))
                return;

            Debug.LogFormat(logFormat, category, message);
        }

        public static void LogWarning(object category, object message)
        {
            if (!IsCategoryEnabled(category))
                return;

            Debug.LogWarningFormat(logFormat, category, message);
        }

        public static void LogError(object category, object message)
        {
            if (!IsCategoryEnabled(category))
                return;

            Debug.LogErrorFormat(logFormat, category, message);
        }

        public static void SetCategories(Dictionary<string, bool> categories)
        {
            _categories = categories;
        }

        public static void EnableCategory(object category, bool enabled)
        {
            _categories[category.ToString()] = enabled;
        }

        public static bool IsCategoryEnabled(object category)
        {
            if (!_categories.ContainsKey(category.ToString()))
                return false;

            if (_categories[category.ToString()])
                return true;

            return false;
        }
    }
}