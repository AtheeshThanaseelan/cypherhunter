using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class createobj : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate(prefab, new Vector3(i * 2.0F, 0, 0), Quaternion.identity);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var current_position = Input.mousePosition;
            Debug.Log(current_position);

        }
        
        
    }
}

