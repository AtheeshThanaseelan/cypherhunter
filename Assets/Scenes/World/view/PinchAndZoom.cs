using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
//using System.Diagnostics;
using UnityEngine;

public class PinchAndZoom : MonoBehaviour
{
    float MouseZoomSpeed = 15.0f;
    float TouchZoomSpeed = 0.1f;
    float ZoomMinBound = 0.1f;
    float ZoomMaxBound = 100.9f;
    Vector3 position = Vector3.zero;
    Vector3 currentEulerAngles = Vector3.zero;
    Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.touchSupported)
        {
            // Pinch to zoom
            if (Input.touchCount == 2)
            {

                // get current touch positions
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);
                // get touch position from the previous frame
                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

                // get offset value
                float deltaDistance = oldTouchDistance - currentTouchDistance;
                Zoom(deltaDistance, TouchZoomSpeed);
            }
        }
        else
        {

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Zoom(scroll, MouseZoomSpeed);
        }



        if (cam.fieldOfView < ZoomMinBound)
        {
            cam.fieldOfView = 0.1f;
        }
        else
        if (cam.fieldOfView > ZoomMaxBound)
        {
            cam.fieldOfView = 179.9f;
        }
    }

    void Zoom(float deltaMagnitudeDiff, float speed)
    {
        position = transform.localPosition;
        position.y += deltaMagnitudeDiff * speed;
        position.x += deltaMagnitudeDiff * speed;
        position.z -= deltaMagnitudeDiff * speed;

        position.x = Mathf.Clamp(position.x, -2.3f, 0);
        position.z = Mathf.Clamp(position.z, 0, 35);
        position.y = Mathf.Clamp(position.y, 12, 750);

        currentEulerAngles = transform.eulerAngles;
        currentEulerAngles[0] = 1.2f * position.y;
        currentEulerAngles[0] = Mathf.Clamp(currentEulerAngles[0], 0, 90);

        transform.localPosition = position;
        transform.eulerAngles = currentEulerAngles;



        /*
        cam.fieldOfView += deltaMagnitudeDiff * speed;
        // set min and max value of Clamp function upon your requirement
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, ZoomMinBound, ZoomMaxBound);

        transform.eulerAngles = new Vector3((1.2f * cam.fieldOfView), 176.309f, 0);

        Debug.Log(transform.eulerAngles[0]);
        */
    }
}