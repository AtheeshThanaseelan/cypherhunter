using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class littlebig : MonoBehaviour
{
    private float scale = 1;
    private bool grow = true;

    public float max_scale = 50;
    public float min_scale = 1;
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = (new Vector3(1,1,1));
    }

    // Update is called once per frame
    void Update()
    {
        if (scale > max_scale)
            grow = false;
        if (scale < min_scale)
            grow = true;
        if (grow)
            scale += speed;
        else
            scale -= speed;
        transform.localScale =(new Vector3(scale, scale, scale));
    }
}
