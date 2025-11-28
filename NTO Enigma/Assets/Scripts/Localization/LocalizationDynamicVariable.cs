using System;

namespace NTO
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class LocalizationDynamicVariable : Attribute
    {
        public readonly string key;

        public LocalizationDynamicVariable(string key)
        {
            this.key = key;
        }
    }
}