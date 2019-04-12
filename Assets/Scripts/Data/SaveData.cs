using System;
using System.Collections.Generic;

namespace LevelManagement.Data
{
    [Serializable]
    public class SaveData
    {
        public string playerName;
        private readonly string defaultPlayerName = "Player";

        public bool musicOn, fxOn;

        public List<Player> players;


        public SaveData()
        {
            playerName = defaultPlayerName;

            players = new List<Player>()
            {


            };

            musicOn = true;
            fxOn = true;

        }

    }
}
