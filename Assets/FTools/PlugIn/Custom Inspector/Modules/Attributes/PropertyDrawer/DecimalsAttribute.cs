using UnityEngine;
using System.Diagnostics;

namespace CustomInspector
{
    [Conditional("UNITY_EDITOR")]
    public class DecimalsAttribute : PropertyAttribute
    {
        public readonly int amount;
        public DecimalsAttribute(int amount)
        {
            order = -10;
            this.amount = amount;
        }
    }
}