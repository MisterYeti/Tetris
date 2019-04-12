using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    void Start()
    {
        GameObject.FindGameObjectWithTag("input").GetComponent<InputField>().text = SaveManager.Instance.currentName;
    }


}
