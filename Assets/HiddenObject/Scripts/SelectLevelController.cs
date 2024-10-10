using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectLevelController : MonoBehaviour {


    public static SelectLevelController instance;
	
	private static GUIImage backgroundBeforeLevel;
	private static GUIImage blackBeforeLevel;
	private static GUIImage levelNameBeforeLevel;
	private static GUIImage textBeforeLevel;
	private static GUIImage scoreBeforeLevel;
	private static GUIButton exitBeforeLevel;
	private static GUIButton okBeforeLevel;
	private static int starsSum;
	private static GUIImage starsSumText;
	private static GUIImage yourScoreBeforeLevel;

    private static List <GUIObject> objs;


    public void CreateLevelButton (int i, int q) {
        var button = new GUIButton ();
		button.texture =ResourcesController.LoadCompressedTexture ("Interface/SelectLevel/"+ (i+1));
		button.layer = -7;
		button.positionInMeters = new Vector2 ((218 + i*99 + 72/2f - 600) / 50f, -(168 + q*182 + 71/2f - 337) / 50f);
		button.sizeInMeters = new Vector2 (72 / 50f, 71 / 50f);
		button.OnClick = () => {
						
            AudioController.instance.CreateAudio ("click");
			Settings.level = i;
			Settings.levelPack = q;
			CreateBeforeLevel ();
		};

		button.OnButtonDown = () => {
						
			button.texture =ResourcesController.LoadCompressedTexture ("Interface/SelectLevel/"+ (i+1)+"Pushed");
		};

		button.OnButtonUp = () => {
						
			button.texture =ResourcesController.LoadCompressedTexture ("Interface/SelectLevel/"+ (i+1));
		};

        objs.Add (button);
    }

    public void CreateUnlockMenu (GameController.Action action) {

        var back = new GUIImage ();
		back.texture =ResourcesController.LoadCompressedTexture ("Interface/Billing/Background");
		back.layer = -6;
		back.positionInMeters = new Vector2 (0,0);
		back.sizeInMeters = new Vector2 (-1, -1);
		back.OnClick = action;

		var text = new GUIImage ();
		text.layer = -5;
		text.sizeInMeters = new Vector2 (0,0);
		text.textTransform = GUIController.CreateText (ref text.textObject);
        text.isCentreText = true;
		text.deltaTextPosition = new Vector2 (0, 0);
		text.textScale = new Vector2 (0.1f, 0.1f);
		text.positionInMeters = new Vector2 (0,2.06f);
		text.text = Settings.TranslateText (Settings.language == Settings.Language.English?"UNLOCK THIS LEVEL":"ОТКРЫТЬ ЭТОТ УРОВЕНЬ");

        var unlock = new GUIButton();
        var cancel = new GUIButton ();

		unlock.texture =ResourcesController.LoadCompressedTexture ("$" + Settings.language + "/Interface/Billing/Unlock");
		unlock.layer = -5;
		unlock.positionInMeters = new Vector2 (-2.44f, -1.7f);
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
		cancel.layer = -5;
		cancel.positionInMeters = new Vector2 (2.44f, -1.7f);
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


	public void Start () {


        instance = this;

		GameController.isGame = false;
		AnimationBox.LoadAnimations();
		AnimationController.Create();
		GamePullController.Create();
		GUIController.Create ();
		SlideController.Create (0,0,SlideController.Mode.ReadOnly);
		
		CameraController.ResizeCamera(CameraController.GetWidthInMeters(GameController.mapHeight));
		
        if (objs != null) {

            foreach (var ob in objs) {

                ob.Destroy ();
            } 
        }

		objs = new List<GUIObject> ();

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
		
		var background = new GUIButton ();
		background.texture =ResourcesController.LoadCompressedTexture ("Interface/SelectLevel/Background");
		background.layer = -8;
		background.positionInMeters = new Vector2 (0, 0);
		background.sizeInMeters = new Vector2 (1200 / 50f, 675 / 50f);


		var nameTexture = (ResourcesController.LoadCompressedTexture ("$" + Settings.language + "/" + "Interface/SelectLevel/" + Settings.location));
		var name = new GUIButton ();
		name.texture = nameTexture;
		name.layer = -7;
		name.positionInMeters = new Vector2 (0, -(65 +7 - 337) / 50f);
		name.sizeInMeters = new Vector2 (nameTexture.width / 50f, nameTexture.height / 50f);
		
		
		var levelPack1Texture = (ResourcesController.LoadCompressedTexture ("$" + Settings.language + "/" + "Interface/SelectLevel/LevelPack1" + Settings.location));
		var levelPack1 = new GUIButton ();
		levelPack1.texture = levelPack1Texture;
		levelPack1.layer = -7;
		levelPack1.positionInMeters = new Vector2 (0, -(141 +7 - 337) / 50f);
		levelPack1.sizeInMeters = new Vector2 (levelPack1Texture.width / 50f, levelPack1Texture.height / 50f);
		
		var levelPack2Texture = (ResourcesController.LoadCompressedTexture ("$" + Settings.language + "/" + "Interface/SelectLevel/LevelPack2" + Settings.location));
		var levelPack2 = new GUIButton ();
		levelPack2.texture = levelPack2Texture;
		levelPack2.layer = -7;
		levelPack2.positionInMeters = new Vector2 (0, -(324 +7 - 337) / 50f);
		levelPack2.sizeInMeters = new Vector2 (levelPack2Texture.width / 50f, levelPack2Texture.height / 50f);
		
		var levelPack3Texture = (ResourcesController.LoadCompressedTexture ("$" + Settings.language + "/" + "Interface/SelectLevel/LevelPack3" + Settings.location));
		var levelPack3 = new GUIButton ();
		levelPack3.texture = levelPack3Texture;
		levelPack3.layer = -7;
		levelPack3.positionInMeters = new Vector2 (0, -(505 +7 - 337) / 50f);
		levelPack3.sizeInMeters = new Vector2 (levelPack3Texture.width / 50f, levelPack3Texture.height / 50f);

		int lastLevelUnlocked = -1;

		starsSum = 0;
		int levelOpened = 0;


		for (int q = 0; q < 3; q++)
			for (int i = 0; i < 8; i++) {
				if (lastLevelUnlocked + 1 >= q*8 + i || i == 0 || PlayerPrefs.GetInt ("opened"+Settings.location.ToString()+"Pack"+q) > 0 || Settings.isOpenedAllLevelsAndLocations) {
					
                    CreateLevelButton (i, q);
				} else {
                   /* int level = i;
                    int levelPack = q;
					var button = new GUIButton ();
					button.texture =ResourcesController.LoadCompressedTexture ("Interface/Invisible");
					button.layer = -7;
					button.positionInMeters = new Vector2 ((218 + i*99 + 72/2f - 600) / 50f, -(168 + q*182 + 71/2f - 337) / 50f);
					button.sizeInMeters = new Vector2 (72 / 50f, 71 / 50f);
					button.OnClick = () => {
                        
                        //You can add unlcok iab here
					};
                    objs.Add (button);
                    */
                }
				starsSum += PlayerPrefs.GetInt ("stars"+Settings.location.ToString()+"Pack"+q+"Level"+i);

				if (PlayerPrefs.GetInt ("stars"+Settings.location.ToString()+"Pack"+q+"Level"+i) > 0) {

					lastLevelUnlocked = q*8 + i;
					levelOpened++;
					var startLeft = new GUIImage ();
					startLeft.texture =ResourcesController.LoadCompressedTexture ("Interface/SelectLevel/StarLeft");
					startLeft.layer = -7;
					startLeft.positionInMeters = new Vector2 ((222 + i*99 + 21/2f - 600) / 50f, -(251 + q*182 + 23/2f - 337) / 50f);
					startLeft.sizeInMeters = new Vector2 (21 / 50f, 23 / 50f);
				}

				if (PlayerPrefs.GetInt ("stars"+Settings.location.ToString()+"Pack"+q+"Level"+i) > 1) {

					var startCentre = new GUIImage ();
					startCentre.texture =ResourcesController.LoadCompressedTexture ("Interface/SelectLevel/StarCentre");
					startCentre.layer = -7;
					startCentre.positionInMeters = new Vector2 ((243 + i*99 + 21/2f - 600) / 50f, -(239 + q*182 + 23/2f - 337) / 50f);
					startCentre.sizeInMeters = new Vector2 (21 / 50f, 23 / 50f);
				}

				if (PlayerPrefs.GetInt ("stars"+Settings.location.ToString()+"Pack"+q+"Level"+i) > 2) {

					var startRight = new GUIImage ();
					startRight.texture =ResourcesController.LoadCompressedTexture ("Interface/SelectLevel/StarRight");
					startRight.layer = -7;
					startRight.positionInMeters = new Vector2 ((265 + i*99 + 21/2f - 600) / 50f, -(251 + q*182 + 23/2f - 337) / 50f);
					startRight.sizeInMeters = new Vector2 (21 / 50f, 23 / 50f);
				}
		}

		if (levelOpened == 24) {
			
			PlayerPrefs.SetInt ("opened"+((Settings.Location)((int)Settings.location + 1)).ToString (), 1);
            
            GooglePlayServicesController.ReportProgress ("Pass1Location", 100);

            if (Settings.location == Settings.Location.Scara) {

                GooglePlayServicesController.ReportProgress ("CompleteAllLevels", 100);
            }
		}
		
		starsSumText = new GUIImage ();
		starsSumText.layer = -7;
		starsSumText.sizeInMeters = new Vector2 (0,0);
		starsSumText.textTransform = GUIController.CreateText (ref starsSumText.textObject);
		starsSumText.deltaTextPosition = new Vector2 (0, 0);
		starsSumText.textScale = new Vector2 (0.14f, 0.14f);
		starsSumText.text = starsSum.ToString ();
		starsSumText.positionInMeters = new Vector2 ((70 - 600) / 50f, -(38 - 337) / 50f);
		
		CameraController.ResizeCamera( Mathf.Min(CameraController.GetWidthInMeters(675 / 50f), 1200 / 50f) );
		GUIController.CreateSettings (true, () => {

			ScenePassageController.instance.LoadScene ("SelectLocationScene");
		}, -4);


        AudioController.instance.UnPauseAudio (MainMenuController.musicTheme);
		ScenePassageController.OnSceneLoaded ();
	}

	
	private void CreateBeforeLevel () {

        AudioController.instance.CreateAudio ("svitok");

		blackBeforeLevel = new GUIImage ();
		blackBeforeLevel.texture =ResourcesController.LoadCompressedTexture ("Interface/Black");
		blackBeforeLevel.layer = -2;
		blackBeforeLevel.positionInMeters = new Vector2 (0, 0);
		blackBeforeLevel.sizeInMeters = new Vector2 (1200 / 50f, 1200 / 50f);

		backgroundBeforeLevel = new GUIImage ();
		backgroundBeforeLevel.texture =ResourcesController.LoadCompressedTexture ("Interface/BeforeLevel/Background");
		backgroundBeforeLevel.layer = -1f;
		backgroundBeforeLevel.positionInMeters = new Vector2 ((746/2f - 746/2f) / 50f, -(668/2f  - 668/2f) / 50f);
		backgroundBeforeLevel.sizeInMeters = new Vector2 (746 / 50f, 668 / 50f);

		levelNameBeforeLevel = new GUIImage ();
		levelNameBeforeLevel.layer = 0f;
		levelNameBeforeLevel.sizeInMeters = new Vector2 (0,0);
		levelNameBeforeLevel.textTransform = GUIController.CreateText (ref levelNameBeforeLevel.textObject);
		levelNameBeforeLevel.deltaTextPosition = new Vector2 (0, 0);
		levelNameBeforeLevel.textScale = new Vector2 (0.08f, 0.08f);
		levelNameBeforeLevel.isCentreText = true;
		levelNameBeforeLevel.positionInMeters = new Vector2 ((350 - 746/2f) / 50f, -(145  - 668/2f) / 50f);
		levelNameBeforeLevel.text = Settings.GetBeforeLevelLevelName ()+ " "+ (Settings.level+1);

		
		textBeforeLevel = new GUIImage ();
		textBeforeLevel.layer = 0f;
		textBeforeLevel.sizeInMeters = new Vector2 (0,0);
		textBeforeLevel.textTransform = GUIController.CreateText (ref textBeforeLevel.textObject);
		textBeforeLevel.deltaTextPosition = new Vector2 (0, 0);
		textBeforeLevel.textScale = new Vector2 (0.1f, 0.1f);
		textBeforeLevel.positionInMeters = new Vector2 (-5.71f, -(225  - 668/2f) / 50f);
		textBeforeLevel.text = Settings.GetBeforeLevelText ();
        
		scoreBeforeLevel = new GUIImage ();
		scoreBeforeLevel.layer = 0f;
		scoreBeforeLevel.sizeInMeters = new Vector2 (0,0);
		scoreBeforeLevel.textTransform = GUIController.CreateText (ref scoreBeforeLevel.textObject);
		scoreBeforeLevel.deltaTextPosition = new Vector2 (0, 0);
		scoreBeforeLevel.textScale = new Vector2 (0.1f, 0.1f);
		scoreBeforeLevel.positionInMeters = new Vector2 ((419 - 746/2f) / 50f, -(465  - 668/2f) / 50f);
		scoreBeforeLevel.text = PlayerPrefs.GetInt ("score"+Settings.location.ToString ()+"Pack"+Settings.levelPack.ToString ()+"Level"+Settings.level.ToString ()).ToString ();

		yourScoreBeforeLevel = new GUIImage ();
		yourScoreBeforeLevel.layer = 0f;
		yourScoreBeforeLevel.sizeInMeters = new Vector2 (0,0);
		yourScoreBeforeLevel.textTransform = GUIController.CreateText (ref yourScoreBeforeLevel.textObject);
		yourScoreBeforeLevel.deltaTextPosition = new Vector2 (0, 0);
		yourScoreBeforeLevel.textScale = new Vector2 (0.1f, 0.1f);
		yourScoreBeforeLevel.positionInMeters = new Vector2 ((99 - 746/2f) / 50f, -(465  - 668/2f) / 50f);
		yourScoreBeforeLevel.text = (Settings.language == Settings.Language.Russian?Settings.TranslateText ("Ваш рекорд:"):"High score:");

		exitBeforeLevel = new GUIButton ();
		exitBeforeLevel.texture =ResourcesController.LoadCompressedTexture ("Interface/LevelCompleted/Exit");
		exitBeforeLevel.layer = -0.5f;
		exitBeforeLevel.positionInMeters = new Vector2 ((681 - 746/2f) / 50f, -(67  - 668/2f) / 50f);
		exitBeforeLevel.sizeInMeters = new Vector2 (64 / 50f, 50 / 50f);
		exitBeforeLevel.OnClick = () => {
			
            AudioController.instance.CreateAudio ("click");
			RemoveBeforeLevel ();
		};
		exitBeforeLevel.OnButtonDown = () => {
			
			exitBeforeLevel.texture =ResourcesController.LoadCompressedTexture ("Interface/LevelCompleted/ExitSelected");
		};
		exitBeforeLevel.OnButtonUp = () => {
			
			exitBeforeLevel.texture =ResourcesController.LoadCompressedTexture ("Interface/LevelCompleted/Exit");
		};

		var okBeforeLevelTexture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/BeforeLevel/Play");
		okBeforeLevel = new GUIButton ();
		okBeforeLevel.texture = okBeforeLevelTexture;
		okBeforeLevel.layer = -0.5f;
		okBeforeLevel.positionInMeters = new Vector2 ((746/2f - 746/2f) / 50f, -(584  - 668/2f) / 50f);
		okBeforeLevel.sizeInMeters = new Vector2 (okBeforeLevelTexture.width / 50f, okBeforeLevelTexture.height / 50f);
		okBeforeLevel.OnClick = () => {
            
            AudioController.instance.CreateAudio ("click");
			ScenePassageController.instance.LoadScene ("GameScene");
		};
		okBeforeLevel.OnButtonDown = () => {
			
			okBeforeLevel.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/BeforeLevel/PlayPushed");
		};
		okBeforeLevel.OnButtonUp = () => {
			
			okBeforeLevel.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/BeforeLevel/Play");
		};
	}

	private void RemoveBeforeLevel () {

		blackBeforeLevel.Destroy ();
		backgroundBeforeLevel.Destroy ();
		levelNameBeforeLevel.Destroy ();
		textBeforeLevel.Destroy ();
		scoreBeforeLevel.Destroy ();
		exitBeforeLevel.Destroy ();
		okBeforeLevel.Destroy ();
        yourScoreBeforeLevel.Destroy ();
	}



	void FixedUpdate () {
		
		GUIController.Update (Time.fixedDeltaTime);
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
			if (Input.GetKeyDown(KeyCode.Escape))
					ScenePassageController.instance.LoadScene ("SelectLocationScene");

		}
		
		
	}
}
