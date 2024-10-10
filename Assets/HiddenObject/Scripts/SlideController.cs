using UnityEngine;
using System.Collections;

public class SlideController {

	public enum Mode {Slide, Zoom, SlideAndZoom, ReadOnly}
    public static Mode mode;

    public static float frictionDelta = 1;
	public static float frictionZoom = 0.002f;

	public static float mapWidth = 0;
	public static float mapHeight = 0;
	
	private static bool isSliding = false;
	private static bool isZooming = false;

	private static Vector2 firstTouchStart = new Vector2 (-100,-100);
	private static Vector2 secondTouchStart = new Vector2 (-100,-100);
	public static float zoomDeltaStart = 0;
	public static float mapWidthStart = 0;
	
	private static Vector2 firstTouch = new Vector2 (0,0);
	private static Vector2 secondTouch = new Vector2 (0,0);
	
	private static Vector2 delta = new Vector2 (0,0);
	public static float zoomDelta = 0;

	private static Vector3 cameraPositionStart;

	private static float timeOver = -100;
	private static float timeOverMax = 2f;


	public static void Create (float _mapWidth = 0, float _mapHeight = 0, Mode _mode = Mode.SlideAndZoom) {
        
		mapWidth = _mapWidth;
		mapHeight = _mapHeight;
        mode = _mode;
	}

	private static bool SetCameraPositionX (float x) {

		if (mapWidth + GameController.interfaceWidth <= CameraController.widthInMeters) {

			CameraController.cameraPosition = new Vector2 (-GameController.interfaceWidth / 2f, CameraController.cameraPosition.y);
			return false;
		}

		if ( (x >  (mapWidth - CameraController.widthInMeters)/2 )) {
			CameraController.cameraPosition = new Vector2((mapWidth - CameraController.widthInMeters)/2,Camera.main.transform.position.z);
			return false;
		}

		if ((-x >  (mapWidth - CameraController.widthInMeters)/2 + GameController.interfaceWidth )) {
			CameraController.cameraPosition = new Vector2(-((mapWidth - CameraController.widthInMeters)/2 + GameController.interfaceWidth)
			                                              ,Camera.main.transform.position.z);
			return false;
		}



		CameraController.cameraPosition = new Vector2(x,Camera.main.transform.position.z);

		return true;
	}
	
	private static bool SetCameraPositionY (float y) {
		
		if (mapHeight <= CameraController.heightInMeters) {

			CameraController.cameraPosition = new Vector2 (CameraController.cameraPosition.x,0);
			return false;
		}

		if (Mathf.Abs(y) >  (mapHeight - CameraController.heightInMeters)/2){
			CameraController.cameraPosition = new Vector2(Camera.main.transform.position.x,Mathf.Sign(y)*(mapHeight - CameraController.heightInMeters)/2);
			return false;
		}

		CameraController.cameraPosition = new Vector2(Camera.main.transform.position.x,y);
		
		if (timeOver <= -100)
			timeOver = timeOverMax;
		return true;
	}


	private static void SetTouchesStart() {

		if (GameController.platform == GameController.Platform.PC) {
			if (Input.GetMouseButtonDown (1)) {
				secondTouchStart = Input.mousePosition;
			} else {
				firstTouchStart = Input.mousePosition;
			}
		} else {
			firstTouchStart = Input.touches[0].position;
			if (Input.touchCount > 1) 
				secondTouchStart = Input.touches[1].position;
		}

	}

	
	private static void SetTouches() {
		
		if (GameController.platform == GameController.Platform.PC) {
			firstTouch = Input.mousePosition;
			secondTouch = secondTouchStart;
		} else {
			if (Input.touchCount >=1)
				firstTouch = Input.touches[0].position;

			if (Input.touchCount > 1) 
				secondTouch = Input.touches[1].position;
		}
		
	}

	public static void ResizeCamera(float width) {
		
		if (mapHeight<= CameraController.GetHeightInMeters (width)) {
	
			CameraController.ResizeCamera( Mathf.Min(CameraController.GetWidthInMeters(mapHeight), mapWidth + GameController.interfaceWidth) );
			SetCameraPositionX (CameraController.cameraPosition.x);
			SetCameraPositionY (CameraController.cameraPosition.y);
			return;
		}
		
		if (width <= 19) {

			CameraController.ResizeCamera( Mathf.Min(CameraController.GetWidthInMeters(mapHeight), 19) );
			SetCameraPositionX (CameraController.cameraPosition.x);
			SetCameraPositionY (CameraController.cameraPosition.y);
			return;
		}

		if (width >= mapWidth + GameController.interfaceWidth) {

			CameraController.ResizeCamera( Mathf.Min(CameraController.GetWidthInMeters(mapHeight), mapWidth + GameController.interfaceWidth) );
			SetCameraPositionX (CameraController.cameraPosition.x);
			SetCameraPositionY (CameraController.cameraPosition.y);
			return;
		}

		if (Camera.main.transform.position.x >=  (mapWidth - width)/2) {

			CameraController.cameraPosition = new Vector2((mapWidth - width)/2,Camera.main.transform.position.z);
		}
		
		if (- Camera.main.transform.position.x >=  (mapWidth - width)/2 + GameController.interfaceWidth) {
			
			CameraController.cameraPosition = new Vector2(-((mapWidth - width)/2 + GameController.interfaceWidth),Camera.main.transform.position.z);
		}
		
		if (Mathf.Abs(Camera.main.transform.position.z) >=  (mapHeight - CameraController.GetHeightInMeters(width))/2) {
			CameraController.cameraPosition = new Vector2(Camera.main.transform.position.x,Mathf.Sign(Camera.main.transform.position.z)*(mapHeight - CameraController.GetHeightInMeters(width))/2);

		}

		CameraController.ResizeCamera(width);
		SetCameraPositionX (CameraController.cameraPosition.x);
		SetCameraPositionY (CameraController.cameraPosition.y);


	}

	public static void SlideControll() {
		
		if (GameController.platform == GameController.Platform.PC) {

			
			float xAxisValue = Input.GetAxis("Horizontal");
			float yAxisValue = Input.GetAxis("Vertical");
			float zAxisValue = 0;//Input.GetAxis("Perspective");
			if (xAxisValue*xAxisValue+yAxisValue*yAxisValue>0) {
				CameraController.cameraPosition +=new Vector2(xAxisValue, yAxisValue);
			}
			
			if (zAxisValue!=0) {
				CameraController.cameraSize+= zAxisValue;
			}


			if (Input.GetMouseButtonDown(0)) {
				isSliding = true;
				SetTouchesStart();
				cameraPositionStart = Camera.main.transform.position;
			}	
			/*
			if (Input.GetMouseButtonDown (1)) {
				isSliding = false;
				isZooming = true;
				SetTouchesStart();
				zoomDeltaStart = Vector2.Distance(firstTouchStart,secondTouchStart);
				mapWidthStart = CameraController.widthInMeters;
			}
			*/
			if (Input.GetMouseButtonUp(0)) {
				isSliding = false;
			} 
			
			if (Input.GetMouseButtonUp(2)) {
				isZooming = false;
			} 

		} else {

			if (Input.touchCount == 0) {
					

				isSliding = false;
				isZooming = false;
			}

			
			if (Input.touchCount >= 2) {
				if (!isZooming) {
					isSliding = false;
					isZooming = true;
					SetTouchesStart();
					zoomDeltaStart = Vector2.Distance(firstTouchStart,secondTouchStart);
					mapWidthStart = CameraController.widthInMeters;
				}
			}

			if (Input.touchCount == 1) {

				if (Input.GetTouch(0).phase == TouchPhase.Began) {
					GameController.OnButtonDown(Input.GetTouch(0).position);
				}
				
				if (Input.GetTouch(0).phase == TouchPhase.Ended) {
					GameController.OnButtonUp(Input.GetTouch(0).position);
					GUIController.OnClick(Input.mousePosition);
				}

				
				if (Input.GetTouch(0).phase != TouchPhase.Stationary) {
					if (!isSliding) {
						isZooming = false;
						isSliding = true;
						SetTouchesStart();
						cameraPositionStart = Camera.main.transform.position;
					}
				}

			}
		}

		SetTouches();


        if (mode == Mode.Slide || mode == Mode.SlideAndZoom) {

		    if (isSliding) {
			    delta = firstTouchStart - firstTouch;

			    if (!SetCameraPositionX(cameraPositionStart.x + delta.x*frictionDelta)) {

				    float oldY = firstTouchStart.y;
				    SetTouchesStart();
				    firstTouchStart = new Vector2 (firstTouchStart.x,oldY); 

				    cameraPositionStart = new Vector3 (Camera.main.transform.position.x,Camera.main.transform.position.y,
				                                       cameraPositionStart.z); 
			    }
			
			    if (!SetCameraPositionY(cameraPositionStart.z + delta.y*frictionDelta)) {
				
				    float oldX = firstTouchStart.x;
				    SetTouchesStart();
				    firstTouchStart = new Vector2 (oldX,firstTouchStart.y); 

				    cameraPositionStart = new Vector3 (cameraPositionStart.x,Camera.main.transform.position.y,
				                                       Camera.main.transform.position.z); 
			    }
		    }
        }

        if (mode == Mode.Zoom || mode == Mode.SlideAndZoom) {

            if (isZooming) {
			    zoomDelta = Vector2.Distance(firstTouch,secondTouch);

			    ResizeCamera((1+(zoomDeltaStart - zoomDelta)*frictionZoom)*mapWidthStart);

		    }
        }
        
        SetCameraPositionX (CameraController.cameraPosition.x);
        SetCameraPositionY (CameraController.cameraPosition.y);

	}


}
