using UnityEngine;

public class MusicMenu : MonoBehaviour
{

    public AudioSource m_music;


    void Start()
    {
        if (SaveManager.Instance.IsMusicOn())
        {
            m_music.Stop();
            m_music.volume = 0.3f;
            m_music.loop = true;
            m_music.Play();
        }
    }

    public void SetMusic()
    {
        if (!m_music.isPlaying)
        {
            m_music.volume = 0.3f;
            m_music.loop = true;
            m_music.Play();
        }

        else
        {
            m_music.Stop();
        }
    }


}
