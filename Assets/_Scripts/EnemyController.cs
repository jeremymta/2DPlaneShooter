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
        //Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            //Debug.Log("BulletPlayer hit enemy");
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player hit Enemy");
            TakeDamage(1); // Tru 1 healthEnemy (Takedame(n): trong do n la luong sat thuong)
            //TakeDamage(healthEnemy); // Tru toan bo healthEnemy
        }
    }

    private void TakeDamage(int damage) // Logic nhan sat thuong
    {
        healthEnemy -= damage;
        //Debug.Log("Enemy health: " + healthEnemy);
        if (healthEnemy <= 0)
        {
            GameManager.Instance.AddScore(1);
            Destroy(gameObject);
            //Debug.Log("Enemy destroyed");
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
            //Debug.Log("Spawning bullet");
            GameObject bullet =  Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
            bullet.tag = "BulletEnemy";
            if (bullet != null)
            {
                bullet.GetComponent<BulletController>().SetDirection(Vector3.down);
            }

        }

    }
}
