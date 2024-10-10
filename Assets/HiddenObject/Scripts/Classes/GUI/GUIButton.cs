using UnityEngine;
using System.Collections;

public class GUIButton : GUIImage {
	
	private ObjectAnimation animation;
	
	public string OnClickAnimation = "";
	public string OnButtonDownAnimation = "";
	public string OnButtonUpAnimation = "";
	
	public float OnClickAnimationTime = 0.1f;
	public float OnButtonDownAnimationTime = 0.3f;
	public float OnButtonUpAnimationTime = 0.3f;

	private bool isButtonDown;

	public GUIButton () {

		gameObject = GamePullController.CreateButton();
		animation = new ObjectAnimation (gameObject);
		isButtonDown = false;
		GUIController.Add(gameObject,this);

	}

	
	public GUIButton (string _texture,float? leftPos,float? topPos,float? rightPos,float? botPos,float widthInPixels,float heightInPixels, float _layer = 0, bool _useScale = false) {
		
		gameObject = GamePullController.CreateButton();
		animation = new ObjectAnimation (gameObject);

		layer = _layer;
		useScale = _useScale;
		
        if (_texture.Contains ("___COMPRESSED")) {

            _texture = _texture.Replace ("___COMPRESSED", "");
            texture = ResourcesController.LoadCompressedTexture (_texture);
        } else {

            texture = ResourcesController.Load (_texture) as Texture;
        }  

		sizeInPixels = new Vector2 (widthInPixels, heightInPixels);
		
		if (leftPos!=null)
			left = leftPos;
		if (topPos!=null)
			top = topPos;
		
		if (rightPos!=null)
			right = rightPos;
		
		if (botPos!=null)
			bottom = botPos;

		

		isButtonDown = false;
		GUIController.Add(gameObject,this);
		
	}


	public override void Click() {

		if (OnClickAnimation != "") {

			animation.Load(OnClickAnimation,-OnClickAnimationTime);
			animation.Play(-1,OnClick);

		} else {
			if (OnClick != null)
				OnClick();
		}

	}
	
	public override void ButtonDown() {
		
		if (OnButtonDownAnimation != "") {
			
			animation.Load(OnButtonDownAnimation,-OnButtonDownAnimationTime);
			animation.Play(-1,OnButtonDown);
			
		} else {
			if (OnButtonDown!=null)
				OnButtonDown();
		}
		isButtonDown = true;
	}
	
	public override void ButtonUp() {

		if (!isButtonDown)
			return;

		if (OnButtonUpAnimation != "") {
			
			animation.Load(OnButtonUpAnimation,-OnButtonUpAnimationTime);
			animation.Play(-1,OnButtonUp);
			
		} else {
			if (OnButtonUp != null) 
				OnButtonUp();
		}
		isButtonDown = false;
	}

	public override void Destroy() {

		GamePullController.DestroyButton(gameObject);
		GameObject.Destroy (textObject);
		animation.Destroy();
		GUIController.Remove(gameObject,this);
	}


}
