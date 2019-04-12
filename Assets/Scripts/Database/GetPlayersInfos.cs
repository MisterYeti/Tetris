using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayersInfos : MonoBehaviour
{

    private string _url = "http://thomasbiffaud.com/Tetris/playersSelect.php";


    public string PlayerInfos;

    private List<String> _names = new List<string>();
    private List<String> _scores = new List<string>();
    private List<String> _levels = new List<string>();


    public List<Player> players = new List<Player>();


    public void Reset()
    {
        _names = new List<string>();
        _scores = new List<string>();
        _levels = new List<string>();

        players = new List<Player>();
    }

    public IEnumerator Get()
    {


        WWW player = new WWW(_url);
        yield return player;
        PlayerInfos = player.text;
        GetValues(PlayerInfos);


    }

    public void GetValues(string infos)
    {
        int i = 0;
        int indexPlayer = 0;

        _names.Add(null);
        _scores.Add(null);
        _levels.Add(null);

        while (i < infos.Length)
        {

            while (infos[i].ToString() != "|")
            {
                _names[indexPlayer] += infos[i];
                i++;
            }

            i++;

            while (infos[i].ToString() != "|")
            {
                _scores[indexPlayer] += infos[i];
                i++;
            }

            i++;

            if (i < infos.Length)
            {
                while (infos[i].ToString() != "|")
                {
                    _levels[indexPlayer] += infos[i];
                    i++;
                }

                i++;
            }

            indexPlayer++;
            _names.Add(null);
            _scores.Add(null);
            _levels.Add(null);

        }

        _names.RemoveAt(_names.Count - 1);
        _scores.RemoveAt(_scores.Count - 1);
        _levels.RemoveAt(_levels.Count - 1);

        for (int x = 0; x < _names.Count; x++)
        {
            players.Add(new Player() { Name = _names[x], MaxScore = int.Parse(_scores[x]), MaxLevel = int.Parse(_levels[x]) });
        }
    }
}
