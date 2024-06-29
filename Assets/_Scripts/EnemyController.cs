using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 5;
    public Vector3 targetPosition;

    //private ScreenBounds screenBounds;

    //private bool isFormattionSet = false;

    private void Start()
    {
        //screenBounds = GetComponent<ScreenBounds>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.Instance.AddScore(1);
            Destroy(gameObject);
        }
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
        //isFormattionSet = true;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2f);

        //if (isFormattionSet)
        //{
        //    // Clamp position only after formation is set
        //    targetPosition = ClampPositionToScreen(targetPosition);
        //}
    }

    //private Vector3 ClampPositionToScreen(Vector3 position)
    //{
    //    float minX = -8f; // Set these values based on your screen bounds
    //    float maxX = 8f;
    //    float minY = -4.5f;
    //    float maxY = 4.5f;

    //    position.x = Mathf.Clamp(position.x, minX, maxX);
    //    position.y = Mathf.Clamp(position.y, minY, maxY);

    //    return position;
    //}
}
