using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGLooper : MonoBehaviour
{
    public float speed = 0.1f;

    private Vector2 _offset = Vector2.zero;

    private Material _mat;
    // Start is called before the first frame update
    void Start()
    {
        _mat = GetComponent<Renderer>().material;
        _offset = _mat.GetTextureOffset("_MainTex");
    }

    // Update is called once per frame
    void Update()
    {
        //Tao hieu ung cuon doc cho main texture (_MainTex) cua meterial duoc gan vao GameObject
        _offset.y += speed * Time.deltaTime;
        _mat.SetTextureOffset("_MainTex", _offset);
    }
}
