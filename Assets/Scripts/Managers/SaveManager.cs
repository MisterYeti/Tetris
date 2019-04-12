using LevelManagement.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{


    private static SaveManager _instance;


    private DataManager _dataManager;
    private InputField _inputField;
    public string currentName;

    public static SaveManager Instance
    {
        get { return _instance; }

    }

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
        {
            _instance = this;
            _dataManager = FindObjectOfType<DataManager>();
            _inputField = GameObject.FindGameObjectWithTag("input").GetComponent<InputField>();

        }

        Object.DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        Load();
        currentName = _dataManager.PlayerName;
    }

    public void OnPlayerNameValueChanged(string name)
    {
        if (_dataManager != null)
        {
            _dataManager.PlayerName = name;
        }

        currentName = name;
    }

    public void OnPlayerNameEndEdit()
    {
        if (_dataManager != null)
        {
            _dataManager.Save();
        }
    }

    public void OnToggleMusic(bool isOn)
    {
        _dataManager.musicOn = isOn;
        _dataManager.Save();
    }

    public void OnToggleFX(bool isOn)
    {
        _dataManager.fxOn = isOn;
        _dataManager.Save();
    }

    private void Load()
    {
        if (_dataManager == null || _inputField == null)
            return;

        _dataManager.Load();
        _inputField.text = _dataManager.PlayerName;

    }

    public void Save()
    {
        _dataManager.Save();
    }



    public void SaveScore(int score, int lvl)
    {
        if (_dataManager == null)
            return;

        //int sameName = 0;

        //foreach (var player in _dataManager.Players)
        //{
        //    if (player.Name == currentName)
        //    {
        //        sameName++;
        //        if (player.MaxScore < Scores)

        //        {
        //            player.MaxScore = Scores;
        //            player.MaxLevel = lvl;
        //            _dataManager.Save();
        //        }

        //    }
        //}

        _dataManager.Players.Add(new Player() { Name = currentName, MaxScore = score, MaxLevel = lvl });
        _dataManager.Save();

    }

    public bool IsMusicOn()
    {
        return _dataManager.musicOn;
    }

    public bool IsFxOn()
    {
        return _dataManager.fxOn;
    }

    public void Reset()
    {
        _dataManager.Players.Clear();
        _dataManager.Players = new List<Player>();
        _dataManager.Save();
    }

    public void ToggleMusic(GameObject musicGO)
    {
        Sprite sprite = musicGO.GetComponent<Image>().sprite;
        if (musicGO.GetComponent<IconToggle>().m_iconTrue == sprite)
        {
            musicGO.GetComponent<Image>().sprite = musicGO.GetComponent<IconToggle>().m_iconFalse;
            _dataManager.musicOn = false;
        }
        else
        {
            musicGO.GetComponent<Image>().sprite = musicGO.GetComponent<IconToggle>().m_iconTrue;
            _dataManager.musicOn = true;
        }

    }

    public void ToggleFX(GameObject FXgo)
    {
        Sprite sprite = FXgo.GetComponent<Image>().sprite;
        if (FXgo.GetComponent<IconToggle>().m_iconTrue == sprite)
        {
            FXgo.GetComponent<Image>().sprite = FXgo.GetComponent<IconToggle>().m_iconFalse;
            _dataManager.fxOn = false;
        }
        else
        {
            FXgo.GetComponent<Image>().sprite = FXgo.GetComponent<IconToggle>().m_iconTrue;
            _dataManager.fxOn = true;
        }
    }


    public int GetNumberOfPlayers()
    {
        return _dataManager.Players.Count;
    }

    public List<Player> GetPlayers()
    {
        return _dataManager.Players;
    }
}
