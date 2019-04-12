using UnityEngine;

public class AddPlayer : MonoBehaviour
{

    private string _url = "http://thomasbiffaud.com/Tetris/playerInsert.php";





    public void AddUser(System.String userName, int score, int level)
    {
        WWWForm form = new WWWForm();
        form.AddField("addName", userName);
        form.AddField("addScore", score);
        form.AddField("addLevel", level);

        WWW www = new WWW(_url, form);

    }

}
