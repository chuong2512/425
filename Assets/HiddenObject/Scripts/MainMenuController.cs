using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour
{

	[SerializeField] private GameObject purchasePanel;
	private static float startButtonScaleProgress;
	private static bool isStartButtonGrowing;
	private static GUIButton startButton;
	
	private static GUIImage backgroundExitConfirm;
	private static GUIButton yesExitConfirm;
	private static GUIButton noExitConfirm;
	private static GUIImage blackResults;

    public static GameObject musicTheme;

    private static GUIImage authorsBackground;
    private static GUIButton authorsExit;

	private static int changes = 0;

	void Start () {

        //LevelsHolder.LoadLevelsToOneFile ();

        new GooglePlayServicesController (false, new Dictionary<string, string> () {

            {"Pass1Level", "CgkI_fuplYIDEAIQAA"}
            , {"Pass1Location", "CgkI_fuplYIDEAIQAQ"}
            , {"CompleteAllLevels", "CgkI_fuplYIDEAIQAg"}
            , {"MuteMusic", "CgkI_fuplYIDEAIQAw"}
            , {"OpenSettings", "CgkI_fuplYIDEAIQBA"}
        }, () => {

        });

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
		changes = 0;
		GameController.isGame = false;

		AnimationBox.LoadAnimations();
		AnimationController.Create();
		GamePullController.Create();
		GUIController.Create ();
        SlideController.Create (0,0,SlideController.Mode.ReadOnly);
        new AdsController ();

        new TextureCompressor ("");

		CameraController.ResizeCamera(1200 / 50f);
		
		
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
        
		var background = new GUIImage ();
		background.texture =ResourcesController.LoadCompressedTexture ("Interface/StartMenuBackground");
		background.layer = -4;
		background.positionInMeters = new Vector2 (0, 0);
		background.sizeInMeters = new Vector2 (1200 / 50f, 1200 / 50f);
		
		startButton = new GUIButton ();
		startButton.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/StartButton");
		startButton.layer = -3;
		startButton.positionInMeters = new Vector2 (0, -120 / 50f);
		startButton.sizeInMeters = new Vector2 (546 / 50f, 308 / 50f) * (1 + (startButtonScaleProgress - 0.5f) * 0.02f);
		startButton.OnButtonDown = () => {
			
			startButton.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/StartButtonPressed");
		};
		startButton.OnButtonUp = () => {
			
			//startButton.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/StartButton");
		};
		startButton.OnClick = () =>
		{
			if (purchasePanel.activeInHierarchy) return;
            AudioController.instance.CreateAudio ("click");
			ScenePassageController.instance.LoadScene ("SelectLocationScene");
		};


        /*var autorsButton = new GUIButton ("$"+Settings.language+"/"+"Interface/Authors" + "___COMPRESSED", null, null, 10 + 222 / 2f, 10 + 95 / 2f, 222, 95, -3, true);
		autorsButton.OnButtonDown = () => {
			
			autorsButton.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/AuthorsPressed");
		};
		autorsButton.OnButtonUp = () => {
			
			autorsButton.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/Authors");
		};
		autorsButton.OnClick = () => {
            
            CreateAuthors ();
		};*/

        /*var achievments = new GUIButton ("Achievement", 10 + 130 / 2f, null, null, 10 + 130 / 2f, 130, 130, -3, true);
		achievments.OnClick = () => {
            
            GooglePlayServicesController.ShowAchievments ();
		};*/

		startButtonScaleProgress = 1;
		isStartButtonGrowing = true;
		
		
		var enFlag = new GUIButton ();
		var ruFlag = new GUIButton ();
		
		enFlag.texture =ResourcesController.LoadCompressedTexture ("Interface/FlagEn" + (Settings.language == Settings.Language.English?"Selected":""));
		enFlag.layer = -3f;
		enFlag.positionInMeters = new Vector2 ((100 - 1200/2f) / 50f, -(51  - 675/2f) / 50f);
		enFlag.sizeInMeters = new Vector2 (80 / 50f, 53 / 50f);
		enFlag.OnClick = () => {

			if (Settings.language == Settings.Language.English)
				return;

            AudioController.instance.CreateAudio ("click");

			changes ++;
			Settings.language =  Settings.Language.English;
			startButton.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/StartButton");
			enFlag.texture =ResourcesController.LoadCompressedTexture ("Interface/FlagEn" + (Settings.language == Settings.Language.English?"Selected":""));
			ruFlag.texture =ResourcesController.LoadCompressedTexture ("Interface/FlagRu" + (Settings.language == Settings.Language.Russian?"Selected":""));
		};
		
		ruFlag.texture =ResourcesController.LoadCompressedTexture ("Interface/FlagRu" + (Settings.language == Settings.Language.Russian?"Selected":""));
		ruFlag.layer = -3f;
		ruFlag.positionInMeters = new Vector2 ((200 - 1200/2f) / 50f, -(51  - 675/2f) / 50f);
		ruFlag.sizeInMeters = new Vector2 (80 / 50f, 53 / 50f);
		ruFlag.OnClick = () => {
			
			if (Settings.language == Settings.Language.Russian)
				return;

            AudioController.instance.CreateAudio ("click");
			
			changes ++;
			Settings.language =  Settings.Language.Russian;
			startButton.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/StartButton");
			enFlag.texture =ResourcesController.LoadCompressedTexture ("Interface/FlagEn" + (Settings.language == Settings.Language.English?"Selected":""));
			ruFlag.texture =ResourcesController.LoadCompressedTexture ("Interface/FlagRu" + (Settings.language == Settings.Language.Russian?"Selected":""));
		};

		GUIController.CreateSettings (true, () => {
			
			CreateExitConfirmMenu ();
		}, -3f);
		

		ScenePassageController.OnSceneLoaded ();
	}


	private static void CreateExitConfirmMenu () {
		
		backgroundExitConfirm = new GUIImage ();
		backgroundExitConfirm.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/ExitConfirm/Background");
		backgroundExitConfirm.layer = -2;
		backgroundExitConfirm.positionInMeters = new Vector2 (0, 0);
		backgroundExitConfirm.sizeInMeters = new Vector2 (513 / 50f, 346 / 50f);

		blackResults = new GUIImage ();
		blackResults.texture =ResourcesController.LoadCompressedTexture ("Interface/Black");
		blackResults.layer = -2.5f;
		blackResults.positionInMeters = new Vector2 (0, 0);
		blackResults.sizeInMeters = new Vector2 (1200 / 50f, 1200 / 50f);

		yesExitConfirm  = new GUIButton ();
		yesExitConfirm.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/ExitConfirm/Yes");
		yesExitConfirm.layer = 0;
		yesExitConfirm.positionInMeters = new Vector2 ((131 - 513 / 2f) / 50f, -(220 - 346 / 2f) / 50f);
		yesExitConfirm.sizeInMeters = new Vector2 (238 / 50f, 207 / 50f);
		yesExitConfirm.OnButtonDown = () => {
			
			yesExitConfirm.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/ExitConfirm/YesPushed");
		};
		yesExitConfirm.OnButtonUp = () => {
			
			//yesExitConfirm.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/StartButton");
		};
		yesExitConfirm.OnClick = () => {
			
            AudioController.instance.CreateAudio ("click");
			Application.Quit ();
		};

		noExitConfirm  = new GUIButton ();
		noExitConfirm.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/ExitConfirm/No");
		noExitConfirm.layer = 0;
		noExitConfirm.positionInMeters = new Vector2 ((390 - 513 / 2f) / 50f, -(220 - 346 / 2f) / 50f);
		noExitConfirm.sizeInMeters = new Vector2 (238 / 50f, 207 / 50f);
		noExitConfirm.OnButtonDown = () => {
			
			noExitConfirm.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/ExitConfirm/NoPushed");
		};
		noExitConfirm.OnButtonUp = () => {
			
			//yesExitConfirm.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/StartButton");
		};
		noExitConfirm.OnClick = () => {
			
            AudioController.instance.CreateAudio ("click");
			RemoveExitConfirmMenu ();
		};

	}

	private static void RemoveExitConfirmMenu () {

		backgroundExitConfirm.Destroy ();
		yesExitConfirm.Destroy ();
		noExitConfirm.Destroy ();
		blackResults.Destroy ();
	}

    private static void CreateAuthors () {

        AudioController.instance.CreateAudio ("click");
        
        if (authorsExit != null)
            return;

        authorsBackground = new GUIImage ();
		authorsBackground.texture = ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/Authors/Background");
		authorsBackground.layer = -2;
		authorsBackground.positionInMeters = new Vector2 (0, 0);
		authorsBackground.sizeInMeters = new Vector2 (-1, -1);

        authorsExit = new GUIButton ();
		authorsExit.texture =ResourcesController.LoadCompressedTexture ("Interface/LevelCompleted/Exit");
		authorsExit.layer = -1;
		authorsExit.positionInMeters = new Vector2 (5.28f, 4.53f);
		authorsExit.sizeInMeters = new Vector2 (-1, -1);
        authorsExit.OnButtonDown = () => {
			
			authorsExit.texture =ResourcesController.LoadCompressedTexture ("Interface/LevelCompleted/ExitSelected");
		};
		authorsExit.OnButtonUp = () => {
			
			authorsExit.texture =ResourcesController.LoadCompressedTexture ("Interface/LevelCompleted/Exit");
		};
		authorsExit.OnClick = () => {
            
            AudioController.instance.CreateAudio ("click");
            RemoveAuthors ();
		};
    }

    private static void RemoveAuthors () {
            
        if (authorsExit == null)
            return;        

        authorsExit.Destroy ();
        authorsBackground.Destroy ();

        authorsExit = null;
    }


	void FixedUpdate () {
        /*
		if (changes == 10) {

			changes = 0;
			
			PlayerPrefs.SetInt ("opened"+Settings.Location.EnchantedForest.ToString(),0);
			PlayerPrefs.SetInt ("opened"+Settings.Location.LonelyHills.ToString(),0);
			PlayerPrefs.SetInt ("opened"+Settings.Location.Scara.ToString(),0);

			for (int q = 0; q < 3; q++)
			for (int i = 0; i < 8; i++) {
				
				PlayerPrefs.SetInt ("stars"+Settings.Location.EnchantedForest.ToString()+"Pack"+q+"Level"+i,0);
				PlayerPrefs.SetInt ("stars"+Settings.Location.LonelyHills.ToString()+"Pack"+q+"Level"+i,0);
				PlayerPrefs.SetInt ("stars"+Settings.Location.Scara.ToString()+"Pack"+q+"Level"+i,0);

				PlayerPrefs.SetInt ("score"+Settings.Location.EnchantedForest.ToString()+"Pack"+q+"Level"+i,0);
				PlayerPrefs.SetInt ("score"+Settings.Location.LonelyHills.ToString()+"Pack"+q+"Level"+i,0);
				PlayerPrefs.SetInt ("score"+Settings.Location.Scara.ToString()+"Pack"+q+"Level"+i,0);
			}
		}

        
		if (changes == 5) {

			changes ++;
			
			PlayerPrefs.SetInt ("opened"+Settings.Location.EnchantedForest.ToString(),1);
			PlayerPrefs.SetInt ("opened"+Settings.Location.LonelyHills.ToString(),1);
			PlayerPrefs.SetInt ("opened"+Settings.Location.Scara.ToString(),1);

			for (int q = 0; q < 3; q++)
			for (int i = 0; i < 8; i++) {
				
				PlayerPrefs.SetInt ("stars"+Settings.Location.EnchantedForest.ToString()+"Pack"+q+"Level"+i,1);
				PlayerPrefs.SetInt ("stars"+Settings.Location.LonelyHills.ToString()+"Pack"+q+"Level"+i,1);
				PlayerPrefs.SetInt ("stars"+Settings.Location.Scara.ToString()+"Pack"+q+"Level"+i,1);

				PlayerPrefs.SetInt ("score"+Settings.Location.EnchantedForest.ToString()+"Pack"+q+"Level"+i,1);
				PlayerPrefs.SetInt ("score"+Settings.Location.LonelyHills.ToString()+"Pack"+q+"Level"+i,1);
				PlayerPrefs.SetInt ("score"+Settings.Location.Scara.ToString()+"Pack"+q+"Level"+i,1);
			}
		}

        
		if (changes == 8) {

			Settings.maxItemsToFind = (Settings.maxItemsToFind == 15? 8 : 15);
		}
        */
		GUIController.Update (Time.fixedDeltaTime);
	}

	void Update () {

		if (startButtonScaleProgress >= 1 || startButtonScaleProgress <= 0)
			isStartButtonGrowing = !isStartButtonGrowing;

		if ( (startButtonScaleProgress > 0 && !isStartButtonGrowing) ||  (startButtonScaleProgress < 1 && isStartButtonGrowing) ) {

			startButtonScaleProgress += (isStartButtonGrowing?1:-1) * Time.deltaTime;
		}


		startButton.sizeInMeters = new Vector2 (546 / 50f, 308 / 50f) * (1 + (startButtonScaleProgress - 0.5f) * 0.02f);


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
				Application.Quit();
		}

		
	}
}
