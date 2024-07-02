using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    public float moveSpeed;
    public float moveRange;

    private Vector3 oldPoistion;
    private GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        obj = gameObject;
        oldPoistion = obj.transform.position;
        moveSpeed = 3f;
        moveRange = 22f;

    }

    // Update is called once per frame
    void Update()
    {
        obj.transform.Translate(new Vector3(0, -1 * Time.deltaTime * moveSpeed, 0));

        if (Vector3.Distance(oldPoistion, obj.transform.position) > moveRange)
        {
            obj.transform.position = oldPoistion;
        }
    }
}
