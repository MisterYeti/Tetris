using UnityEngine;
using UnityEngine.SceneManagement;

public class UtilityManager : MonoBehaviour
{



    public void LoadLevel(int id)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(id);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleMusic(GameObject go)
    {
        SaveManager.Instance.ToggleMusic(go);

    }

    public void ToggleFX(GameObject go)
    {
        SaveManager.Instance.ToggleFX(go);
    }

    public void Reset()
    {
        SaveManager.Instance.Reset();
    }

    public void Save()
    {
        SaveManager.Instance.Save();
    }

    public void OnPlayerNameValueChanged(string name)
    {
        SaveManager.Instance.OnPlayerNameValueChanged(name);
    }

    public void OnPlayerNameEndEdit()
    {
        SaveManager.Instance.OnPlayerNameEndEdit();
    }

}
