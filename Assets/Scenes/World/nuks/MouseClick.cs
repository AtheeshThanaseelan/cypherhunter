using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class MouseClick : MonoBehaviour
{
	[SerializeField]
	[Range(1, 20)]
	public float _panSpeed = 1.0f;

	[SerializeField]
	float _zoomSpeed = 0.25f;

	[SerializeField]
	public Camera _referenceCamera;

	[SerializeField]
	AbstractMap _mapManager;

	[SerializeField]
	bool _useDegreeMethod;

	private Vector3 _origin;
	private Vector3 _mousePosition;
	private Vector3 _mousePositionPrevious;
	private bool _shouldDrag;
	private bool _isInitialized = false;
	private Plane _groundPlane = new Plane(Vector3.up, 0);

	public static Vector2 latLong;

	void Awake()
	{
		if (null == _referenceCamera)
		{
			_referenceCamera = GetComponent<Camera>();
			if (null == _referenceCamera) { Debug.LogErrorFormat("{0}: reference camera not set", this.GetType().Name); }
		}
		_mapManager.OnInitialized += () =>
		{
			_isInitialized = true;
		};
	}


	private void LateUpdate()
	{
		if (!_isInitialized) { return; }

		if (Input.touchSupported && Input.touchCount > 0)
		{
			HandleTouch();
		}
		else
		{
			HandleMouseAndKeyBoard();
		}
	}

	void HandleMouseAndKeyBoard()
	{
		/*
		// zoom
		float scrollDelta = 0.0f;
		scrollDelta = Input.GetAxis("Mouse ScrollWheel");
		ZoomMapUsingTouchOrMouse(scrollDelta);


		//pan keyboard
		float xMove = Input.GetAxis("Horizontal");
		float zMove = Input.GetAxis("Vertical");

		PanMapUsingKeyBoard(xMove, zMove);

		*/
		//pan mouse
		PanMapUsingTouchOrMouse();
	}

	void HandleTouch()
	{
		float zoomFactor = 0.0f;
		//pinch to zoom. 
		switch (Input.touchCount)
		{
			case 1:
				{
					PanMapUsingTouchOrMouse();
				}
				break;
			default:
				break;
		}
	}

	void PanMapUsingTouchOrMouse()
	{
		if (_useDegreeMethod)
		{
			UseDegreeConversion();
		}
		else
		{
			UseMeterConversion();
		}
	}

	void UseMeterConversion()
	{
			var mousePosScreen = Input.mousePosition;
			//assign distance of camera to ground plane to z, otherwise ScreenToWorldPoint() will always return the position of the camera
			//http://answers.unity3d.com/answers/599100/view.html
			mousePosScreen.z = _referenceCamera.transform.localPosition.y;
			var pos = _referenceCamera.ScreenToWorldPoint(mousePosScreen);

		Debug.Log("gamepos: " + pos);

			var latlongDelta = _mapManager.WorldToGeoPosition(pos);
			//Debug.Log("Latitude: " + latlongDelta.x + " Longitude: " + latlongDelta.y);
			latLong = new Vector2((float)latlongDelta.x, (float)latlongDelta.y);
			//_mapManager.UpdateMap(latlongDelta, _mapManager.Zoom);
	}

	void UseDegreeConversion()
	{
		if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
		{
			var mousePosScreen = Input.mousePosition;
			//assign distance of camera to ground plane to z, otherwise ScreenToWorldPoint() will always return the position of the camera
			//http://answers.unity3d.com/answers/599100/view.html
			mousePosScreen.z = _referenceCamera.transform.localPosition.y;
			_mousePosition = _referenceCamera.ScreenToWorldPoint(mousePosScreen);

			if (_shouldDrag == false)
			{
				_shouldDrag = true;
				_origin = _referenceCamera.ScreenToWorldPoint(mousePosScreen);
			}
		}
		else
		{
			_shouldDrag = false;
		}

		if (_shouldDrag == true)
		{
			var changeFromPreviousPosition = _mousePositionPrevious - _mousePosition;
			if (Mathf.Abs(changeFromPreviousPosition.x) > 0.0f || Mathf.Abs(changeFromPreviousPosition.y) > 0.0f)
			{
				_mousePositionPrevious = _mousePosition;
				var offset = _origin - _mousePosition;

				if (Mathf.Abs(offset.x) > 0.0f || Mathf.Abs(offset.z) > 0.0f)
				{
					if (null != _mapManager)
					{
						// Get the number of degrees in a tile at the current zoom level. 
						// Divide it by the tile width in pixels ( 256 in our case) 
						// to get degrees represented by each pixel.
						// Mouse offset is in pixels, therefore multiply the factor with the offset to move the center.
						float factor = _panSpeed * Conversions.GetTileScaleInDegrees((float)_mapManager.CenterLatitudeLongitude.x, _mapManager.AbsoluteZoom) / _mapManager.UnityTileSize;
						//MapLocationOptions locationOptions = new MapLocationOptions
						//{
						//	latitudeLongitude = String.Format("{0},{1}", _mapManager.CenterLatitudeLongitude.x + offset.z * factor, _mapManager.CenterLatitudeLongitude.y + offset.x * factor),
						//	zoom = _mapManager.Zoom
						//};
						var latitudeLongitude = new Vector2d(_mapManager.CenterLatitudeLongitude.x + offset.z * factor, _mapManager.CenterLatitudeLongitude.y + offset.x * factor);
						_mapManager.UpdateMap(latitudeLongitude, _mapManager.Zoom);
					}
				}
				_origin = _mousePosition;
			}
			else
			{
				if (EventSystem.current.IsPointerOverGameObject())
				{
					return;
				}
				_mousePositionPrevious = _mousePosition;
				_origin = _mousePosition;
			}
		}
	}

	private Vector3 getGroundPlaneHitPoint(Ray ray)
	{
		float distance;
		if (!_groundPlane.Raycast(ray, out distance)) { return Vector3.zero; }
		return ray.GetPoint(distance);
	}

}
