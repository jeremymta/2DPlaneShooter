using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health = 3;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }    

    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public float fireRate = 0.1f;

    private float nextFireTime;

    private ScreenBounds screenBounds;

    private void Start()
    {
        screenBounds = GetComponent<ScreenBounds>();
    }

    void Update()
    {
        MoveTowardsMouse();
        Shoot();
    }

    void MoveTowardsMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;

        Vector3 targetPosition = Vector3.MoveTowards(transform.position, mousePosition, moveSpeed * Time.deltaTime);
        targetPosition = screenBounds.ClampPosition(targetPosition);

        transform.position = targetPosition;  

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Bullet hit player");
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    void Shoot()
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
