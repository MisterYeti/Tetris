using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;

namespace LevelManagement.Data
{

    public class JsonSaver
    {
        private static readonly string _fileName = "saveData1.sav";

        public static string GetSaveFilename()
        {
            return Application.persistentDataPath + "/" + _fileName;
        }

        public void Save(SaveData data)
        {

            string json = JsonUtility.ToJson(data);

            json = JsonUtility.ToJson(data);


            string saveFilename = GetSaveFilename();

            FileStream filestream = new FileStream(saveFilename, FileMode.Create);

            using (StreamWriter writer = new StreamWriter(filestream))
            {
                writer.Write(json);
            }
        }

        public bool Load(SaveData data)
        {
            string loadFilename = GetSaveFilename();
            if (File.Exists(loadFilename))
            {
                using (StreamReader reader = new StreamReader(loadFilename))
                {
                    string json = reader.ReadToEnd();

                        JsonUtility.FromJsonOverwrite(json, data);

                    
                }
                return true;
            }
            return false;
        }

        

        public void Delete()
        {
            File.Delete(GetSaveFilename());
        }

       

    } 
}
