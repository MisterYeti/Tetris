using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LocalScoresUpdate : MonoBehaviour
{

    private List<Text> _names;
    private List<Text> _scores;
    private List<Text> _levels;
    private List<Player> _players;
    private int _nbPlayers;


    // Use this for initialization
    void Start()
    {
        _names = new List<Text>();
        _scores = new List<Text>();
        _levels = new List<Text>();
        _players = new List<Player>();

        foreach (Transform child in transform)
        {
            foreach (Transform child2 in child)
            {
                if (child2.name == "Name")
                    _names.Add(child2.gameObject.GetComponent<Text>());
                else if (child2.name == "Score")
                    _scores.Add(child2.gameObject.GetComponent<Text>());
                else if (child2.name == "Level")
                    _levels.Add(child2.gameObject.GetComponent<Text>());
            }
        }

        _players = SaveManager.Instance.GetPlayers();

        _players = _players.OrderByDescending(p => p.MaxScore).ToList();

        _nbPlayers = _players.Count >= 10 ? 10 : _players.Count;

        for (int i = 0; i < _nbPlayers; i++)
        {
            _names[i].text = _players[i].Name;
            _scores[i].text = _players[i].MaxScore.ToString();
            _levels[i].text = "Lvl " + _players[i].MaxLevel.ToString();
        }

    }

}
