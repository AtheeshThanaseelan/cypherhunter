using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;

public class SpawnOnCoordinates : MonoBehaviour
{
    public GameObject game;

    [SerializeField]
    AbstractMap _mapManager;

    public static bool airstrike= false;
    public static int count= 0;

    private bool clicked = false;
    // Update is called once per frame
    void Update()
    {
        
        // Get mouse position (in screen-space) and convert to world-space
        // var mousePos = myCamera.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the vector between the mouse position and object
        // var vector = myGameObject.transform.position - mousePos;

        // And then the distance
        // var distance = vector.magnitude;
        //Debug.Log("MouseClick X: " + MouseClick.latLong.x + "MouseClick Y: " + MouseClick.latLong.y);
        if(airstrike){
            
            if (Input.GetMouseButtonDown(0))
            {
                clicked = !clicked;
            }
            if (clicked)
            { 
                if (Input.GetMouseButtonUp(0))
                {
                    clicked = !clicked;
                    count++;

                    if (count > 1)
                    {
                        var wurld_pos = _mapManager.GeoToWorldPosition(new Vector2d(MouseClick.latLong.x, MouseClick.latLong.y));

                        Debug.Log(wurld_pos);

                        Instantiate(game, wurld_pos, Quaternion.identity);
                        // airstrike= false;
                        // count++;
                    }
                }
            }
            Debug.Log("Airstrike: " + airstrike + "cilkd" + clicked);
        }
        else {
            clicked = false;
            count = 0;
            Debug.Log("Airstrike: " + airstrike);
        }
    }
}
