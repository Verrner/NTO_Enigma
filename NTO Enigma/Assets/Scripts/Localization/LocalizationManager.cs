using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace NTO
{
    public sealed class LocalizationManager : MonoBehaviour, ISavingReceiver
    {
        private static LocalizationManager _instance;
        
        [SerializeField] private LocalizationTable[] tables;
        [SerializeField] private SystemLanguage language = SystemLanguage.English;

        private Dictionary<SystemLanguage, LocalizationTable> _tablesDictionary;
        private LocalizationTable _table;

        public static event Action LanguageChanged;

        public static SystemLanguage Language
        {
            set
            {
                _instance.language = value;
                LanguageChanged?.Invoke();
            }
        }
        
        private void Awake()
        {
            if (_instance == null) _instance = this;
            _tablesDictionary = tables.ToDictionary(table => table.language);
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                Language = SystemLanguage.English;
            else if (Input.GetKeyDown(KeyCode.R))
                Language = SystemLanguage.Russian;
        }

        public static string GetValue(string key, object dynamicVariablesSource)
        {
            var variable = _instance._tablesDictionary[_instance.language].variables.ToList().Find(variable => variable.key == key);
            if (variable == null) throw new Exception($"variable \"{key}\" not found");
            var format = variable.choiceMethod switch
            {
                LocalizationVariableChoiceMethod.One => variable.values[0].value,
                LocalizationVariableChoiceMethod.Random => _instance.ChoseByRandom(variable.values),
                _ => _instance.ChoseByStringComparison(variable.values, dynamicVariablesSource)
            };
            return _instance.PasteDynamicVariables(format, dynamicVariablesSource);
        }

        private string ChoseByRandom(LocalizationTable.LocalizationVariable.ConditionAndValue[] values)
        {
            foreach (var value in values)
            {
                var parts = value.condition.Split('|');
                if (parts.Length == 0) throw new Exception("condition is null");
                if (float.Parse(parts[0]) <= Random.Range(0, parts.Length == 1 ? 101 : float.Parse(parts[1]))) return value.value;
            }

            return values.Last().value;
        }

        private string ChoseByStringComparison(LocalizationTable.LocalizationVariable.ConditionAndValue[] values,
            object dynamicVariablesSource)
        {
            foreach (var value in values)
            {
                var parts = value.condition.Split('|');
                if (parts.Length != 2) throw new Exception("condition is invalid");
                if (parts[0] == GetVariableFromDynamicSource(parts[1], dynamicVariablesSource)) return value.value;
            }

            return "";
        }

        private string PasteDynamicVariables(string format, object source)
        {
            var result = format;
            var wordsToReplace = format.Split(' ')
                .Where(word => word.Length >= 3 && word[0] == '{' && word.Last() == '}').Distinct();
            foreach (var word in wordsToReplace)
            {
                var key = word.Substring(1, word.Length - 2);
                var value = GetVariableFromDynamicSource(key, source);
                result = result.Replace(word, value);
            }
            return result;
        }

        private string GetVariableFromDynamicSource(string key, object source)
        {
            foreach (var field in source.GetType().GetFields())
            {
                var attribute = field.GetCustomAttribute<LocalizationDynamicVariable>();
                if (attribute == null) continue;
                if (attribute.key == key) return field.GetValue(source).ToString();
            }

            throw new Exception($"dynamic variable \"{key}\" not found in {source.GetType()}");
        }

        public string GetSavedData() => language == SystemLanguage.English ? "en" : "ru";

        public void LoadData(string data) => Language = data == "en" ? SystemLanguage.English : SystemLanguage.Russian;

        public string Id => "localization";

        public static void LocalizeUI(VisualElement root, object source, bool subscribeForLanguageChanging = true)
        {
            if (root is ILocalizable localizable)
            {
                localizable.SetValue(GetValue(localizable.Key, source));
                if (subscribeForLanguageChanging)
                    LanguageChanged += () => localizable.SetValue(GetValue(localizable.Key, source));
            }

            foreach (var child in root.Children())
                LocalizeUI(child, source);
        }
    }
}