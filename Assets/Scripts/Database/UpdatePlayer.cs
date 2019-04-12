using UnityEngine;

public class UpdatePlayer : MonoBehaviour
{


    private string _url = "http://thomasbiffaud.com/Tetris/playerUpdate.php";


    public void UpdatePlayerScore(System.String userName, int score, int level)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", userName);
        form.AddField("setScore", score);
        form.AddField("setLevel", level);

        WWW www = new WWW(_url, form);

    }
}
