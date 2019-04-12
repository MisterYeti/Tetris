using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{


    private Board m_board;

    private Spawner m_spawner;
    private Shape m_activeShape;
    private SoundManager m_soundManager;
    private ScoreManager m_scoreManager;
    private Ghost m_ghost;
    private Holder m_holder;
    private AddPlayer m_addPlayer;
    private GetPlayerInfos m_getPlayerInfo;
    private UpdatePlayer m_updatePlayer;

    private bool m_gameOver = false;

    public float m_dropInterval = 0.9f;
    private float m_dropIntervalModded;

    private float m_timeToDrop;

    private float m_timeToNextKeyLeftRight;

    private float m_timeToNextKey;

    [Range(0.2f, 1f)]
    public float m_keyRepeatRateLeftRight = 0.15f;

    private float m_timeToNextKeyDown;

    [Range(0.02f, 0.5f)]
    public float m_keyRepeatRateDown = 0.5f;

    private float m_timeToNextKeyRotate;

    [Range(0.2f, 1f)]
    public float m_keyRepeatRateRotate = 0.25f;

    public GameObject m_gameOverPanel;

    public IconToggle m_rotIconToggle;

    private bool m_clockwise = true;

    public bool m_isPaused = false;

    public GameObject m_pausePanel;

    public ParticlePlayer m_gameOverFx;

    enum Direction
    {
        none,
        left,
        right,
        up,
        down
    };

    private Direction m_dragDirection = Direction.none;
    private Direction m_swipeDirection = Direction.none;


    private float m_timeToNextDrag;
    private float m_timeToNextSwipe;

    [Range(0.05f, 1f)]
    public float m_minTimeToDrag = 0.15f;

    [Range(0.05f, 1f)]
    public float m_minTimeToSwipe = 0.3f;

    private bool m_didTap = false;

    void OnEnable()
    {
        TouchController.DragEvent += DragHandler;
        TouchController.SwipeEvent += SwipeHandler;
        TouchController.TapEvent += TapHandler;
    }

    void OnDisable()
    {
        TouchController.DragEvent -= DragHandler;
        TouchController.SwipeEvent -= SwipeHandler;
        TouchController.TapEvent -= TapHandler;
    }

    void Start()
    {
        m_board = FindObjectOfType<Board>();
        m_spawner = FindObjectOfType<Spawner>();
        m_soundManager = FindObjectOfType<SoundManager>();
        m_scoreManager = FindObjectOfType<ScoreManager>();
        m_ghost = FindObjectOfType<Ghost>();
        m_holder = FindObjectOfType<Holder>();
        m_addPlayer = FindObjectOfType<AddPlayer>();
        m_getPlayerInfo = FindObjectOfType<GetPlayerInfos>();
        m_updatePlayer = FindObjectOfType<UpdatePlayer>();

        m_timeToNextKey = Time.time;
        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
        m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
        m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

        if (!m_spawner || !m_board || !m_soundManager || !m_scoreManager)
        {
            Debug.LogWarning("Warning! m_board or m_spawner or m_soundManager or m_scoreManager aren't defined.");
        }
        else
        {
            if (m_activeShape == null)
            {
                m_activeShape = m_spawner.SpawnShape();
            }

            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
        }

        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(false);
        }

        if (m_pausePanel)
        {
            m_pausePanel.SetActive(false);
        }


        m_dropIntervalModded = m_dropInterval;
        m_soundManager.m_fxEnabled = SaveManager.Instance.IsFxOn();
        m_soundManager.m_musicEnabled = SaveManager.Instance.IsMusicOn();

        m_soundManager.m_fxIconToggle.ToggleIcon(m_soundManager.m_fxEnabled);
        m_soundManager.m_musicIconToggle.ToggleIcon(m_soundManager.m_musicEnabled);

        m_soundManager.UpdateMusic();
    }


    void Update()
    {

        if (!m_spawner || !m_board || !m_activeShape || m_gameOver || !m_soundManager || !m_scoreManager)
        {
            return;
        }

        PlayerInput();


    }

    void LateUpdate()
    {
        if (m_ghost)
        {
            m_ghost.DrawGhost(m_activeShape, m_board);
        }
    }

    void PlayerInput()
    {


        if ((Input.GetButton("MoveRight") && Time.time > m_timeToNextKeyLeftRight) || Input.GetButtonDown("MoveRight"))
            MoveRight();
        else if ((Input.GetButton("MoveLeft") && Time.time > m_timeToNextKeyLeftRight) || Input.GetButtonDown("MoveLeft"))
            MoveLeft();
        else if ((Input.GetButtonDown("Rotate") && Time.time > m_timeToNextKeyRotate))
            Rotate();
        else if (((Input.GetButton("MoveDown") && Time.time > m_timeToNextKeyDown)) || Time.time > m_timeToDrop)
            MoveDown();




        else if ((m_swipeDirection == Direction.right && Time.time > m_timeToNextSwipe) ||
                 (m_dragDirection == Direction.right) && Time.time > m_timeToNextDrag)
        {
            MoveRight();

            m_timeToNextDrag = Time.time + m_minTimeToDrag;
            m_timeToNextSwipe = Time.time + m_minTimeToSwipe;
        }

        else if ((m_swipeDirection == Direction.left && Time.time > m_timeToNextSwipe) ||
                 (m_dragDirection == Direction.left) && Time.time > m_timeToNextDrag)
        {
            MoveLeft();

            m_timeToNextDrag = Time.time + m_minTimeToDrag;
            m_timeToNextSwipe = Time.time + m_minTimeToSwipe;
        }

        else if ((m_swipeDirection == Direction.up && Time.time > m_timeToNextSwipe) || (m_didTap))
        {
            if (!m_isPaused)
            {
                Rotate();
                m_timeToNextSwipe = Time.time + m_minTimeToSwipe;
                m_didTap = false;
            }
        }

        else if (m_dragDirection == Direction.down && Time.time > m_timeToNextDrag)
        {
            MoveDown();
            m_timeToNextDrag = Time.time + m_minTimeToDrag;

        }





        else if (Input.GetButtonDown("ToggleRot"))
            ToggleRotDirection();
        else if (Input.GetButtonDown("Pause"))
            TogglePause();
        else if (Input.GetButtonDown("Hold"))
            Hold();

        m_dragDirection = Direction.none;
        m_swipeDirection = Direction.none;
        m_didTap = false;


    }

    private void MoveDown()
    {
        m_timeToDrop = Time.time + m_dropIntervalModded;
        m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;

        m_activeShape.MoveDown();
        if (!m_board.IsValidPosition(m_activeShape))
        {
            if (m_board.IsOverLimit((m_activeShape)))
            {
                GameOver();
            }
            else
                LandShape();
        }
    }

    private void Rotate()
    {
        m_activeShape.RotateClockwise(m_clockwise);
        m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

        if (!m_board.IsValidPosition(m_activeShape))
        {
            m_activeShape.RotateClockwise(!m_clockwise);
            PlaySound(m_soundManager.m_errorSound, 0.5f);
        }
        else
        {
            PlaySound(m_soundManager.m_moveSound, 0.5f);
        }
    }

    private void MoveLeft()
    {
        m_activeShape.MoveLeft();
        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

        if (!m_board.IsValidPosition(m_activeShape))
        {
            m_activeShape.MoveRight();
            PlaySound(m_soundManager.m_errorSound, 0.5f);
        }
        else
        {
            PlaySound(m_soundManager.m_moveSound, 0.5f);
        }
    }

    private void MoveRight()
    {
        m_activeShape.MoveRight();
        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

        if (!m_board.IsValidPosition(m_activeShape))
        {
            m_activeShape.MoveLeft();
            PlaySound(m_soundManager.m_errorSound, 0.5f);
        }
        else
        {
            PlaySound(m_soundManager.m_moveSound, 0.5f);
        }
    }

    private void PlaySound(AudioClip clip, float volMultiplier)
    {
        if (clip && m_soundManager.m_fxEnabled)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position,
                Mathf.Clamp(m_soundManager.m_fxVolume * volMultiplier, 0.05f, 1f));
        }
    }

    public void ToggleRotDirection()
    {
        m_clockwise = !m_clockwise;
        if (m_rotIconToggle)
        {
            m_rotIconToggle.ToggleIcon(m_clockwise);
        }
    }

    public void TogglePause()
    {
        if (m_gameOver)
            return;

        m_isPaused = !m_isPaused;
        if (m_pausePanel)
        {
            m_pausePanel.SetActive(m_isPaused);

            if (m_soundManager)
                m_soundManager.m_musicSource.volume = (m_isPaused) ? m_soundManager.m_musicVolume * 0.25f : m_soundManager.m_musicVolume;
        }

        Time.timeScale = (m_isPaused) ? 0 : 1;
    }

    private void GameOver()
    {
        m_activeShape.MoveUp();
        m_gameOver = true;


        PlaySound(m_soundManager.m_gameOverSound, 5f);

        PlaySound(m_soundManager.m_gameOverVocalClip, 5f);

        StartCoroutine(GameOverRoutine());

        Save();


    }

    IEnumerator GameOverRoutine()
    {
        if (m_gameOverFx)
            m_gameOverFx.Play();

        yield return new WaitForSeconds(0.3f);

        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(true);
        }
    }

    private void Save()
    {
        SaveManager.Instance.SaveScore(m_scoreManager.m_score, m_scoreManager.m_level);


        StartCoroutine(SaveToDatabase());

    }

    IEnumerator SaveToDatabase()
    {
        yield return m_getPlayerInfo.Get(SaveManager.Instance.currentName);
        if (m_getPlayerInfo.Score != "")
        {
            if (int.Parse(m_getPlayerInfo.Score) < m_scoreManager.m_score)
            {
                m_updatePlayer.UpdatePlayerScore(SaveManager.Instance.currentName, m_scoreManager.m_score, m_scoreManager.m_level);
            }

        }
        else
        {
            m_addPlayer.AddUser(SaveManager.Instance.currentName, m_scoreManager.m_score, m_scoreManager.m_level);
        }
    }

    private void LandShape()
    {
        m_timeToNextKey = Time.time;
        m_timeToNextKeyLeftRight = Time.time;
        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyRotate = Time.time;

        if (m_activeShape)
        {
            m_activeShape.MoveUp();
            m_board.StoreShapeInGrid(m_activeShape);

            m_activeShape.LandShapeFX();

            if (m_ghost)
            {
                m_ghost.Reset();
            }

            if (m_holder)
                m_holder.m_canRelease = true;

            m_activeShape = m_spawner.SpawnShape();

            m_board.StartCoroutine("ClearAllRows");


            PlaySound(m_soundManager.m_dropSound, 0.75f);


            if (m_board.m_completedRows > 0)
            {
                m_scoreManager.ScoreLines(m_board.m_completedRows);

                if (m_scoreManager.m_didLevelUp)
                {
                    PlaySound(m_soundManager.m_levelUpVocalClip, 1.0f);
                    m_dropIntervalModded = Mathf.Clamp(m_dropInterval - (((float)m_scoreManager.m_level - 1) * 0.05f),
                        0.05f, 1f);
                }
                else
                {
                    if (m_board.m_completedRows > 1)
                    {
                        PlaySound(m_soundManager.GetRandomClip(m_soundManager.m_vocalClips), 1f);
                    }
                }

                PlaySound(m_soundManager.m_clearRowSound, 0.75f);
            }
        }
    }

    public void Hold()
    {
        if (!m_holder || m_isPaused)
            return;

        if (!m_holder.m_heldShape)
        {
            m_holder.Catch(m_activeShape);
            m_activeShape = m_spawner.SpawnShape();
            PlaySound(m_soundManager.m_holdSound, 0.5f);
        }
        else if (m_holder.m_canRelease)
        {
            Shape shape = m_activeShape;
            m_activeShape = m_holder.Release();
            m_activeShape.transform.position = m_spawner.transform.position;
            m_holder.Catch(shape);
            PlaySound(m_soundManager.m_holdSound, 0.5f);

        }
        else
        {
            PlaySound(m_soundManager.m_errorSound, 0.5f);
        }



        if (m_ghost)
            m_ghost.Reset();
    }

    void DragHandler(Vector2 drapMovement)
    {
        m_dragDirection = GetDirection(drapMovement);
    }

    void SwipeHandler(Vector2 swipeMovement)
    {
        m_swipeDirection = GetDirection(swipeMovement);
    }

    void TapHandler(Vector2 swipeMovement)
    {
        m_didTap = true;
    }

    Direction GetDirection(Vector2 swipeMovement)
    {
        Direction swipeDir = Direction.none;

        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            swipeDir = (swipeMovement.x >= 0) ? Direction.right : Direction.left;
        }
        else
        {
            swipeDir = (swipeMovement.y >= 0) ? Direction.up : Direction.down;
        }

        return swipeDir;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
