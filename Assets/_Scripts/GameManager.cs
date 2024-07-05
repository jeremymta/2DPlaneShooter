using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject enemyPrefab;
    public List<EnemyController> enemies = new();
    private Vector3 offScreenPosition = new(-10f, 6f, 0);

    private const float enemySpacing = 0.5f;
    private float enemyHeight;

    public int score = 0;
    public Text txtScore;
    public Text txtLive;
    public GameObject pnlEndGame;
    public Text txtEndPoint;
    public Button btnRestart;

    public Sprite btnIdle;
    public Sprite btnHover;
    public Sprite btnClick;

    public bool isEndGame;
    private bool isStartFirstTime;

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
        Time.timeScale = 1.0f; //0
        isEndGame = false;
        pnlEndGame.SetActive(false);
        isStartFirstTime = true;

        GameObject tempEnemy = Instantiate(enemyPrefab, offScreenPosition, Quaternion.identity);
        SpriteRenderer spriteRenderer = tempEnemy.GetComponent<SpriteRenderer>();
        enemyHeight = spriteRenderer.bounds.size.y;
        Destroy(tempEnemy);

        SpawnEnemies();
        StartCoroutine(EnemyFormationSequence());

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
        else
        {
            if (Input.GetMouseButtonDown(0) && Time.timeScale == 0)
            {
                Time.timeScale = 1;
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

    IEnumerator EnemyFormationSequence()
    {
        while (true)
        {
            yield return StartCoroutine(MoveEnemiesToSquareFormation());
            yield return new WaitForSeconds(5f);
            SetDiamondFormation();
            yield return new WaitForSeconds(5f);
            SetTriangleFormation();
            yield return new WaitForSeconds(5f);
            SetRectangleFormation();
            yield return new WaitForSeconds(5f);
        }

    }

    IEnumerator MoveEnemiesToSquareFormation()
    {
        yield return new WaitForSeconds(1f);

        float screenTop = Camera.main.orthographicSize; // chieu cao cua man hinh
        float yOffset = screenTop - enemyHeight / 2; // Khoang cach enemy voi screenTop

        // tong chieu rong/chieu cao cua hinh vuong
        float formationSize = enemySpacing * 3;

        // Tinh toan toa do bat dau cua hinh vuong
        float xCenter = 0f;
        float xOffset = xCenter - formationSize / 2;

        for (int i = 0; i < enemies.Count; i++)
        {
            int row = i / 4;
            int col = i % 4;
            Vector3 position = new(col * enemySpacing + xOffset, yOffset - row * enemySpacing, 0);
            enemies[i].SetTargetPosition(position);
        }
    }

    void SetDiamondFormation()
    {
        float screenTop = Camera.main.orthographicSize;
        float yOffset = screenTop - enemyHeight / 2;
        float xCenter = 0f; // toa do trung tam theo truc x cua man hinh
        float yCenter = yOffset - 2f * enemySpacing; // toa do trung tam theo truc y cua hinh thoi

        int[] enemiesPerRow = { 1, 4, 6, 4, 1 };
        int enemyIndex = 0;

        // vong lap dau tien duyet qua tung hang cua hinh thoi
        for (int row = 0; row < enemiesPerRow.Length; row++)
        {
            int enemiesInThisRow = enemiesPerRow[row];
            float rowWidth = (enemiesInThisRow - 1) * enemySpacing;
            float startX = xCenter - rowWidth / 2;

            // Vong lap hai duyet qua tung cot cua hinh thoi
            for (int col = 0; col < enemiesInThisRow; col++)
            {
                if (enemyIndex < enemies.Count)
                {
                    float x = startX + col * enemySpacing;
                    float y = yCenter + (2 - row) * enemySpacing;
                    Vector3 position = new(x, y, 0);
                    enemies[enemyIndex].SetTargetPosition(position);
                    enemyIndex++;
                }
            }
        }
    }

    void SetTriangleFormation()
    {
        float screenTop = Camera.main.orthographicSize;
        float yOffset = screenTop - enemyHeight / 2;
        float xCenter = 0f;

        int baseEnemies = 9;
        float baseWidth = (baseEnemies - 1) * enemySpacing;
        float startX = xCenter - baseWidth / 2;
        float height = 4 * enemySpacing; // Chiều cao của tam giác

        // Đặt enemy ở đỉnh tam giác
        enemies[0].SetTargetPosition(new Vector3(xCenter, yOffset, 0));

        // Đặt enemy ở hai cạnh bên
        for (int i = 1; i < 4; i++)
        {
            float y = yOffset - i * (height / 4);
            float xOffset = i * (baseWidth / 8);

            enemies[i * 2 - 1].SetTargetPosition(new Vector3(xCenter - xOffset, y, 0));
            enemies[i * 2].SetTargetPosition(new Vector3(xCenter + xOffset, y, 0));
        }

        // Đặt enemy ở cạnh đáy
        for (int i = 0; i < baseEnemies; i++)
        {
            float x = startX + i * enemySpacing;
            enemies[7 + i].SetTargetPosition(new Vector3(x, yOffset - height, 0));
        }
    }

    void SetRectangleFormation()
    {
        float screenTop = Camera.main.orthographicSize;
        float yOffset = screenTop - enemyHeight / 2;
        float xCenter = 0f;

        int width = 7;
        int height = 3;
        float rectangleWidth = (width - 1) * enemySpacing;
        float rectangleHeight = (height - 1) * enemySpacing;

        float startX = xCenter - rectangleWidth / 2;
        float startY = yOffset - rectangleHeight - enemySpacing; // Thêm khoảng cách để hạ thấp hình chữ nhật

        int enemyIndex = 0;

        // Đặt enemy ở cạnh trên (7 enemy)
        for (int i = 0; i < width; i++)
        {
            float x = startX + i * enemySpacing;
            enemies[enemyIndex++].SetTargetPosition(new Vector3(x, startY + rectangleHeight, 0));
        }

        // Đặt enemy ở cạnh dưới (7 enemy)
        for (int i = 0; i < width; i++)
        {
            float x = startX + i * enemySpacing;
            enemies[enemyIndex++].SetTargetPosition(new Vector3(x, startY, 0));
        }

        // Đặt enemy ở hai cạnh bên (1 enemy mỗi bên, không tính các góc)
        for (int i = 1; i < height - 1; i++)
        {
            float y = startY + i * enemySpacing;
            enemies[enemyIndex++].SetTargetPosition(new Vector3(startX, y, 0));
            enemies[enemyIndex++].SetTargetPosition(new Vector3(startX + rectangleWidth, y, 0));
        }
    }

    public void AddScore(int points)
    {
        score += points;
        txtScore.text = "Score\n" + score.ToString();
        //Debug.Log("Score: " + score);
    }

    public void UpdateLives(int lives)
    {
        //Debug.Log("Lives: " + lives);
        txtLive.text = "Lives\n" + lives.ToString();
    }

    private void StartGane()
    {
        //SceneManager.LoadScene(0);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // Khoi dong lai scene hien tai
    }

    public void ReStart()
    {
        Debug.Log("Test Restart");
        StartGane();

        AudioManager.Instance.ResetMusic();
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
        Debug.Log("Game Over");

        AudioManager.Instance.StopBackgroundMusic();
        AudioManager.Instance.PlayGameOverSound();

        isEndGame = true;
        isStartFirstTime = false;

        Time.timeScale = 0; // Dung tat ca hd trong game
        pnlEndGame.SetActive(true); //Hthi giao dien Endgame
        txtEndPoint.text = "Your Score:\n" + score.ToString();
    }
}
