using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement.Data
{
    public class DataManager : MonoBehaviour
    {
        private SaveData _saveData;
        private JsonSaver _jsonSaver;
        private static DataManager _instance;


        public string PlayerName
        {
            get { return _saveData.playerName; }
            set { _saveData.playerName = value; }
        }

        public List<Player> Players
        {
            get { return _saveData.players; }
            set { _saveData.players = value; }
        }

        public bool musicOn
        {
            get { return _saveData.musicOn; }
            set { _saveData.musicOn = value; }
        }

        public bool fxOn
        {
            get { return _saveData.fxOn; }
            set { _saveData.fxOn = value; }
        }

        public static DataManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
            {
                _instance = this;
                _saveData = new SaveData();
                _jsonSaver = new JsonSaver();

            }
            Object.DontDestroyOnLoad(gameObject);
            
        }

        public void Save()
        {
            _jsonSaver.Save(_saveData);
        }

        public void Load()
        {
            _jsonSaver.Load(_saveData);
        }


    } 
}
