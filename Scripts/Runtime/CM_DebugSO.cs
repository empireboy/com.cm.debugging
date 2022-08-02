using System;
using System.Collections.Generic;
using UnityEngine;

namespace CM.Debugging
{
    [CreateAssetMenu(fileName = "CM_Debug", menuName = "CM/Debug SO")]
    public class CM_DebugSO : ScriptableObject
    {
        public List<Category> categories = new List<Category>();

        public string logFormat = "[{0}] {1}";

        [Serializable]
        public class Category
        {
            public string name;
            public bool enabled;

            public Category(string name, bool enabled)
            {
                this.name = name;
                this.enabled = enabled;
            }
        }
    }
}