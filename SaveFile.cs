using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace VincentBrodin.SaveSystem
{
    public class SaveFile
    {
        public string FileName{ get; private set; }
        public string Path{ get; private set; }
        public Dictionary<string, Data> Cache{ get; private set; } = new();
        public readonly UnityEvent OnReady = new();
        
        public class Data
        {
            public string Key{ get; set; }
            public object Value{ get; set; }
            public DataTypes Type{ get; set; }
            public readonly UnityEvent OnChanged = new();
        }

        public enum DataTypes
        {
            Int,
            Float,
            String,
            Bool
        }

        /// <summary>
        /// Creates a new save file.
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="directories">The directories that the file lays in form the persistent data path</param>
        public SaveFile(string fileName, IEnumerable directories){
            FileName = fileName;
            string directoryString = "";
            foreach (string dir in directories){
                directoryString += dir + "/";

                if (!Directory.Exists($"{Application.persistentDataPath}/{directoryString}")){
                    Directory.CreateDirectory($"{Application.persistentDataPath}/{directoryString}");
                }
            }

            Path = $"{Application.persistentDataPath}/{directoryString}/{fileName}";

            if (!File.Exists(Path)){
                File.Create(Path).Close();
            }

            LoadFromFile();
        }

        /// <summary>
        /// Loads the file and stores it in the cache.
        /// </summary>
        public async void LoadFromFile(){
            string[] lines = await File.ReadAllLinesAsync(Path);

            foreach (string line in lines){
                string[] data = line.Split(':');
                string key = data[0];
                string value = data[1];
                DataTypes type = (DataTypes)Enum.Parse(typeof(DataTypes), data[2]);

                Cache.Add(key, new Data{
                    Key = key,
                    Value = value,
                    Type = type
                });
            }

            OnReady.Invoke();
        }

        /// <summary>
        /// Writes the cache to the file.
        /// </summary>
        public void WriteToFile(){
            List<string> lines = new List<string>();

            foreach (KeyValuePair<string, Data> data in Cache){
                lines.Add($"{data.Key}:{data.Value.Value}:{data.Value.Type}");
            }

            File.WriteAllLines(Path, lines);
        }

        /// <summary>
        /// Sets the value of a key in the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Data Set(string key, object value, DataTypes type){
            if (Cache.TryGetValue(key, out Data data)){
                data.Value = value;
                data.Type = type;
                data.OnChanged.Invoke();
                return data;
            }

            return Cache[key] = new Data{
                Key = key,
                Value = value,
                Type = type,
            };
        }

        public Data Get(string key){
            return Cache.GetValueOrDefault(key);
        }

#if UNITY_EDITOR
        [MenuItem("Unity Tools/Save System/Open Persistent Data Path")]
        private static void OpenProjectFolder(){
            Application.OpenURL(Application.persistentDataPath);
        }
#endif
        public Data this[string key]{
            get => Cache.GetValueOrDefault(key);
            set => Cache[key] = (Data)value;
        }
    }
}
