using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    protected int healthPlayer = 2;
    protected float fireRate = 0.1f;
    private float nextFireTime;

    protected float moveSpeed = 5f;
    public GameObject bulletPrefab;
    private ScreenBounds screenBounds;

    private void Start()
    {
        screenBounds = GetComponent<ScreenBounds>();
    }

    private void Update()
    {
        MoveTowardsMouse();
        Shoot();
    }

    private void MoveTowardsMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;

        Vector3 targetPosition = Vector3.MoveTowards(transform.position, mousePosition, moveSpeed * Time.deltaTime);
        targetPosition = screenBounds.ClampPosition(targetPosition);

        transform.position = targetPosition;  

    }

    private void TakeDamage(int damage)
    {
        healthPlayer -= damage;
        
        if (healthPlayer > 0)
        {
            GameManager.Instance.UpdateLives(healthPlayer);
        }
        else
        {
            healthPlayer = 0;
            GameManager.Instance.UpdateLives(healthPlayer); // Cap nhat so mang cuoi cung
            GameManager.Instance.GameOver();
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BulletEnemy"))
        {
            //Debug.Log("Collision detected with: " + collision.gameObject.name);
            //Debug.Log("BulletEnemy hit player");
            TakeDamage(1);
            Destroy(collision.gameObject); // Huy dan cua Enemy 

            //Debug.Log("Player health: " + healthPlayer);
        }
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            if (bulletPrefab != null)
            {
                //Debug.Log("Spawning bullet");
                Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                nextFireTime = Time.time + fireRate;
            }
            else
            {
                Debug.LogWarning("Bullet prefab is missing or destroyed.");
            }
        }
    }
}
