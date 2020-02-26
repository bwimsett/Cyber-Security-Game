using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace backend.serialization {
    public class GameSerializer {

        private const string saveGameExtension = ".cyber";        
        
        public void SaveGame(SaveGame saveGame) {
            saveGame.SetSaveTime(DateTime.Now);
            
            BinaryFormatter bf = new BinaryFormatter();
            String path = Application.persistentDataPath + "/" + DateTime.Now.Day+""+DateTime.Now.Month+""+DateTime.Now.Year+""+ saveGameExtension;
            FileStream file = File.Create(path);
            bf.Serialize(file, saveGame);
            file.Close();
            
            Debug.Log("Game saved to: "+path);
        }

        public SaveGame GetMostRecentSave() {
            SaveGame[] saves = GetSaves();

            if (saves.Length == 0) {
                return null;
            }

            SaveGame mostRecent = saves[0];
            
            foreach (SaveGame s in saves) {
                if (s.GetSaveTime().CompareTo(mostRecent.GetSaveTime()) > 0) {
                    mostRecent = s;
                }
            }

            return mostRecent;
        }

        public SaveGame[] GetSaves() {
            String[] files = Directory.GetFiles(Application.persistentDataPath, "*"+saveGameExtension);
            List<SaveGame> saves = new List<SaveGame>();
            
            Debug.Log("Save files found: "+files.Length);
            BinaryFormatter bf = new BinaryFormatter();
            
            foreach (String s in files) {
                FileStream fs = new FileStream(s, FileMode.Open);
                SaveGame saveGame = (SaveGame)bf.Deserialize(fs);
                saves.Add(saveGame);
            }
            
            return saves.ToArray();
        }            
    }
}