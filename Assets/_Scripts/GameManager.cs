using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject enemyPrefab;
    public int score = 0;
    public List<EnemyController> enemies = new List<EnemyController>();

    private int currentFormation = 0;
    private float formationChangeInterval = 5f;

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
        SpawnEnemies();
        StartCoroutine(ChangeFormation());
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector3 spawnPosition = new Vector3(j * 1.5f - 2.25f, i * 1.5f + 6f, 0);
                GameObject enemyObj = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                EnemyController enemy = enemyObj.GetComponent<EnemyController>();
                enemies.Add(enemy);
            }
        }
    }

    IEnumerator ChangeFormation()
    {
        while (true)
        {
            yield return new WaitForSeconds(formationChangeInterval);
            currentFormation = (currentFormation + 1) % 4;
            SetFormation(currentFormation);
        }
    }

    void SetFormation(int formationIndex)
    {
        switch (formationIndex)
        {
            case 0: SetSquareFormation(); break;
            case 1: SetDiamondFormation(); break;
            case 2: SetTriangleFormation(); break;
            case 3: SetRectangleFormation(); break;
        }
    }

    void SetSquareFormation()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            int row = i / 4;
            int col = i % 4;
            Vector3 position = new Vector3(col * 1.5f - 2.25f, row * 1.5f + 3f, 0);
            enemies[i].SetTargetPosition(position);
        }
    }

    void SetDiamondFormation()
    {
        int mid = enemies.Count / 2;
        for (int i = 0; i < enemies.Count; i++)
        {
            int offset = Mathf.Abs(mid - i);
            Vector3 position = new Vector3((i - mid) * 1.5f, offset * 1.5f + 3f, 0);
            enemies[i].SetTargetPosition(position);
        }
    }

    void SetTriangleFormation()
    {
        int index = 0;
        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col <= row; col++)
            {
                if (index < enemies.Count)
                {
                    Vector3 position = new Vector3(col * 1.5f - row * 0.75f, row * 1.5f + 3f, 0);
                    enemies[index].SetTargetPosition(position);
                    index++;
                }
            }
        }
    }

    void SetRectangleFormation()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            int row = i / 4;
            int col = i % 4;
            Vector3 position = new Vector3(col * 1.5f - 2.25f, row * 1.5f + 3f, 0);
            enemies[i].SetTargetPosition(position);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Score: " + score);
    }
}
