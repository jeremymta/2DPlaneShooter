using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //private EnemyFormationManager enemyFormationManager;
    private float enemyHeight;

    public GameObject enemyPrefab;
    public List<EnemyController> enemies = new();
    private Vector3 offScreenPosition = new(-10f, 6f, 0);

    public List<GameObject> img_lives;
    public int score = 0;
    public Text txtScore;
    public Text txtHighScore;
    private int highScore; // Save load qua Playerprefs
    private string highScorePlayer; //Save load qua Playerprefs
    public Text txtLive;
    public GameObject pnlEndGame;
    public Text txtEndPoint;
    public Button btnRestart;

    public Sprite btnIdle;
    public Sprite btnHover;
    public Sprite btnClick;

    public bool isEndGame;
    private bool isStartFirstTime;
    private bool isPaused = false;
    private bool isHighScoreSaved = false;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private InputField nameInputField;
    [SerializeField] private Button saveHighScoreButton;
    [SerializeField] private Button resetHighScoreButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        AudioManager.Instance.PlayBackgroundMusic();

        Time.timeScale = 1.0f; //0
        isEndGame = false;
        pnlEndGame.SetActive(false);
        isStartFirstTime = true;

        //Tinh toan enemyHeight
        GameObject tempEnemy = Instantiate(enemyPrefab, offScreenPosition, Quaternion.identity);
        SpriteRenderer spriteRenderer = tempEnemy.GetComponent<SpriteRenderer>();
        enemyHeight = spriteRenderer.bounds.size.y;
        Destroy(tempEnemy);

        //private EnemyFormationManager enemyFormationManager;
        //enemyFormationManager = gameObject.AddComponent<EnemyFormationManager>();
        //enemyFormationManager = EnemyFormationManager.Instance;
        //enemyFormationManager.Initialize(enemyHeight);
        EnemyFormationManager.Instance.Initialize(enemyHeight);

        SpawnEnemies();
        StartCoroutine(EnemyFormationManager.Instance.EnemyFormationSequence(enemies));

        highScore = PlayerPrefs.GetInt("HighScore", 0); // Tai du lieu
        highScorePlayer = PlayerPrefs.GetString("HighScorePlayer", "None");
        txtHighScore.text = $"High Score:\n {highScorePlayer} - {highScore}";

        saveHighScoreButton.onClick.AddListener(SaveHighScore);
        //resetHighScoreButton.onClick.AddListener(ResetHighScore);
        nameInputField.gameObject.SetActive(false);
        saveHighScoreButton.gameObject.SetActive(false);
        resetHighScoreButton.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (isEndGame)
        {
            if (Input.GetMouseButtonDown(0) && isStartFirstTime)
            {
                StartGane();
            }
        }
        else if (!isPaused)
        {
            if(Input.GetMouseButtonDown(0) && Time.timeScale == 0)
            {
                //Time.timeScale = 1;
                PauseGamebutton();
            }
        }
        
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < 16; i++)
        {
            GameObject enemyObj = Instantiate(enemyPrefab, offScreenPosition, Quaternion.identity);
            EnemyController enemy = enemyObj.GetComponent<EnemyController>();
            enemies.Add(enemy);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        txtScore.text = "Score\n" + score.ToString();
    }

    public void UpdateLives(int lives)
    {
        //Debug.Log("Lives: " + lives);
        txtLive.text = "Lives\n" + lives.ToString();
    }
    public void LostLive()
    {
        //PlayerController.instance.healthPlayer
        //Debug.Log("Lost live " + PlayerController.instance.healthPlayer);
        img_lives[PlayerController.instance.healthPlayer].SetActive(false);
    }

    private void StartGane()
    {
        //SceneManager.LoadScene(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // Khoi dong lai scene hien tai
    }

    public void ReStart()
    {
        if (score >= highScore && nameInputField.gameObject.activeSelf && !isHighScoreSaved)
        {
            txtEndPoint.text = "Please enter your name to save the score before restarting.";
        }
        else
        {
            StartGane();
            AudioManager.Instance.ResetMusic();
        }
    }

    public void RestartBottonClick()
    {
        btnRestart.GetComponent<Image>().sprite = btnClick;
    }

    public void RestartBottonHover()
    {
        btnRestart.GetComponent<Image>().sprite = btnHover;
    }

    public void RestartBottonIdle()
    {
        btnRestart.GetComponent<Image>().sprite = btnIdle;
    }

    public void GameOver()
    {
        AudioManager.Instance.StopBackgroundMusic();
        AudioManager.Instance.PlayGameOverSound();

        isEndGame = true;
        isStartFirstTime = false;

        Time.timeScale = 0; // Dung tat ca hd trong game
        pnlEndGame.SetActive(true); //Hthi giao dien Endgame
        //pausePanel.SetActive(false);
        txtEndPoint.text = "Your Score:\n" + score.ToString();

        if (score >= highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore); // Luu du lieu
            PlayerPrefs.Save();

            resetHighScoreButton.gameObject.SetActive(false);
            nameInputField.gameObject.SetActive(true);
            saveHighScoreButton.gameObject.SetActive(true);
            txtEndPoint.text = "Congratulations! You've got a new high score. Please enter your name and save.";
        }
        else
        {
            resetHighScoreButton.gameObject.SetActive(true);
            nameInputField.gameObject.SetActive(false);
            saveHighScoreButton.gameObject.SetActive(false);
            txtHighScore.text = $"High Score:\n {highScorePlayer} - {highScore}";
        }
    }

    public void SaveHighScore()
    {
        if (score >= highScore)
        {
            string playerName = nameInputField.text;
            if (!string.IsNullOrEmpty(playerName))
            {
                highScorePlayer = playerName;
                PlayerPrefs.SetString("HighScorePlayer", highScorePlayer);
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
                txtHighScore.text = $"High Score:\n {highScorePlayer} - {highScore}";

                isHighScoreSaved = true;

                nameInputField.gameObject.SetActive(false);
                saveHighScoreButton.gameObject .SetActive(false);
                
            }
            else
            {
                txtEndPoint.text = "Please enter your name to save the score.";
            }
        }
        else
        {
            Debug.Log("Your score is not high enough to save.");
        }    
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("HighScorePlayer");
        highScore = 0;
        highScorePlayer = "None";
        txtHighScore.text = $"High Score:\n {highScorePlayer} - {highScore}";
    }

    public void PauseGamebutton()
    {
        if (!isEndGame)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void ResumeButton()
    {
        if (!isEndGame)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    public void OptionsButton()
    {
        if (score >= highScore && nameInputField.gameObject.activeSelf && !isHighScoreSaved)
        {
            txtEndPoint.text = "Please enter your name to save the score before going to options.";
        }
        else
        {
            AudioManager.Instance.StopAllSounds();
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}
