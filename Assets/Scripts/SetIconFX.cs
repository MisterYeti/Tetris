using UnityEngine;
using UnityEngine.UI;

public class SetIconFX : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (SaveManager.Instance.IsFxOn())
            gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<IconToggle>().m_iconTrue;
        else
        {
            gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<IconToggle>().m_iconFalse;

        }
    }

}
