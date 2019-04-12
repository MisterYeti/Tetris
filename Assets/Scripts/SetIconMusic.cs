using UnityEngine;
using UnityEngine.UI;

public class SetIconMusic : MonoBehaviour
{


    void Start()
    {

        if (SaveManager.Instance.IsMusicOn())
            gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<IconToggle>().m_iconTrue;
        else
        {
            gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<IconToggle>().m_iconFalse;

        }
    }


}
