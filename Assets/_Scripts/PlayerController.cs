using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Only 1 Player allow to exist");
            Destroy(gameObject);
            return;
        }
    }

    public int healthPlayer = 3;
    protected float fireRate = 0.1f;
    private float nextFireTime;

    protected float moveSpeed = 5f;
    public GameObject bulletPrefab;
    private ScreenBounds screenBounds;

    private void Start()
    {
        //instance = this;
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

    private void TakeDamage(int damage) // Logic nhan sat thuong
    {
        
        healthPlayer -= damage;
        GameManager.Instance.LostLive();
        AudioManager.Instance.PlayPlayerHitSound();

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
        if (PlayerController.instance.healthPlayer > 0)
        {
            if (collision.gameObject.CompareTag("BulletEnemy"))
            {
                //Debug.Log("Collision detected with: " + collision.gameObject.name);
                //Debug.Log("BulletEnemy hit player");
                TakeDamage(1);
                Destroy(collision.gameObject); // Huy dt dan cua Enemy 
            }
            else if (collision.gameObject.CompareTag("Enemy"))
            {
                //Debug.Log("Enemy hit player");
                TakeDamage(1);
                //Destroy(collision.gameObject); // Huy dt va cham voi player, trong TH nay la dt co the "Enemy"
            }
        }
    }

    private void Shoot()
    {
        
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            AudioManager.Instance.PlayShootSound();

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
