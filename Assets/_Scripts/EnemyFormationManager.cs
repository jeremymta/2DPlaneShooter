using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFormationManager : MonoBehaviour
{
    private const float enemySpacing = 0.5f;
    private float enemyHeight;

    public void Initialize(float enemyHeight)
    {
        this.enemyHeight = enemyHeight;
    }
    public IEnumerator EnemyFormationSequence(List<EnemyController> enemies)
    {
        while (!GameManager.Instance.isEndGame)
        {
            yield return StartCoroutine(MoveEnemiesToSquareFormation(enemies));
            yield return new WaitForSeconds(5f);
            SetDiamondFormation(enemies);
            yield return new WaitForSeconds(5f);
            SetTriangleFormation(enemies);
            yield return new WaitForSeconds(5f);
            SetRectangleFormation(enemies);
            yield return new WaitForSeconds(5f);
        }

    }


    IEnumerator MoveEnemiesToSquareFormation(List<EnemyController> enemies)
    {
        yield return new WaitForSeconds(1f);

        float screenTop = Camera.main.orthographicSize;
        float yOffset = screenTop - enemyHeight * 3 / 2;

        // 
        float formationSize = enemySpacing * 3;

        // 
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

    void SetDiamondFormation(List<EnemyController> enemies)
    {
        float screenTop = Camera.main.orthographicSize;
        float yOffset = screenTop - enemyHeight * 3 / 2;
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

    void SetTriangleFormation(List<EnemyController> enemies)
    {
        float screenTop = Camera.main.orthographicSize;
        float yOffset = screenTop - enemyHeight * 3 / 2;
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

    void SetRectangleFormation(List<EnemyController> enemies)
    {
        float screenTop = Camera.main.orthographicSize;
        float yOffset = screenTop - enemyHeight * 3 / 2;
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

}
