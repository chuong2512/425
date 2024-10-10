using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIController {

	public static float layer = 19;
	public static GameObject canvas;

    public static bool isSettingsClick = false;


	public static float GUIBackgroundWidth = 1200;
	public static float width {
		get { return Screen.width; }
	}

	private static List <GUIObject> objects = new List<GUIObject>();
	private static Dictionary <GameObject,GUIObject> objectsDictionary = new Dictionary <GameObject,GUIObject>();

	private static GUIButton settings;
	private static GUIButton underSettings;
	private static GUIButton underSettingsExit;
	private static GUIButton underSettingsSounds;
	private static GUIButton underSettingsMusic;
	private static float underSettingsSpeed = 5f;

	private static GUIImage flyingText = null;
	private static float stopTime;
	private static GameController.Action onFlyingTextOver;

	public static float underSettingsPosition {
		
		get {
			if (underSettings == null || underSettings.gameObject == null)
				return 0;

			return 1 - underSettings.gameObject.GetComponent<Renderer> ().material.mainTextureOffset.x / (-1f); 
		}
		
		set {
			if (underSettings == null || underSettings.gameObject == null)
				return;

			underSettings.gameObject.GetComponent<Renderer> ().material.mainTextureOffset = new Vector2 (Mathf.Clamp(-1f*(1-value),-1,0),0);

			settings.gameObject.transform.rotation = Quaternion.Euler (new Vector3 (90,90*value,0));
			

			if (value >= 0.28f)
				underSettingsExit.right = (10 + 88/2f)*(1-value) + (88/2f + 283/2f + 68 + 50)*value - 50;
			else
				underSettingsExit.right = (10 + 88/2f);
			
			if (value >= 0.4f)
				underSettingsSounds.right = (10 + 88/2f)*(1-value) + (88/2f + 283/2f + 68 + 50)*value - 115;
			else 
				underSettingsSounds.right = (10 + 88/2f);

			if (value >= 0.7f)
				underSettingsMusic.right = (10 + 88/2f)*(1-value) + (88/2f + 283/2f + 68 + 50)*value - 180;
			else 
				underSettingsMusic.right = (10 + 88/2f);
			
		}
		
	}

	public static bool isUnderSettingsOpening;
    private static GameController.Action onSettingsClose = null;
	
	public static void Create() {

		objects = new List<GUIObject>();
		objectsDictionary = new Dictionary <GameObject,GUIObject>();
		canvas = GameObject.Find ("Canvas");

		if (settings != null) {

			/*settings.Destroy ();
			underSettings.Destroy ();
			underSettingsExit.Destroy ();
			underSettingsSounds.Destroy ();
			underSettingsMusic.Destroy ();
*/
			settings = null;
			underSettings = null;
			underSettingsExit = null;
			underSettingsSounds = null;
			underSettingsMusic = null;
		}

	}

	public static void OnCameraUpdate() {

		foreach (var obj in objects) {

			obj.SetSize();
			obj.SetPosition();
			obj.SetText();
		}

	}


	public static Transform CreateText(ref TextMesh textObject) {

		GameObject text = GameObject.Instantiate (Resources.Load ("Prefabs/GUI/TextMeshObject"+Settings.language)) as GameObject;
		//text.transform.SetParent (canvas.transform, false);
		textObject = text.GetComponent<TextMesh> ();

		return text.transform;
	}
	
	public static void Add(GameObject gameObject, GUIObject guiObject) {

		objects.Add(guiObject);
		objectsDictionary.Add(gameObject,guiObject);
	}
	public static void Add(GUIObject guiObject) {

		objects.Add(guiObject);
	}
	
	public static void Remove(GameObject gameObject, GUIObject guiObject) {
		
		objects.Remove(guiObject);
		objectsDictionary.Remove(gameObject);
	}
	
	public static void Remove(GUIObject guiObject) {
		
		objects.Remove(guiObject);
	}


	
	public static void OnClick(Vector2 position) {

        isSettingsClick = false;

		Ray ray;
		RaycastHit hit;
		
		ray = Camera.main.ScreenPointToRay(position);

		if (GameController.isGame) {

			RaycastHit[] hits;
			hits = Physics.RaycastAll(ray,200);

			float minDistance = 10000;
			int nearestObjectId = -1;

			for (int i = 0; i < hits.Length; i++) {

				hit = hits[i];

				if (!hit.transform.gameObject.name.Contains ("ToFind"))
					continue;

				if (minDistance > Vector3.Distance (hit.point,hit.transform.gameObject.transform.position)) {

					minDistance = Vector3.Distance (hit.point,hit.transform.gameObject.transform.position);
					nearestObjectId = int.Parse (hit.transform.gameObject.name.Split ('X') [1]);
				}
			}

			if (nearestObjectId != -1)
				GameController.instance.OnObjectClick (nearestObjectId);

		}

		if (Physics.Raycast (ray, out hit, 200)) {


			if (hit.transform.gameObject.name.Contains ("GUIButton")) {
				GUIController.OnClick (hit.transform.gameObject);
			}
		} 


        if (!isSettingsClick) {

            if (onSettingsClose != null && isUnderSettingsOpening)
                onSettingsClose ();

            isUnderSettingsOpening = false;
        }
	}

	public static void OnClick (GameObject gameObject) {

		objectsDictionary[gameObject].Click();
	}

	public static void OnButtonDown (GameObject gameObject) {

		objectsDictionary[gameObject].ButtonDown();
		
	}

	public static void OnButtonUp () {
		
		foreach (var button in objectsDictionary) 
			button.Value.ButtonUp ();
		
	}


	public static void CreateSettings (bool isTop, GameController.Action onExit, float deltaLayer = 0, GameController.Action onOpen = null, GameController.Action onClose = null) {

        onSettingsClose = onClose;

		if (isTop) {


			settings = new GUIButton ("Interface/Settings" + "___COMPRESSED", null, 10 + 88 / 2f, 10 + 88 / 2f, null, 88, 88, 0 + deltaLayer, true);
			settings.OnClick = () => {
                
                if (GameController.isGame && GameController.isLevelEnding)
                    return;

                GooglePlayServicesController.ReportProgress ("OpenSettings", 100);

                isSettingsClick = true;
		        AudioController.instance.CreateAudio ("click");
				isUnderSettingsOpening = !isUnderSettingsOpening;

                if (isUnderSettingsOpening) {

                    if (onOpen != null)
                        onOpen ();
                } else {

                    if (onSettingsClose != null)
                        onSettingsClose ();
                }

			};

			underSettings = new GUIButton ("Interface/UnderSettings", null, 10 + 88 / 2f, 88 / 2f + 283 / 2f, null, 283, 111, -1 + deltaLayer, true);

			underSettingsExit = new GUIButton ("Interface/Exit" + "___COMPRESSED", null, 10 + 88 / 2f + 5, 88 / 2f + 10, null, 66, 63, -0.5f + deltaLayer, true);
			underSettingsExit.OnClick = () => {
				
                isSettingsClick = true;
		        AudioController.instance.CreateAudio ("click");
				onExit();
			};
			underSettingsSounds = new GUIButton ("Interface/Sounds" + "___COMPRESSED", null, 10 + 88 / 2f + 5, 88 / 2f + 10, null, 64, 59, -0.5f + deltaLayer, true);
            underSettingsSounds.OnClick = () => {

                isSettingsClick = true;
            };

			underSettingsSounds.OnClick = () => {
				
                isSettingsClick = true;
		        AudioController.instance.CreateAudio ("click");
				Settings.sounds = !Settings.sounds;
				
				if (Settings.sounds)
				underSettingsSounds.texture =ResourcesController.LoadCompressedTexture ("Interface/Sounds");
				else
				underSettingsSounds.texture =ResourcesController.LoadCompressedTexture ("Interface/SoundsOff");
				
			};
			underSettingsMusic = new GUIButton ("Interface/Music" + "___COMPRESSED", null, 10 + 88 / 2f + 5, 88 / 2f + 10, null, 58, 57, -0.5f + deltaLayer, true);
			underSettingsMusic.OnClick = () => {
				
                isSettingsClick = true;
		        AudioController.instance.CreateAudio ("click");
				Settings.music = !Settings.music;
				
                GooglePlayServicesController.ReportProgress ("MuteMusic", 100);

				if (Settings.music)
				underSettingsMusic.texture =ResourcesController.LoadCompressedTexture ("Interface/Music");
				else
				underSettingsMusic.texture =ResourcesController.LoadCompressedTexture ("Interface/MusicOff");
				
			};


			
		} else {


			settings = new GUIButton ("Interface/Settings" + "___COMPRESSED", null, null, 10 + 88 / 2f, 10 + 88 / 2f, 88, 88, 0 + deltaLayer, true);
			settings.OnClick = () => {
				
                isSettingsClick = true;
		        AudioController.instance.CreateAudio ("click");
				isUnderSettingsOpening = !isUnderSettingsOpening;
			};
			
			underSettings = new GUIButton ("Interface/UnderSettings" + "___COMPRESSED", null, null, 88 / 2f + 283 / 2f, 10 + 88 / 2f, 283, 111, -1 + deltaLayer, true);
			underSettingsSounds.OnClick = () => {

                isSettingsClick = true;
            };
			underSettingsExit = new GUIButton ("Interface/Exit" + "___COMPRESSED", null, null, 88 / 2f + 10, 10 + 88 / 2f - 5, 66, 63, -0.5f + deltaLayer, true);
			underSettingsExit.OnClick = () => {

                isSettingsClick = true;
		        AudioController.instance.CreateAudio ("click");
				onExit();
			};
			underSettingsSounds = new GUIButton ("Interface/Sounds" + "___COMPRESSED", null, null, 88 / 2f + 10, 10 + 88 / 2f - 5, 64, 59, -0.5f + deltaLayer, true);
			underSettingsSounds.OnClick = () => {

                isSettingsClick = true;
		        AudioController.instance.CreateAudio ("click");
				Settings.sounds = !Settings.sounds;
				
				if (Settings.sounds)
					underSettingsSounds.texture =ResourcesController.LoadCompressedTexture ("Interface/Sounds");
				else
					underSettingsSounds.texture =ResourcesController.LoadCompressedTexture ("Interface/SoundsOff");

			};
			underSettingsMusic = new GUIButton ("Interface/Music" + "___COMPRESSED", null, null, 88 / 2f + 10, 10 + 88 / 2f - 5, 58, 57, -0.5f + deltaLayer, true);
			underSettingsMusic.OnClick = () => {
				
                isSettingsClick = true;
		        AudioController.instance.CreateAudio ("click");
				Settings.music = !Settings.music;
				
				if (Settings.music)
					underSettingsMusic.texture =ResourcesController.LoadCompressedTexture ("Interface/Music");
				else
					underSettingsMusic.texture =ResourcesController.LoadCompressedTexture ("Interface/MusicOff");
				
			};


		}


		isUnderSettingsOpening = false;

		underSettingsPosition = 0;

		
		if (Settings.sounds)
			underSettingsSounds.texture =ResourcesController.LoadCompressedTexture ("Interface/Sounds");
		else
			underSettingsSounds.texture =ResourcesController.LoadCompressedTexture ("Interface/SoundsOff");
		
		if (Settings.music)
			underSettingsMusic.texture =ResourcesController.LoadCompressedTexture ("Interface/Music");
		else
			underSettingsMusic.texture =ResourcesController.LoadCompressedTexture ("Interface/MusicOff");
		
	}

	public static void CreateFlyingText (string texture, float _stopTime, GameController.Action _onFlyingTextOver) {

        AudioController.instance.CreateAudio ("lenta");
		
		if (flyingText != null) 
			flyingText.Destroy ();

		flyingText = new GUIImage ();
		flyingText.texture =ResourcesController.LoadCompressedTexture (texture);
		flyingText.layer = -4.5f;
		flyingText.sizeInMeters = new Vector2 (1200, 188) / 50f;
		flyingText.positionInMeters = new Vector2 (1200, 0) / 50f;

		onFlyingTextOver = _onFlyingTextOver;
		stopTime = _stopTime;
	}

	public static void Update (float deltaTime) {

		if (flyingText != null) {

			if (flyingText.positionInMeters.x <= 0 && stopTime > 0) {

                stopTime -= deltaTime;

                if (stopTime <= 0)
                    AudioController.instance.CreateAudio ("lenta");

            }
			else
				flyingText.positionInMeters -= new Vector2 (deltaTime * 20f, 0);

			if (flyingText.positionInMeters.x <= -1300 / 50f) {

				flyingText.Destroy ();
				flyingText = null;

				onFlyingTextOver ();
			}
		}

		if (!GUIController.isUnderSettingsOpening && GUIController.underSettingsPosition > 0) {
			
			GUIController.underSettingsPosition -= deltaTime * underSettingsSpeed;
		} 
		
		if (GUIController.isUnderSettingsOpening && GUIController.underSettingsPosition < 1) {
			
			GUIController.underSettingsPosition += deltaTime * underSettingsSpeed;
		} 
	}

	

}
