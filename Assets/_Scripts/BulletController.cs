using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y > 10)
        {
            Destroy(gameObject);
        }
    }

   
}
