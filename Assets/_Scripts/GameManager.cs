using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

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
        GameObject tempEnemy = Instantiate(enemyPrefab, offScreenPosition, Quaternion.identity);
        SpriteRenderer spriteRenderer = tempEnemy.GetComponent<SpriteRenderer>();
        enemyHeight = spriteRenderer.bounds.size.y;
        Destroy(tempEnemy);

        SpawnEnemies();
        StartCoroutine(EnemyFormationSequence());
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
        float screenTop = Camera.main.orthographicSize;
        float yOffset = screenTop - enemyHeight / 2;
        for (int i = 0; i < enemies.Count; i++)
        {
            int row = i / 4;
            int col = i % 4;
            Vector3 position = new(col * enemySpacing - 1.5f, yOffset - row * enemySpacing, 0);
            enemies[i].SetTargetPosition(position);
        }
    }

    void SetDiamondFormation()
    {
        float screenTop = Camera.main.orthographicSize;
        float yOffset = screenTop - enemyHeight / 2;
        float xCenter = 0f;
        float yCenter = yOffset - 2f * enemySpacing;

        int[] enemiesPerRow = { 1, 4, 6, 4, 1 };
        int enemyIndex = 0;

        for (int row = 0; row < enemiesPerRow.Length; row++)
        {
            int enemiesInThisRow = enemiesPerRow[row];
            float rowWidth = (enemiesInThisRow - 1) * enemySpacing;
            float startX = xCenter - rowWidth / 2;

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

    public void GameOver()
    {
        Debug.Log("Game Over");

    }
}
