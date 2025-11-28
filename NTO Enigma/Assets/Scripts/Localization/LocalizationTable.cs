using System;
using UnityEngine;

namespace NTO
{
    [CreateAssetMenu(fileName = "New Localization Table", menuName = "Configs/Localization Table", order = 0)]
    public sealed class LocalizationTable : ScriptableObject
    {
        [SerializeField] public SystemLanguage language = SystemLanguage.English;
        [SerializeField] public LocalizationVariable[] variables;

        [Serializable]
        public sealed class LocalizationVariable
        {
            [SerializeField] public string key = "key";
            [SerializeField] public LocalizationVariableChoiceMethod choiceMethod;
            [SerializeField] public ConditionAndValue[] values;
            
            [Serializable]
            public sealed class ConditionAndValue
            {
                [SerializeField] public string condition;
                [SerializeField, Multiline] public string value;
            }
        }
    }

    public enum LocalizationVariableChoiceMethod
    {
        One,
        Random,
        ByStringComparison
    }
}