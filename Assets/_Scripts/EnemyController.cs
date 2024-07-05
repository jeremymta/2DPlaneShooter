using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected int healthEnemy = 5;
    protected Vector3 targetPosition;
    protected float fireRate = 10f;
    protected float nextFireTime;

    private ScreenBounds screenBounds;
    public GameObject enemyBulletPrefab;

    private void Start()
    {
        // Set initial position to off-screen if needed
        targetPosition = transform.position;
        screenBounds = GetComponent<ScreenBounds>();

        nextFireTime = Time.time + Random.Range(0f, fireRate); //Randomize initial fire time
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    private void TakeDamage(int damage)
    {
        healthEnemy -= damage;
        if (healthEnemy <= 0)
        {
            GameManager.Instance.AddScore(1);
            Destroy(gameObject);
        }
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    private void Update()
    {
        targetPosition = screenBounds.ClampPosition(targetPosition);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2f);
        
        if (Time.time >= nextFireTime)
        {
            FireEnemy();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void FireEnemy()
    {
        if (enemyBulletPrefab != null)
        {
            GameObject bullet =  Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
            bullet.tag = "BulletEnemy";
            if (bullet != null)
            {
                bullet.GetComponent<BulletController>().SetDirection(Vector3.down);
            }
        }
    }
}
