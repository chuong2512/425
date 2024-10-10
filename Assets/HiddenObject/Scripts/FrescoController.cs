using UnityEngine;
using System.Collections;

public class FrescoController : MonoBehaviour {


    private static GameObject music;

    private static GUIButton continueFresco;
    private static Vector2 continueFrescoStartSize;

	void Start () {

		int frescoIndex = (Settings.frescoIndex == -1?((int) Settings.location) / 2 + 1:Settings.frescoIndex);

		GameController.isGame = false;
		AnimationBox.LoadAnimations();
		AnimationController.Create();
		GamePullController.Create();
		GUIController.Create ();
		SlideController.Create (1200 / 50f, 675 / 50f, SlideController.Mode.Slide);

        AudioController.instance.PauseAudio (MainMenuController.musicTheme);

		
		#if UNITY_IPHONE
		GameController.platform = GameController.Platform.Phone;
		#endif
		
		#if UNITY_ANDROID
		GameController.platform = GameController.Platform.Phone;
		#endif
		
		#if UNITY_STANDALONE_OSX
		GameController.platform = GameController.Platform.PC;
		#endif
		
		#if UNITY_STANDALONE_WIN
		GameController.platform = GameController.Platform.PC;
		#endif
		
		#if UNITY_EDITOR
		GameController.platform = GameController.Platform.PC;
		#endif	

		CameraController.ResizeCamera( Mathf.Min(CameraController.GetWidthInMeters(675 / 50f), 1200 / 50f) );

		
		var background = new GUIImage ();
		background.texture =ResourcesController.LoadCompressedTexture ("Interface/Fresco/Background");
		background.layer = -8;
		background.sizeInMeters = new Vector2 (-1, -1);
		background.positionInMeters = new Vector2 (0, 0);
		
		CameraController.cameraPosition = new Vector2 (background.sizeInMeters.x / 2f - CameraController.widthInMeters / 2f, 0);
		
		var backgroundText = new GUIImage ();
		backgroundText.texture =ResourcesController.LoadCompressedTexture ("$" + Settings.language.ToString () +"/Interface/Fresco/BackgroundText");
		backgroundText.layer = -6;
		backgroundText.sizeInMeters = new Vector2 (-1, -1);
		backgroundText.positionInMeters = new Vector2 ((-1200 / 2f + 2081 / 2f) / 50f, 1.76f);

		var backgroundTextName = new GUIImage ();
		backgroundTextName.texture =ResourcesController.LoadCompressedTexture ("$" + Settings.language.ToString () +"/Interface/Fresco/BackgroundTextName" + frescoIndex);
		backgroundTextName.layer = -5;
		backgroundTextName.sizeInMeters = new Vector2 (-1, -1);
		backgroundTextName.positionInMeters = new Vector2 ((-1200 / 2f + 2081 / 2f) / 50f, 0.22f);
		
		var left = new GUIButton ();
		left.texture =ResourcesController.LoadCompressedTexture ("Interface/Fresco/Left");
		left.layer = -5;
		left.sizeInMeters = new Vector2 (-1, -1);
		left.positionInMeters = new Vector2 (6.54f, 0.22f);
		left.OnClick = () => {
            
            AudioController.instance.CreateAudio ("click");
			if (frescoIndex != 1) {

				Settings.frescoIndex = frescoIndex - 1;
				ScenePassageController.instance.LoadScene ("FrescoScene");
			}
		};
		left.OnButtonDown = () => {
			
			left.texture =ResourcesController.LoadCompressedTexture ("Interface/Fresco/LeftPushed");
		};
		left.OnButtonUp = () => {
			
			left.texture =ResourcesController.LoadCompressedTexture ("Interface/Fresco/Left");
		};

		
		var right = new GUIButton ();
		right.texture =ResourcesController.LoadCompressedTexture ("Interface/Fresco/Right");
		right.layer = -5;
		right.sizeInMeters = new Vector2 (-1, -1);
		right.positionInMeters = new Vector2 (11.08f, 0.22f);
		right.OnClick = () => {
			
            AudioController.instance.CreateAudio ("click");
			if (frescoIndex != 2) {
				Settings.frescoIndex = frescoIndex + 1;
				ScenePassageController.instance.LoadScene ("FrescoScene");
			}
		};
		right.OnButtonDown = () => {
			
			right.texture =ResourcesController.LoadCompressedTexture ("Interface/Fresco/RightPushed");
		};
		right.OnButtonUp = () => {
			
			right.texture =ResourcesController.LoadCompressedTexture ("Interface/Fresco/Right");
		};

		
		var aroundTheWorld = new GUIImage ();
		aroundTheWorld.texture =ResourcesController.LoadCompressedTexture ("Interface/Fresco/AroundTheWorld");
		aroundTheWorld.layer = -7;
		aroundTheWorld.sizeInMeters = new Vector2 (-1, -1);
		aroundTheWorld.positionInMeters = - new Vector2 ((1200 / 2f - 16 ) / 50f, -(675 / 2f - 12) / 50f);
		aroundTheWorld.positionInMeters += new Vector2 (aroundTheWorld.sizeInMeters.x, -aroundTheWorld.sizeInMeters.y) / 2f;

		var fresco = new GUIImage ();
		fresco.texture =ResourcesController.LoadCompressedTexture ("Interface/Fresco/Fresco" + frescoIndex);
		fresco.layer = -7;
		fresco.sizeInMeters = new Vector2 (-1, -1);
		fresco.positionInMeters = aroundTheWorld.positionInMeters;

		var config = Settings.configFresco.Split ('|');


		int toDrop = -1;
					
		if (Settings.isLevelEnded) {

			toDrop = ( (((int) Settings.location) % 2)*3+Settings.levelPack) * 8 + (Settings.level + 1);
		}
        
        if (frescoIndex == 2)
            toDrop *=2;


		Settings.isLevelEnded = false;

		bool isOpened = true;

		for (int i = 2; i + 6 < config.Length; i += 6) {

			toDrop--;
			
			if (PlayerPrefs.GetInt ("fresco"+frescoIndex +"_"+(i / 6)) == 1) 
			    continue;

			isOpened = false;

			var part = new GUIButton ();
			part.texture =ResourcesController.LoadCompressedTexture ("Interface/Fresco/Parts/"+config[i]);
			part.layer = -6;
			part.sizeInMeters = new Vector2 (float.Parse(config[i+1]) / 50f, float.Parse(config[i+2]) /50f) * 964 / 1200f;
			part.positionInMeters = new Vector2 (float.Parse(config[i+3]) / 50f, -float.Parse(config[i+4]) /50f) * 964 / 1200f + new Vector2 (part.sizeInMeters.x,-part.sizeInMeters.y) / 2f;
			part.positionInMeters -= new Vector2 ((1200 / 2f - 16) / 50f, -(675 / 2f - 12) / 50f);
			part.OnClick = () => {

				//StartCoroutine (DropPart (part, false));
			};

			if (toDrop == 0 || (toDrop == -1 && frescoIndex == 2)) {

				StartCoroutine (DropPart (part, true));
                PlayerPrefs.SetInt ("fresco"+frescoIndex +"_"+(i / 6), 1);
			}

		}

		var bottom = new GUIImage ();
		bottom.texture =ResourcesController.LoadCompressedTexture ("$" + Settings.language.ToString () +"/Interface/Fresco/"+(isOpened?"Text" + frescoIndex:"Closed"));
		bottom.layer = -6;
		bottom.sizeInMeters = new Vector2 (-1, -1);
		bottom.positionInMeters = new Vector2 ((-1200 / 2f + 2081 / 2f) / 50f, -2.66f);

		continueFresco = new GUIButton ();
		continueFresco.texture =ResourcesController.LoadCompressedTexture ("$" + Settings.language.ToString () +"/Interface/Fresco/Continue");
		continueFresco.layer = -6;
		continueFresco.sizeInMeters = new Vector2 (-1, -1);
        continueFrescoStartSize = continueFresco.sizeInMeters;
		continueFresco.positionInMeters = new Vector2 ((-1200 / 2f + 2081 / 2f) / 50f, -5.51f);
        continueFresco.OnClick = () => {
            
            AudioController.instance.RemoveAudio (music);
			ScenePassageController.instance.LoadScene ("SelectLevelScene");
        };
        continueFresco.OnButtonDown = () => {

		    continueFresco.texture =ResourcesController.LoadCompressedTexture ("$" + Settings.language.ToString () +"/Interface/Fresco/ContinuePushed");
        };
        continueFresco.OnButtonUp = () => {

		    continueFresco.texture =ResourcesController.LoadCompressedTexture ("$" + Settings.language.ToString () +"/Interface/Fresco/Continue");
        };

        music = AudioController.instance.CreateAudio ("freska", true, true);
        
		GUIController.CreateSettings (true, () => {
			
            AudioController.instance.RemoveAudio (music);
			ScenePassageController.instance.LoadScene ("SelectLevelScene");
		}, -4);

		ScenePassageController.OnSceneLoaded ();
	}


	private IEnumerator DropPart (GUIObject obj, bool isDroping) {

		if (obj.layer != -5) {

            yield return new WaitForSeconds (1f);

            AudioController.instance.CreateAudio ("freska_zvuk");

            yield return new WaitForSeconds (1.4f);

			obj.layer = -5;
			obj.positionInMeters = obj.positionInMeters;
			Vector2 startPos = obj.positionInMeters;

			for (int i = 0; i < 5; i++) {

				Vector2 toAdd = new Vector2 (2 * (Random.value - 0.5f), 2 * (Random.value - 0.5f)) * 0.01f;
				for (int q = 0; q < 10; q++) {

					obj.positionInMeters += toAdd;
					yield return new WaitForSeconds (0.02f);
				}


			}

			if (!isDroping) {

				while (Vector2.Distance (obj.positionInMeters, startPos) > 0.01f) {
			 
					obj.positionInMeters += (startPos - obj.positionInMeters).normalized * 2f * 0.01f;
					yield return new WaitForSeconds (0.01f);
				}
				obj.layer = -6;
				obj.positionInMeters = startPos;

			} else {

				float qs = 0.4f;

				while (obj.positionInMeters.y > - 700 / 50f) {

					qs += 0.01f;
					obj.positionInMeters += new Vector2 (0, -10 * qs * qs - 4) * 0.02f;
					yield return new WaitForSeconds (0.02f);
				}

			}

		}
	}


	
	void FixedUpdate () {
		
		GUIController.Update (Time.fixedDeltaTime);
        continueFresco.sizeInMeters = continueFrescoStartSize * (0.92f + (1 - Mathf.Abs(Mathf.Sin (Time.timeSinceLevelLoad * 0.5f))) * 0.08f);
	}
	
	void Update () {
		
		
		AnimationController.Update(Time.deltaTime);
		
		SlideController.frictionDelta = CameraController.widthInMeters/Screen.width;
		SlideController.SlideControll();
		
		
		if (GameController.platform == GameController.Platform.PC) {
			
			if (Input.GetMouseButtonUp(0)) {
				
				GUIController.OnClick(Input.mousePosition);
				GameController.OnButtonUp(Input.mousePosition);
			}
			
			if (Input.GetMouseButtonDown(0)) 
				GameController.OnButtonDown(Input.mousePosition);
			
		}
		
		if (GameController.platform == GameController.Platform.Phone) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
                
                AudioController.instance.RemoveAudio (music);
			    ScenePassageController.instance.LoadScene ("SelectLevelScene");
            }
		}
		
		
	}
}
