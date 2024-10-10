using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class GUIObject {

	public GameObject gameObject;

	public bool useScale;

	public bool isCentreText = false;
	public bool isCentreTextInPixels = false;

	private  float? _left = null;
	private float? _top = null;
	private float? _bottom = null;
	private float? _right = null;

	public float layer = 0;

	
	public Texture texture {

		get {return gameObject.GetComponent<Renderer>().material.mainTexture;}
		set {gameObject.GetComponent<Renderer>().material.mainTexture = value;} 
		
	}

	public float? left {
		
		get { return _left;}
		set {
			_left = value * (useScale?GUIController.width/GUIController.GUIBackgroundWidth:1);
			_right = null;
			SetPosition();
		}
	}
	
	public float? right {
		
		get { return _right;}
		set {
			_right = value * (useScale?GUIController.width/GUIController.GUIBackgroundWidth:1);
			_left = null;
			SetPosition();
		}
	}
	
	public float? top {
		
		get { return _top;}
		set {
			_top = value * (useScale?GUIController.width/GUIController.GUIBackgroundWidth:1);
			_bottom = null;
			SetPosition();
		}
	}
	
	public float? bottom {
		
		get { return _bottom;}
		set {
			_bottom = value * (useScale?GUIController.width/GUIController.GUIBackgroundWidth:1);
			_top = null;
			SetPosition();
		}
	}


	public void SetPosition() {

		float? x = null;
		float? y = null;
		
		if (left != null) {
			x = CameraController.cameraPosition.x - CameraController.widthInMeters/2f + left*CameraController.pixelSize;
		}
		if (right != null) {
			x = CameraController.cameraPosition.x + CameraController.widthInMeters/2f - right*CameraController.pixelSize;
		}
		
		if (top != null) {
			y = CameraController.cameraPosition.y + CameraController.heightInMeters/2f - top*CameraController.pixelSize;
		}
		if (bottom != null) {
			y = CameraController.cameraPosition.y - CameraController.heightInMeters/2f + bottom*CameraController.pixelSize;
		}

		
		if (x != null && y != null) {

			positionInMeters = new Vector2 ((float)x,(float)y);

		}

	}


	private Vector2 _sizeInPixels = new Vector2 (-1000,-1000);
	public Vector2 sizeInPixels {

		get { return _sizeInPixels; }
		set { 

            if (value == new Vector2 (-1, -1)) {

				_sizeInPixels = new Vector2 (texture.width,texture.height) * (useScale?GUIController.width/GUIController.GUIBackgroundWidth:1);
			    SetSize();
				return;
			}
			_sizeInPixels = value * (useScale?GUIController.width/GUIController.GUIBackgroundWidth:1);
			SetSize();
		} 


	}

	public void SetSize() {

		if (sizeInPixels == new Vector2 (-1000, -1000))
			return;

		sizeInMeters = sizeInPixels * CameraController.pixelSize; 
	}

	public void SetText() {

		if (deltaTextPositionInPixels != new Vector2 (-1000,-1000))
			deltaTextPosition = deltaTextPositionInPixels * CameraController.pixelSize; 

		if (textScaleInPixels != new Vector2 (-1000,-1000))
			textScale = textScaleInPixels * CameraController.pixelSize; 
        

	}

	
	public Transform textTransform = null;
	public TextMesh textObject = null;
	public string text {

		get { return textObject.text; }
		set { 
			textObject.text = value; 
			
	        if (isCentreText) {
				
		        textPosition = positionInMeters + deltaTextPosition - new Vector2 (textObject.GetComponent < Renderer > ().bounds.max.x / 2f
				                                , - (textObject.GetComponent < Renderer > ().bounds.max.z - textObject.GetComponent < Renderer > ().bounds.min.z) / 2f);
	        }
		}
	}

	private Vector2 _deltaTextPositionInPixels = new Vector2 (-1000,-1000);
	public Vector2 deltaTextPositionInPixels {

		get { return _deltaTextPositionInPixels; }
		set {
			_deltaTextPositionInPixels = value * (useScale?GUIController.width/GUIController.GUIBackgroundWidth:1);
			SetText();
		}
	}
	
	private Vector2 _textScaleInPixels  = new Vector2 (-1000,-1000);
	public Vector2 textScaleInPixels {
		
		get { return _textScaleInPixels; }
		set {
			_textScaleInPixels = value * (useScale?GUIController.width/GUIController.GUIBackgroundWidth:1);
			SetText();
		}
	}

	public Vector2 deltaTextPosition;

	public Vector2 textScale {
		get {
			if (textTransform == null)
				return new Vector2 (0,0);
			return new Vector2 (textTransform.localScale.x, textTransform.localScale.y);}
		set {
			if (textTransform == null)
				return;
			textTransform.localScale = new Vector3 (value.x,value.y,1);
		}

	}

	private Vector2 textPosition {
		
		get {

            if (textTransform == null) 
                return new Vector2 (0, 0);

            return new Vector2 (textTransform.position.x, textTransform.position.z);}
		
		set {
			textTransform.position = new Vector3 (value.x,GUIController.layer+layer,value.y);
		}
		
	}

	public Vector2 positionInMeters {
		
		get {return new Vector2(gameObject.transform.position.x,gameObject.transform.position.z); }
		set {
			gameObject.transform.position = new Vector3 (value.x,GUIController.layer+layer,value.y);

			if (textTransform != null) {
				textPosition = positionInMeters + deltaTextPosition;


			}
		
		}
		
	}
	public Vector2 sizeInMeters {
		
		get {
			  if (gameObject == null)
				return new Vector2 ();
			return new Vector2(gameObject.transform.localScale.x,gameObject.transform.localScale.y); }
		set {
			if (gameObject == null)
				return;

			if (value == new Vector2 (-1, -1)) {

				gameObject.transform.localScale = new Vector3 (texture.width / 50f,texture.height / 50f,1);
				return;
			}

			gameObject.transform.localScale = new Vector3 (value.x,value.y,1);
		}
		
	}

	
	public GameController.Action OnClick;
	public GameController.Action OnButtonDown;
	public GameController.Action OnButtonUp;

	public abstract void Click();
	public abstract void ButtonDown();
	public abstract void ButtonUp();

	public abstract void Destroy();

}
