using UnityEngine;
using System.Collections;

public class SelectLocationController : MonoBehaviour {
	
    private Vector2 startArrowPosition;

    private GUIImage selection = null;

    private void CheckOpened () {

        int levelOpened = 0;

        Settings.Location[] loc = {Settings.Location.EnchantedForest
                , Settings.Location.LonelyHills
                , Settings.Location.Scara };

        foreach (var l in loc) {

            levelOpened = 0;

            for (int q = 0; q < 3; q++)
                for (int i = 0; i < 8; i++)
                    if (PlayerPrefs.GetInt ("stars"+l.ToString ()+"Pack"+q+"Level"+i) > 0)
                        levelOpened ++;
        
            if (levelOpened >= 24) {

                PlayerPrefs.SetInt ("opened"+((Settings.Location)((int)l + 1)).ToString (), 1);
            }
            
              // PlayerPrefs.SetInt ("opened"+((Settings.Location)((int)l + 1)).ToString (), 0);
        }
    }

    public void CreateUnlockMenu (GameController.Action action) {

        var back = new GUIImage ();
		back.texture =ResourcesController.LoadCompressedTexture ("Interface/Billing/Background");
		back.layer = -2.5f;
		back.positionInMeters = new Vector2 (0,0) + CameraController.cameraPosition; 
		back.sizeInMeters = new Vector2 (-1, -1);
		back.OnClick = action;

		var text = new GUIImage ();
		text.layer = -1.5f;
		text.sizeInMeters = new Vector2 (0,0);
		text.textTransform = GUIController.CreateText (ref text.textObject);
        text.isCentreText = true;
		text.deltaTextPosition = new Vector2 (0, 0);
		text.textScale = new Vector2 (0.1f, 0.1f);
		text.positionInMeters = new Vector2 (0, 2.06f) + CameraController.cameraPosition;
		text.text = Settings.TranslateText (Settings.language == Settings.Language.English?"UNLOCK THIS LOCATION":"ОТКРЫТЬ ЭТОТ СВИТОК");

        var unlock = new GUIButton();
        var cancel = new GUIButton ();

		unlock.texture =ResourcesController.LoadCompressedTexture ("$" + Settings.language + "/Interface/Billing/Unlock");
		unlock.layer = -1.5f;
		unlock.positionInMeters = new Vector2 (-2.44f, -1.7f) + CameraController.cameraPosition;
		unlock.sizeInMeters = new Vector2 (-1, -1);
		unlock.OnClick = () => {

            action ();

            text.Destroy ();
            back.Destroy ();
            unlock.Destroy ();
            cancel.Destroy ();
        };
        unlock.OnButtonDown = () => {

            unlock.texture =ResourcesController.LoadCompressedTexture ("$" + Settings.language + "/Interface/Billing/UnlockPushed");
        };
        unlock.OnButtonUp = () => {

            unlock.texture =ResourcesController.LoadCompressedTexture ("$" + Settings.language + "/Interface/Billing/Unlock");
        };

		cancel.texture =ResourcesController.LoadCompressedTexture ("$" + Settings.language + "/Interface/Billing/Cancel");
		cancel.layer = -1.5f;
		cancel.positionInMeters = new Vector2 (2.44f, -1.7f) + CameraController.cameraPosition;
		cancel.sizeInMeters = new Vector2 (-1, -1);
		cancel.OnClick = () => {

            text.Destroy ();
            back.Destroy ();
            unlock.Destroy ();
            cancel.Destroy ();
        };
        cancel.OnButtonDown = () => {

            cancel.texture =ResourcesController.LoadCompressedTexture ("$" + Settings.language + "/Interface/Billing/CancelPushed");
        };
        cancel.OnButtonUp = () => {

            cancel.texture =ResourcesController.LoadCompressedTexture ("$" + Settings.language + "/Interface/Billing/Cancel");
        };

    }

	void Start () {

		GameController.isGame = false;
		AnimationBox.LoadAnimations();
		AnimationController.Create();
		GamePullController.Create();
		GUIController.Create ();
		SlideController.Create (1200 / 50f, 1100 / 50f, SlideController.Mode.Slide);
		
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
		
        CheckOpened ();

		var background = new GUIImage ();
		background.texture =ResourcesController.LoadCompressedTexture ("Interface/SelectLocation/Background");
		background.layer = -5;
		background.positionInMeters = new Vector2 (0, 0);
		background.sizeInMeters = new Vector2 (1200 / 50f, 1100 / 50f);

		var enchantedForest = new GUIButton ();
		enchantedForest.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/SelectLocation/EnchantedForest");
		enchantedForest.layer = -4;
		enchantedForest.positionInMeters = new Vector2 ((177 - 600) / 50f, -(179 + 50 - 1100 / 2f) / 50f);
		enchantedForest.sizeInMeters = new Vector2 (375 / 50f, 185 / 50f);
		enchantedForest.OnClick = () => {
            
            AudioController.instance.CreateAudio ("click");
			StopAllCoroutines ();
			Settings.location = Settings.Location.EnchantedForest;
			ScenePassageController.instance.LoadScene ("SelectLevelScene");
		};
        enchantedForest.OnButtonDown = () => {

            selection = new GUIImage ();
		    selection.texture =ResourcesController.LoadCompressedTexture ("Interface/SelectLocation/Selection");
		    selection.layer = -4.5f;
		    selection.positionInMeters = enchantedForest.positionInMeters + new Vector2 (0, 0.24f);
		    selection.sizeInMeters = new Vector2 (-1, -1);
            selection.sizeInMeters *= (375/50f) / selection.sizeInMeters.x;
        };
        enchantedForest.OnButtonUp = () => {

            selection.Destroy ();
        };

		var lonelyHills = new GUIButton ();
		lonelyHills.texture = ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/SelectLocation/LonelyHills"
		                                      + (PlayerPrefs.GetInt ("openedLonelyHills")==1?"":"Blocked"));
		lonelyHills.layer = -4;
		lonelyHills.positionInMeters = new Vector2 ((532 - 600) / 50f, -(215 + 50 - 1100 / 2f) / 50f);
		lonelyHills.sizeInMeters = new Vector2 (375 / 50f, 185 / 50f);
		lonelyHills.OnClick = () => {

			if (PlayerPrefs.GetInt ("openedLonelyHills") != 1 && !Settings.isOpenedAllLevelsAndLocations) {

               return;
            }

            AudioController.instance.CreateAudio ("click");
			StopAllCoroutines ();
			Settings.location = Settings.Location.LonelyHills;
			ScenePassageController.instance.LoadScene ("SelectLevelScene");
		};
        lonelyHills.OnButtonDown = () => {

            selection = new GUIImage ();
		    selection.texture =ResourcesController.LoadCompressedTexture ("Interface/SelectLocation/Selection");
		    selection.layer = -4.5f;
		    selection.positionInMeters = lonelyHills.positionInMeters + new Vector2 (0, 0.24f);
		    selection.sizeInMeters = new Vector2 (-1, -1);
            selection.sizeInMeters *= (375/50f) / selection.sizeInMeters.x;
        };
        lonelyHills.OnButtonUp = () => {

            selection.Destroy ();
        };
		
		var scara = new GUIButton ();
		scara.texture = ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/SelectLocation/Scara"
		                                + (PlayerPrefs.GetInt ("openedScara")==1?"":"Blocked"));
		scara.layer = -4;
		scara.positionInMeters = new Vector2 ((953 - 600) / 50f, -(142 + 50 - 1100 / 2f) / 50f);
		scara.sizeInMeters = new Vector2 (375 / 50f, 185 / 50f);
		scara.OnClick = () => {

			
			if (PlayerPrefs.GetInt ("openedScara") != 1 && !Settings.isOpenedAllLevelsAndLocations) {
               
               return;
            }
            
            AudioController.instance.CreateAudio ("click");
			StopAllCoroutines ();
			Settings.location = Settings.Location.Scara;
			ScenePassageController.instance.LoadScene ("SelectLevelScene");
		};
        scara.OnButtonDown = () => {

            selection = new GUIImage ();
		    selection.texture =ResourcesController.LoadCompressedTexture ("Interface/SelectLocation/Selection");
		    selection.layer = -4.5f;
		    selection.positionInMeters = scara.positionInMeters + new Vector2 (0, 0.24f);
		    selection.sizeInMeters = new Vector2 (-1, -1);
            selection.sizeInMeters *= (375/50f) / selection.sizeInMeters.x;
        };
        scara.OnButtonUp = () => {

            selection.Destroy ();
        };

		var arrow = new GUIButton ();
		arrow.texture =ResourcesController.LoadCompressedTexture ("Interface/SelectLocation/Arrow");
		arrow.layer = -3;
		arrow.positionInMeters = new Vector2 ((177 - 600) / 50f, -(80 + 50 - 1100 / 2f) / 50f);
        
        if (PlayerPrefs.GetInt ("openedLonelyHills") == 1)
		arrow.positionInMeters = new Vector2 ((532 - 600) / 50f, -(215 - 100 + 50 - 1100 / 2f) / 50f);

        if (PlayerPrefs.GetInt ("openedScara") == 1)
		arrow.positionInMeters = new Vector2 ((953 - 600) / 50f, -(142 - 100 + 50 - 1100 / 2f) / 50f);

		arrow.sizeInMeters = new Vector2 (80 / 50f, 80 / 50f);
		
        startArrowPosition = arrow.positionInMeters;


		//CameraController.ResizeCamera( Mathf.Min(CameraController.GetWidthInMeters(675 / 50f), 1200 / 50f) );
		CameraController.ResizeCamera(1200 / 50f);
		CameraController.cameraPosition = new Vector2(0,(1100 / 50f - CameraController.heightInMeters)/2);

		GUIController.CreateSettings (true, () => { 

			ScenePassageController.instance.LoadScene ("MainMenuScene");
		});
		StartCoroutine (MoveArrow (arrow));

		if (Settings.isTutorial) {
			
			Settings.isTutorial = false;
			CreateTutorial ();
		}

		ScenePassageController.OnSceneLoaded ();
	}

	
	private void CreateTutorial () {
		
		var background = new GUIImage ();
		background.texture =ResourcesController.LoadCompressedTexture ("Interface/Tutorial/Background");
		background.layer = -2;
		background.positionInMeters = new Vector2 (0, 0);
		background.sizeInMeters = new Vector2 (1200 / 50f, 1100 / 50f);
		
		var tarto = new GUIImage ();
		tarto.texture =ResourcesController.LoadCompressedTexture ("Interface/Tutorial/Tarto");
		tarto.layer = -1;
		tarto.positionInMeters = new Vector2 ((162 - 600) / 50f, -(503 - 1100 / 2f) / 50f);
		tarto.sizeInMeters = new Vector2 (380 / 50f, 367 / 50f);
		
		var madgard = new GUIImage ();
		madgard.texture =ResourcesController.LoadCompressedTexture ("Interface/Tutorial/Madgard");
		madgard.layer = -1;
		madgard.positionInMeters = new Vector2 ((1012 - 600) / 50f, -(314 - 1100 / 2f) / 50f);
		madgard.sizeInMeters = new Vector2 (380 / 50f, 399 / 50f);
		
		var text = new GUIImage ();
		text.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/Tutorial/Text");
		text.layer = -1;
		text.positionInMeters = new Vector2 ((601 - 600) / 50f, -(151 - 1100 / 2f) / 50f);
		text.sizeInMeters = new Vector2 (472 / 50f, 275 / 50f);

		background.gameObject.GetComponent <BoxCollider> ().enabled = false;
		tarto.gameObject.GetComponent <BoxCollider> ().enabled = false;
		madgard.gameObject.GetComponent <BoxCollider> ().enabled = false;
		text.gameObject.GetComponent <BoxCollider> ().enabled = false;
		
	}


	private IEnumerator MoveArrow (GUIButton arrow) {

		while (true) {
			
			arrow.positionInMeters = startArrowPosition + new Vector2 (0,0.2f*Mathf.Sin(Time.timeSinceLevelLoad*6f));
			yield return new WaitForSeconds (0.01f);
		}

	}

	void FixedUpdate () {

		GUIController.Update (Time.fixedDeltaTime);
	}
	
	void Update () {
		
		/*
		if (startButtonScaleProgress >= 1 || startButtonScaleProgress <= 0)
			isStartButtonGrowing = !isStartButtonGrowing;
		
		if ( (startButtonScaleProgress > 0 && !isStartButtonGrowing) ||  (startButtonScaleProgress < 1 && isStartButtonGrowing) ) {
			
			startButtonScaleProgress += (isStartButtonGrowing?1:-1) * Time.deltaTime;
		}
		*/

		
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
			if (Input.GetKeyDown(KeyCode.Escape))
				ScenePassageController.instance.LoadScene ("MainMenuScene");

		}
		
		
	}
}
