using UnityEngine;
using System.Collections;

public class GUIImage : GUIObject {

	private bool isAlpha;
	
	public GUIImage () {

		gameObject = GamePullController.CreateImage();
		GUIController.Add(this);
	}
	
	
	public GUIImage (string _texture,float? leftPos,float? topPos,float? rightPos,float? botPos,float widthInPixels,float heightInPixels, float _layer = 0, bool _useScale = false, bool _isAlpha = false) {

		isAlpha = _isAlpha;

		if (isAlpha) 
			gameObject = GamePullController.CreateImageAlpha ();
		else
			gameObject = GamePullController.CreateImage ();
			

		layer = _layer;
		useScale = _useScale;

        if (_texture.Contains ("___COMPRESSED")) {

            _texture = _texture.Replace ("___COMPRESSED", "");
            texture = ResourcesController.LoadCompressedTexture (_texture);
        } else {

            texture = ResourcesController.LoadIgnore (_texture) as Texture;
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
		
		
		
		GUIController.Add(this);
		
	}
	
	public override void Destroy() {

		GameObject.Destroy (textObject);

		if (isAlpha) 
			GamePullController.DestroyImageAlpha(gameObject);
		else
			GamePullController.DestroyImage(gameObject);
			
		GUIController.Remove(this);
		
	}
	
	public override void Click () {}
	public override void ButtonDown () {}
	public override void ButtonUp () {}

}
