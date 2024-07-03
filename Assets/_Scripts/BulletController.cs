using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction = Vector3.up;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction); 

        //if (transform.position.y > 10)
        //{
        //    Destroy(gameObject);
        //}
        
        // Huy dan khi ra khoi man hinh

        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //Debug.Log("Collision detected with: " + collision.gameObject.name);
    //    if (direction == Vector3.down && collision.CompareTag("Player")) // dan cua enemy
    //    {
    //        PlayerController player = collision.GetComponent<PlayerController>();
    //        if (player != null)
    //        {
    //            Debug.Log("Bullet hit player");
    //            player.TakeDamage(1);
    //        }
    //        Destroy(gameObject);
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (direction == Vector3.up && collision.CompareTag("Enemy")) // dan cua  player
    //    {
    //        EnemyController enemy = collision.GetComponent<EnemyController>();
    //        if (enemy != null)
    //        {
    //            Debug.Log("Bullet hit enemy");
    //            enemy.TakeDamage(1);
    //        }
    //        Destroy(gameObject);
    //    }
    //    else if (direction == Vector3.down && collision.CompareTag("Player")) // Dan cua enemy
    //    {
    //        PlayerController player = collision.GetComponent<PlayerController>();
    //        if (player != null)
    //        {
    //            Debug.Log("Bullet hit player");
    //            player.TakeDamage(1);
    //        }
    //        Destroy(gameObject);
    //    }
    //}

}
