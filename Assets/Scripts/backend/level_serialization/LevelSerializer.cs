using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DefaultNamespace;
using UnityEngine;

namespace backend.level_serialization {
    public class LevelSerializer {
        
        private const string levelFileExtension = ".cyblvl";

        public void SaveCurrentLevelAsTemplate(string levelName) {
            LevelSave save = new LevelSave(GameManager.currentLevel);
            
            BinaryFormatter bf = new BinaryFormatter();
            String path = Application.persistentDataPath + "/" + levelName + levelFileExtension;
            FileStream file = File.Create(path);
            bf.Serialize(file, save);
            file.Close();
            
            Debug.Log("Level saved to: "+path);
        }

        public LevelSave GetLevelSave(String name) {
            LevelSave levelSave = null;
            
            BinaryFormatter bf = new BinaryFormatter();
            String path = Application.persistentDataPath + "/" + name + levelFileExtension;
            FileStream file = null;
            
            try {
                file = File.Open(path, FileMode.Open);
                levelSave = (LevelSave)bf.Deserialize(file);
            }
            catch {
                Debug.Log("File path not a valid level: "+name);
            }

            file?.Close();

            return levelSave;
        }
    }
}