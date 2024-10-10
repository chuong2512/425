using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Div {

	public GameObject gameObject;

	public static Vector2 start;

	private float _layer;
	public float layer {

		get { return _layer; }
		set { 

			_layer = value;
			gameObject.transform.position = new Vector3 (gameObject.transform.position.x, value, gameObject.transform.position.z);
		}
	}

	public Vector2 position {
		
		get { return new Vector2(gameObject.transform.position.x - start.x,-gameObject.transform.position.z - start.y); }
		set {

			gameObject.transform.position = new Vector3 (value.x + start.x,layer,- (value.y + start.y));
		}
			
	}

	
	public Vector2 positionInMeters {
		
		get {return new Vector2(gameObject.transform.position.x,gameObject.transform.position.z); }
		set {
			gameObject.transform.position = new Vector3 (value.x,layer,value.y);
		}
	}

	public Texture texture {
		
		set {gameObject.GetComponent<Renderer>().material.mainTexture = value;} 
		
	}

	
	public Vector2 size {
		
		get {return new Vector2(gameObject.transform.localScale.x,gameObject.transform.localScale.y); }
		set {gameObject.transform.localScale = new Vector3 (value.x,value.y,1);}
		
	}


	public Div (string _texture,float leftPos,float topPos, float widthInPixels,float heightInPixels, float _layer, ref Dictionary <GameObject,Div> toAdd) {

		leftPos /= 50f;
		topPos /= 50f;
		widthInPixels /= 50f;
		heightInPixels /= 50f;
		leftPos += widthInPixels / 2f;
		topPos += heightInPixels / 2f;
		gameObject = GamePullController.CreateImage();
		
		layer = _layer;
		
		texture =ResourcesController.LoadCompressedTexture (_texture);
		size = new Vector2 (widthInPixels, heightInPixels);
		position = new Vector2 (leftPos, topPos);

		toAdd.Add (gameObject, this);

	}


	
	
	public void Destroy(ref Dictionary <GameObject,Div> toAdd) {

		toAdd.Remove (gameObject);
		GamePullController.DestroyImage(gameObject);
	}




}
