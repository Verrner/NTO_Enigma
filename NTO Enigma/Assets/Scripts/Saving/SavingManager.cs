using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace NTO
{
    public sealed class SavingManager : MonoBehaviour, ISavingReceiver
    {
        [Serializable]
        public sealed class GeneralData
        {
            public string name = "New Save";
            [FormerlySerializedAs("creationDate")] public string creatingDate;
            public string updatingDate;
        }

        [Serializable]
        private sealed class Category
        {
            public string id;
            public string data;
        }
        
        [Serializable]
        private sealed class CategoryData{}

        [Serializable]
        private sealed class SaveObject
        {
            public Category[] categories;
        }
        
        [SerializeField] public GeneralData generalData;
        [SerializeField] private GameObject[] sources;
        
        public event Action SavingStarted;
        public event Action<string, bool> SavingEnded;
        
        public event Action LoadingStarted;
        public event Action<string, bool> LoadingEnded;
        
        private List<ISavingReceiver> _sources;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            CheckForSavesFolder();
            ConvertSources();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                Save();
            else if (Input.GetKeyDown(KeyCode.L))
                Load(generalData.name);
        }

        private void ConvertSources() => _sources = sources.Select(s => s.GetComponent<ISavingReceiver>()).ToList();

        private string[] GetAllSavesPaths() => Directory.GetFiles($"{Application.dataPath}/Saves")
            .Where(path => Path.GetExtension(path) == ".json").ToArray();

        public (string, GeneralData)[] GetAllSavesData()
        {
            var saves = GetAllSavesPaths();
            var result = new (string, GeneralData)[saves.Length];

            for (var i = 0; i < saves.Length; i++)
            {
                var path = saves[i];
                var json = File.ReadAllText(path);
                var saveObject = JsonUtility.FromJson<SaveObject>(json);
                var generalCategory = saveObject.categories[0];
                var data = JsonUtility.FromJson<GeneralData>(generalCategory.data);
                result[i] = (path, data);
            }
            
            return result;   
        }
        
        public async Task Save()
        {
            Debug.Log("Saving started");
            Debug.Log($"{Application.dataPath}/Saves/{generalData.name}.json");
            
            SavingStarted?.Invoke();
            
            var message = "Saved successfully";
            var success = true;

            await Task.Run(() =>
            {
                try
                {
                    SetDates();
                    var data = GetJson();
                    CheckForFile(generalData.name);
                    File.WriteAllText($"{Application.dataPath}/Saves/{generalData.name}.json", data);
                }
                catch (Exception e)
                {
                    message = e.Message;
                    success = false;
                }
            });
            
            SavingEnded?.Invoke(message, success);
        }

        public async Task Load(string fileName)
        {
            Debug.Log($"Loading {fileName}");
            
            LoadingStarted?.Invoke();
            
            var message = "Loaded successfully";
            var success = true;

            await Task.Run(() =>
            {
                try
                {
                    var path = $"{Application.dataPath}/Saves/{fileName}.json";
                    
                    if (!File.Exists(path))
                        throw new Exception("file not found");
                    
                    var dataJson = File.ReadAllText(path);
                    var data = JsonUtility.FromJson<SaveObject>(dataJson);
                    SetData(data);
                }
                catch (Exception e)
                {
                    message = e.Message;
                    success = false;
                }
            });
            
            LoadingEnded?.Invoke(message, success);
        }

        private string GetJson()
        {
            var dataArray = _sources.Select(s =>
                new Category{data = s.GetSavedData(), id = s.Id}).ToArray();
            
            var data = new SaveObject { categories = dataArray };
            var res = JsonUtility.ToJson(data);
            Debug.Log(res);
            return res;
        }

        private void SetData(SaveObject data)
        {
            var dictionary = _sources.ToDictionary(source => source.Id);

            foreach (var category in data.categories)
            {
                var source = dictionary[category.id];
                source.LoadData(category.data);
            }
        }

        private void SetDates()
        {
            var timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (generalData.creatingDate == "")
                generalData.creatingDate = timeNow;
            generalData.updatingDate = timeNow;
        }

        private void CheckForFile(string fileName)
        {
            var path = $"{Application.dataPath}/Saves/{fileName}.json";
            if (!File.Exists(path))
                File.Create(path).Close();
        }

        private void CheckForSavesFolder()
        {
            var path = $"{Application.dataPath}/Saves";
            if (Directory.Exists(path))
                return;
            Directory.CreateDirectory(path);
        }
        
        public string GetSavedData() => JsonUtility.ToJson(generalData);
        public void LoadData(string data) => generalData = JsonUtility.FromJson<GeneralData>(data);
        public string Id => "general-data";
    }
}