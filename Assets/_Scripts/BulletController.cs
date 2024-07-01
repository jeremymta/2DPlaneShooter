using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.up); 

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
   
}
