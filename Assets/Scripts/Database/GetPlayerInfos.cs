using System.Collections;
using UnityEngine;

public class GetPlayerInfos : MonoBehaviour
{

    private string _url = "http://thomasbiffaud.com/Tetris/playerSelect.php";

    public string PlayerInfos;
    public string Score, Level;


    void Start()
    {



    }


    public IEnumerator Get(string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);

        WWW player = new WWW(_url, form);
        yield return player;
        PlayerInfos = player.text;
        if (PlayerInfos.Length > 0)
        {
            GetValues(PlayerInfos);
        }

    }


    private void GetValues(string infos)
    {
        int i = 0;

        while (infos[i].ToString() != "|")
        {
            Score += infos[i];
            i++;
        }

        i += 1;
        while (i < infos.Length)
        {
            Level += infos[i];
            i++;
        }

    }



}
