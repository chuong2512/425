﻿using UnityEngine;
using System.Collections;

public class CameraController {

	public static float heightInMeters;
	public static float widthInMeters;

	public static float pixelSize;

	public static Vector2 cameraPosition {
		get { return new Vector2 (Camera.main.transform.position.x, Camera.main.transform.position.z);}
		set {
			Camera.main.transform.position = new Vector3 (value.x,Camera.main.transform.position.y,value.y);
			GUIController.OnCameraUpdate();
		}
	}

	public static float cameraSize {

		get { return Camera.main.orthographicSize; }

		set {
			Camera.main.orthographicSize = Mathf.Clamp(value,0.5f,25);
			widthInMeters = Camera.main.orthographicSize/Screen.height*(2f*Screen.width);
			heightInMeters = GetHeightInMeters(widthInMeters);
			pixelSize = widthInMeters / Screen.width;
			GUIController.OnCameraUpdate();
		}
	}


	public static float GetWidthInMeters (float _heightInMeters) {
		return _heightInMeters*Screen.width/(Screen.height);
	}

	public static float GetHeightInMeters (float _widthInMeters) {
		return _widthInMeters*Screen.height/(Screen.width);
	}
	
	public static void ResizeCamera (float _widthInMeters) {
		widthInMeters = _widthInMeters;
		Camera.main.orthographicSize = widthInMeters*Screen.height/(2f*Screen.width);	
		heightInMeters = GetHeightInMeters(widthInMeters);
		
		pixelSize = widthInMeters / Screen.width;

		GUIController.OnCameraUpdate();

	}

}
