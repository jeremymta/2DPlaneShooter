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
}
