using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 5;
    public Vector3 targetPosition;

    private ScreenBounds screenBounds;

    public GameObject enemyBulletPrefab;
    public float fireRate = 10f;

    private float nextFireTime;

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
        if (collision.gameObject.CompareTag("Bullet"))
        {
            //Debug.Log("Bullet hit enemy");
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        //Debug.Log("Enemy health: " + health);
        if (health <= 0)
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
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        if (enemyBulletPrefab != null)
        {
            //Debug.Log("Spawning bullet");
            GameObject bullet =  Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
            if (bullet != null)
            {
                bullet.GetComponent<BulletController>().SetDirection(Vector3.down);
            }

        }

    }
}
