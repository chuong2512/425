using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameController : MonoBehaviour {


	public static GameController instance;

	public static bool isGame;
	public static bool isPaused;
	public static bool isLevelEnding;

	public enum Platform {PC, Phone}
	public static Platform platform;

	public delegate void Action();
	public delegate void ActionCallback(Action callback);
	
	public static float mapWidth = 1200 / 50f;
	public static float mapHeight = 800 / 50f;

    private static float interfaceLayer = -5f;
	
	public static bool isLoaded = false;

    private bool isText;

	private static int toFindId;
	private static int maxToFind;
	private static int foundCount;

	private static bool isGameStarted;

	private static float interfaceScale = 1.18519f;

	private static List < GUIImage > toFindDivs;

	private static string[] discriptionArray;
	private static string[] selectedArray;
	private static Vector2[] selectedPositions;

	private static bool isCameraMinimizing;
	private static Action afterCameraMinimized;

	private static Settings.Language lng;

	public static Dictionary <GameObject, Div> divs;

	private static List < int > objectsToFind;
	private static List < int > objectsToFindLeft;

    private static GameObject music;
    private GUIImage pauseText;

    private static int currentStars;

	private static int _score;
	private static int score {
		get { return _score; }
		set {
			_score = value;
			scoreImage.text = value + "";
		    scoreImage.textScaleInPixels = new Vector2 (4f, 4f) * interfaceScale * (scoreImage.text.Length > 4?(4f / scoreImage.text.Length):1);
            scoreImage.top = (675/ 2f + 28) * interfaceScale + (scoreImage.text.Length > 4?5f * interfaceScale:0);
		}

	}

	
	private static float _time;
	private static float time {
		get { return _time; }
		set {
			_time = value;
			timeImage.text = ((int) value) + "";
		    timeImage.textScaleInPixels = new Vector2 (4f, 4f) * interfaceScale * (timeImage.text.Length > 4?(4f / timeImage.text.Length):1);
            timeImage.top = (675/ 2f + 100) * interfaceScale + (timeImage.text.Length > 4?5f * interfaceScale:0);
		}
		
	}

	private static GUIImage scoreImage;
	private static GUIImage timeImage;

	private static GUIImage skillGreen;
	private static GUIImage skillText;

	private static GUIImage backgroundExitConfirm = null;
	private static GUIButton yesExitConfirm;
	private static GUIButton noExitConfirm;

	public static float skillCooldown {
		set { 
			skillGreen.gameObject.GetComponent <Renderer> ().material.SetFloat ("_Cutoff", 0.01f * (1-value) + 1*value);
		}
	}

	private static GUIImage underInterface;
	public static float interfaceWidth {
		get { 
			if (underInterface == null)
				return 0;
			return underInterface.sizeInMeters.x; }
	}


	public static float skillCooldownMaxTime = 15;

	private static float _skillCooldownTime;
	public static float skillCooldownTime {
		get { return _skillCooldownTime; }
		set {
			_skillCooldownTime = value;
			skillCooldown = skillCooldownTime / skillCooldownMaxTime;

			if (skillCooldownTime <= 0 && skillText != null) {

				skillText.Destroy ();
				skillText = null;
			}

			if (skillCooldownTime <= 0) 
				return;

			if (skillText == null) {
				
				skillText = new GUIImage ("", 147 * interfaceScale / 2f
				                          , 530 * interfaceScale,null
				                          , null, 0
				                          , 0,0 + interfaceLayer,true);
				//skillText.isCentreText = true;
				skillText.textTransform = GUIController.CreateText (ref skillText.textObject);
				skillText.deltaTextPositionInPixels = new Vector2 (0, 0);
				skillText.textScaleInPixels = new Vector2 (3f, 3f);
				skillText.text = Mathf.Floor(skillCooldownTime + 1).ToString ();
				skillText.SetPosition ();
				skillText.SetSize ();
				skillText.SetText ();
			} else {

				if (skillCooldownTime < 10)
					skillText.left = 160 * interfaceScale / 2f;
				else
					skillText.left = 147 * interfaceScale / 2f;

				skillText.text = Mathf.Floor(skillCooldownTime + 1).ToString ();
			}
		}
	}

	private int missclickCount;
	private float deltaMisslickTime;
	
	private float deltaObjectFoundTime;
	
	private static GUIImage blackDDOS;
	private static GUIImage madgardDDOS;
	private static GUIImage textDDOS;
	private static GUIImage secondsDDOS;
	
	private static GUIImage backgroundResults;
	private static GUIImage signResults;
	private static GUIImage starLeftResults;
	private static GUIImage starCentreResults;
	private static GUIImage starRightResults;
	private static GUIButton exitResults;
	private static GUIImage scoreResults;
	private static GUIImage dragonResults;
	private static GUIImage lightResults;
	private static GUIImage blackResults;
	private static GUIButton okResults;
	private static int moveResultsIndex;
	private int starsSum;
	private static int scoreResultsOn;

    private static string[] paramArray3;

	private static GUIButton spam;
	
    private static GameObject musicWin;

	private static List < int > RandomShuffle (List < int > array) {

		int currentIndex = array.Count, temporaryValue, randomIndex ;
		
		while (currentIndex != 0) {
			
			randomIndex = (int) Mathf.Floor ( Random.value * currentIndex);
			currentIndex -= 1;
			temporaryValue = array[currentIndex];
			array[currentIndex] = array[randomIndex];
			array[randomIndex] = temporaryValue;
		}
		
		return array;
	}

	private static string GetSelectedObject (string obj) {

		int slashPos = obj.LastIndexOf ('/');
		string res = obj.Substring (0, slashPos + 1);
		res += "2_";
		res += obj.Substring (slashPos + 1);
		return res;
	}

	private static int GetNewObjectToFind () {

		if (objectsToFindLeft.Count <= 0 || foundCount == maxToFind)
			return -1;

		int res = objectsToFindLeft [objectsToFindLeft.Count - 1];
		objectsToFindLeft.RemoveAt (objectsToFindLeft.Count - 1);
		return res;
	}

	private static string GetCoolWord (int level) {
		
		int res = Random.Range (level, 6);
		switch (Settings.language) {
			
		case Settings.Language.English:
			switch (res) {
				
			case 0: return "PERFECTLY";
			case 1: return "GREAT";
			case 2: return "BRILLIANT";
			case 3: return "COOL";
			case 4: return "EXCELLENT";
			case 5: return "AWESOME";
			case 6: return "AMAZING";
			case 7: return "FANTASTIC";
			}
			break;
		case Settings.Language.Russian:
			switch (res) {
				
			case 0: return "QRFLRASOP";
			case 1: return "CFMJLPMFQOP";
			case 2: return "BMFST/[F";
			case 3: return "LRUTP";
			case 4: return "QRFCPSWPEOP";
			case 5: return "QPTR/SA&[F";
			case 6: return "JIUNJTFM^OP";
			case 7: return "VAOTASTJYFSLJ";
			}
			break;

		}

		return "URAMAZING";
	}


	public static void OnButtonDown(Vector2 position) {
		
		Ray ray;
		RaycastHit hit;
		
		ray = Camera.main.ScreenPointToRay(position);
		if (Physics.Raycast(ray,out hit, 100)) {
			
			if (hit.transform.gameObject.name.Contains("GUIButton")) {
				GUIController.OnButtonDown (hit.transform.gameObject);
			}
		}
	}
	
	public static void OnButtonUp(Vector2 position) {

		GUIController.OnButtonUp ();
	}



	private IEnumerator MoveObject (Div obj, float targetPositionX = -1000, float targetPositionY = -1000, Action action = null) {


		while (Vector2.Distance (obj.positionInMeters,(targetPositionX != -1000?new Vector2 (targetPositionX,targetPositionY):skillGreen.positionInMeters)) > 0.1f) {

			obj.positionInMeters += ((targetPositionX != -1000?new Vector2 (targetPositionX,targetPositionY):skillGreen.positionInMeters) - obj.positionInMeters).normalized * 0.01f * 25f;
			obj.gameObject.GetComponent <Renderer> ().material.color -= new Color (0,0,0,0.01f);
			yield return new WaitForSeconds (0.01f);
		}

		if (action != null)
			action ();

		obj.Destroy (ref divs);
		yield return null;
	}

	private IEnumerator MoveObject (GUIImage obj, float targetPositionX = -1000, float targetPositionY = -1000, Action action = null) {
		
		while (Vector2.Distance (obj.positionInMeters,(targetPositionX != -1000?new Vector2 (targetPositionX,targetPositionY):skillGreen.positionInMeters)) > 0.1f) {

			obj.positionInMeters += ((targetPositionX != -1000?new Vector2 (targetPositionX,targetPositionY):skillGreen.positionInMeters) - obj.positionInMeters).normalized * 0.01f * 15f;
			yield return new WaitForSeconds (0.01f);
		}
		
		if (action != null)
			action ();
		yield return new WaitForSeconds (0.5f);
		obj.Destroy ();
		yield return null;
	}
	
	private IEnumerator ScaleObject (Div obj) {
		
		for (int i = 0; i < 20; i++) {
			obj.size += new Vector2 (0.04f,0.04f);
			yield return new WaitForSeconds (0.02f);
		}
		StartCoroutine (MoveObject (obj));
		yield return null;
	}

	private void StartMovingObject (Div obj) {

		StartCoroutine (ScaleObject (obj));
	}

	private IEnumerator MoveText (GUIImage obj) {
		
		while (obj.gameObject.GetComponent <Renderer> ().material.color.a > 0) {

			obj.positionInMeters += new Vector2 (0, 100 / 50f * 0.01f);
			obj.textObject.gameObject.GetComponent <Renderer> ().material.color -= new Color (0,0,0,0.01f);
			yield return new WaitForSeconds (0.01f);
		}

		obj.Destroy ();
	}

	private void CreateFlyingText (Vector2 position, string text) {


		var textObj = new GUIImage ();
		textObj.isCentreText = true;
		textObj.layer = -5;
		textObj.sizeInMeters = new Vector2 (0, 0);	
		textObj.textTransform = GUIController.CreateText (ref textObj.textObject);
		textObj.deltaTextPosition = new Vector2 (0, 0);
		textObj.textScale = new Vector2 (0.07f, 0.07f);
		textObj.positionInMeters = position;
		textObj.text = text;
		textObj.SetPosition ();
		textObj.SetSize ();
		textObj.SetText ();

		StartCoroutine (MoveText (textObj));
	}
	
	
	public void OnObjectClick(int id) {

		if (missclickCount >= 5)
			return;

        if (isPaused)
            return;

		if (objectsToFind.Contains (id)) {

            if (!GameObject.Find("ToFindX"+id)) 
                return;

			foundCount ++;

            AudioController.instance.CreateAudio ("naiden");
			
			int toAdd = -100;

			if (deltaObjectFoundTime > -3) {
				
				if (deltaObjectFoundTime > -3) {
					toAdd = Settings.forOneItem * 3 / 2;
				}
				if (deltaObjectFoundTime > -2) {
					toAdd = Settings.forOneItem * 2;
				}
				if (deltaObjectFoundTime > -1) {
					toAdd = Settings.forOneItem * 3;
				}

				Vector2 r = divs[GameObject.Find("ToFindX"+id)].positionInMeters + new Vector2 (- 100 / 50f, 50 / 50f);
				Vector2 q = divs[GameObject.Find("ToFindX"+id)].positionInMeters + new Vector2 (0 / 50f, 50 / 50f);
					
				if (r.x < - mapWidth / 2f) {

					q += new Vector2 (- mapWidth / 2f - r.x, 0);
					r += new Vector2 (- mapWidth / 2f - r.x, 0);
				}

                if (q.x > mapWidth / 2f) {

					q -= new Vector2 (- mapWidth / 2f - q.x, 0);
					r -= new Vector2 (- mapWidth / 2f - q.x, 0);
				}

				CreateFlyingText (r, toAdd.ToString ());
				CreateFlyingText (q, GetCoolWord (0));
			} else {

				toAdd = Settings.forOneItem;
				CreateFlyingText (divs[GameObject.Find("ToFindX"+id)].positionInMeters + new Vector2 (- 50 / 50f, 50 / 50f), toAdd.ToString ());
			}

			score += toAdd;

			deltaObjectFoundTime = 0;
			

			int stars = 0;
			
			if (score >= Settings.forOneStar)
				stars = 1;
			
			if (score >= Settings.forTwoStars)
				stars = 2;
			
			if (score >= Settings.forThreeStars)
				stars = 3;

            currentStars = stars;

			if (foundCount == maxToFind) {

				isCameraMinimizing = true;
				afterCameraMinimized = () => {

                    SlideController.mode = SlideController.Mode.Slide;
                    
                    isLevelEnding = true;

					GUIController.CreateFlyingText ("$" + lng + "/Interface/LevelCompleteText", 1f, () => {

                        AudioController.instance.RemoveAudio (music);

						Settings.isLevelEnded = true;
						Settings.scoreEnded = score;
						Settings.starsEnded = stars;
						
                        if (isPaused) {
                            pauseText.Destroy ();
                            GUIController.isUnderSettingsOpening = false;
                            isPaused = false;
                            ScenePassageController.instance.UnPause (() => {

						        CreateResults (Settings.scoreEnded, currentStars);
						        StartCoroutine (MoveResults (Settings.starsEnded));
                            });
                        } else {
                            
						    CreateResults (Settings.scoreEnded, currentStars);
						    StartCoroutine (MoveResults (Settings.starsEnded));
                        }

					});
				};
			}

			Texture selectedTexture =ResourcesController.LoadCompressedTexture ("$English/"+selectedArray[id]);

			divs[GameObject.Find("ToFindX"+id)].texture = selectedTexture;
			divs[GameObject.Find("ToFindX"+id)].size = new Vector2 (selectedTexture.width / 50f, selectedTexture.height / 50f);
			divs[GameObject.Find("ToFindX"+id)].position = selectedPositions[id] / 50f + divs[GameObject.Find("ToFindX"+id)].size / 2f;
			divs[GameObject.Find("ToFindX"+id)].layer += 1;

			StartMovingObject (divs[GameObject.Find("ToFindX"+id)]);

			divs[GameObject.Find("ToFindX"+id)].gameObject.name = "Found";


			if (PlayerPrefs.GetInt ("score"+Settings.location.ToString()+"Pack"+Settings.levelPack+"Level"+Settings.level) < score) {

				PlayerPrefs.SetInt ("score"+Settings.location.ToString()+"Pack"+Settings.levelPack+"Level"+Settings.level, score);


				if (PlayerPrefs.GetInt ("stars"+Settings.location.ToString()+"Pack"+Settings.levelPack+"Level"+Settings.level) < stars) {

					PlayerPrefs.SetInt ("stars"+Settings.location.ToString()+"Pack"+Settings.levelPack+"Level"+Settings.level, stars);
				}
			}


			int _id = 0;
			for (; id != objectsToFind [_id]; _id ++);

            if (isText) {

                toFindDivs[_id].text = "";
            } else {
                
			    toFindDivs[_id].texture = null;
			    toFindDivs[_id].sizeInPixels = new Vector2 (0,0);
			    toFindDivs[_id].sizeInMeters = new Vector2 (0,0);
            }
            objectsToFind [_id] = -1;

             if (foundCount % 4 == 0 && foundCount != 0) {
                    
                if (isText) {

                    for (int q = 0; q < 4; q++) {
                            
                        objectsToFind [q] = GetNewObjectToFind ();
                            
                        if (objectsToFind [q] != -1)
                            toFindDivs[q].text = discriptionArray[objectsToFind [q]];
                        else
                            toFindDivs[q].text = "";

                        toFindDivs[q].textScaleInPixels = new Vector2 (3f, 3f) * interfaceScale * (toFindDivs[q].text.Length > 8?(8f / toFindDivs[q].text.Length):1);

                        if ( toFindDivs[q].text.Contains ("\n")) {

                            toFindDivs[q].top = (23 + 77*q + 15/1.25f) * interfaceScale;
                            toFindDivs[q].textScaleInPixels = new Vector2 (3f, 3f) * interfaceScale;
                        }
                    }

                } else {

                    for (int q = 0; q < 4; q++) {
                         
                        objectsToFind [q] = GetNewObjectToFind ();    
                        if (objectsToFind [q] != -1) {
                            
				            var texture =ResourcesController.LoadCompressedTexture ("$"+lng+"/"+discriptionArray[objectsToFind [q]]);
				            toFindDivs[q].texture = texture;
				            toFindDivs[q].sizeInPixels = ScaleSize(new Vector2 (texture.width,texture.height), new Vector2 (152 * interfaceScale, 74 * interfaceScale) * 0.9f);
                        } else {
                            
				            toFindDivs[q].texture = null;
				            toFindDivs[q].sizeInPixels = new Vector2 (0,0);
				            toFindDivs[q].sizeInMeters = new Vector2 (0,0);
                        }
                    }
                }
            } 

		} else
			Missclick ();
	}

	private Vector2 ScaleSize (Vector2 target, Vector2 bounds) {

		if (target.x * bounds.y / target.y <= bounds.x)
			return target * bounds.y / target.y;
			
		return target * bounds.x / target.x;
	}

	public void CreateLevel(string config, string config2, string config3 = "") {

        isText = (config3 != "");

        config3 = config3.Replace ("\\n","\n");

        var paramArray = config.Split('|');
		

		skillText = null;

		var paramArray2 = config2.Split('|');

        paramArray3 = config3.Split ('|');
		
		maxToFind = (paramArray.Length-3)/6;

		discriptionArray = new string[maxToFind];
		selectedArray = new string[maxToFind];
		selectedPositions = new Vector2[maxToFind];

		for (var i = maxToFind-1; i>=0; i--) {
            
			var thisDiv = new Div("$English/"+paramArray[6*i+2],float.Parse(paramArray[6*i+5]),float.Parse(paramArray[6*i+6]),float.Parse(paramArray[6*i+3]),float.Parse(paramArray[6*i+4]),1, ref divs);
			thisDiv.gameObject.name = "ToFindX" + i;

			selectedArray[i] = GetSelectedObject (paramArray[6*i+2]);
			selectedPositions[i] = new Vector2 (float.Parse(paramArray2[6*i+5]),float.Parse(paramArray2[6*i+6]));
			discriptionArray[i] = Settings.TranslateText (isText?paramArray3[i]:paramArray[6*i+7]);
			objectsToFindLeft.Add (i);
		}

		objectsToFindLeft = RandomShuffle (objectsToFindLeft);

		for (int i = 0; i < 4; i++) 
			objectsToFind.Add (GetNewObjectToFind ());

        if (isText) {

            toFindDivs.Add (new GUIImage ("", (187 - 132*0.9f) * interfaceScale / 2f
		                                  , (23 + 30/1.25f) * interfaceScale,null,null
		                                  , 0
		                                  , 0, -0.25f + interfaceLayer, true));
		    toFindDivs.Add (new GUIImage ("", (187 - 132*0.9f)  * interfaceScale / 2f
		                                  , (100 + 30/1.25f) * interfaceScale,null,null
		                                  , 0
		                                  , 0, -0.25f + interfaceLayer, true));
		    toFindDivs.Add (new GUIImage ("", (187 - 132*0.9f)  * interfaceScale / 2f
		                                  , (177 + 30/1.25f) * interfaceScale,null,null
		                                  , 0
		                                  , 0, -0.25f + interfaceLayer, true));
		    toFindDivs.Add (new GUIImage ("", (187 - 132*0.9f)  * interfaceScale / 2f
		                                  , (254 + 30/1.25f) * interfaceScale,null,null
		                                  , 0
		                                  , 0, -0.25f + interfaceLayer, true));
            
            for (int i = 0; i < 4; i++) {

		        toFindDivs[i].textTransform = GUIController.CreateText (ref toFindDivs[i].textObject);
		        toFindDivs[i].deltaTextPosition = new Vector2 (0, 0);
		        toFindDivs[i].text = discriptionArray [objectsToFind [i]];
		        toFindDivs[i].textScaleInPixels = new Vector2 (3f, 3f) * interfaceScale * (toFindDivs[i].text.Length > 8?(8f / toFindDivs[i].text.Length):1);

                if ( toFindDivs[i].text.Contains ("\n")) {

                    toFindDivs[i].top = (23 + 77*i + 15/1.25f) * interfaceScale;
                    toFindDivs[i].textScaleInPixels = new Vector2 (3f, 3f) * interfaceScale;
                }

		        toFindDivs[i].SetText();
		        toFindDivs[i].SetPosition();
		        toFindDivs[i].SetSize();
            }

        } else {

		    toFindDivs.Add (new GUIImage ("$"+lng+"/"+discriptionArray [objectsToFind [0]] + "___COMPRESSED", 187 * interfaceScale / 2f
		                                  , (23 + 10 + 30/1.25f) * interfaceScale,null,null
		                                  , 152*0.9f * interfaceScale
		                                  , 40.53f*0.9f * interfaceScale, -0.25f + interfaceLayer, true));
		    toFindDivs.Add (new GUIImage ("$"+lng+"/"+discriptionArray [objectsToFind [1]] + "___COMPRESSED", 187 * interfaceScale / 2f
		                                  , (100 + 10 + 30/1.25f) * interfaceScale,null,null
		                                  , 152*0.9f * interfaceScale
		                                  , 40.53f*0.9f * interfaceScale, -0.25f + interfaceLayer, true));
		    toFindDivs.Add (new GUIImage ("$"+lng+"/"+discriptionArray [objectsToFind [2]] + "___COMPRESSED", 187 * interfaceScale / 2f
		                                  , (177 + 10 + 30/1.25f) * interfaceScale,null,null
		                                  , 152*0.9f * interfaceScale
		                                  , 40.53f*0.9f * interfaceScale, -0.25f + interfaceLayer, true));
		    toFindDivs.Add (new GUIImage ("$"+lng+"/"+discriptionArray [objectsToFind [3]] + "___COMPRESSED", 187 * interfaceScale / 2f
		                                  , (254 + 10 + 30/1.25f) * interfaceScale,null,null
		                                  , 152*0.9f * interfaceScale
		                                  , 40.53f*0.9f * interfaceScale, -0.25f + interfaceLayer, true));

		    var texture1 =ResourcesController.LoadCompressedTexture ("$"+lng+"/"+discriptionArray [objectsToFind [0]]);
		    var texture2 =ResourcesController.LoadCompressedTexture ("$"+lng+"/"+discriptionArray [objectsToFind [1]]);
		    var texture3 =ResourcesController.LoadCompressedTexture ("$"+lng+"/"+discriptionArray [objectsToFind [2]]);
		    var texture4 =ResourcesController.LoadCompressedTexture ("$"+lng+"/"+discriptionArray [objectsToFind [3]]);

		    toFindDivs [0].sizeInPixels = ScaleSize(new Vector2 (texture1.width,texture1.height), new Vector2 (152 * interfaceScale, 74 * interfaceScale) * 0.9f);
		    toFindDivs [1].sizeInPixels = ScaleSize(new Vector2 (texture2.width,texture2.height), new Vector2 (152 * interfaceScale, 74 * interfaceScale) * 0.9f);
		    toFindDivs [2].sizeInPixels = ScaleSize(new Vector2 (texture3.width,texture3.height), new Vector2 (152 * interfaceScale, 74 * interfaceScale) * 0.9f);
		    toFindDivs [3].sizeInPixels = ScaleSize(new Vector2 (texture4.width,texture4.height), new Vector2 (152 * interfaceScale, 74 * interfaceScale) * 0.9f);
        }

        GUIController.OnCameraUpdate ();

		GUIController.CreateFlyingText ("$" + Settings.language + "/Interface/GetReadyText", 1f, () => {

			isGameStarted = true;
		});

        
        maxToFind = Settings.maxItemsToFind;
	}



	public void Create() {

		instance = this;
		isGame = true;
        isPaused = false;
        isLevelEnding = false;
		isCameraMinimizing = false;
        
		toFindDivs = new List< GUIImage > ();
		objectsToFind = new List<int> ();
		objectsToFindLeft = new List<int> ();
		foundCount = 0;

		moveResultsIndex = 51;
		
		skillCooldownMaxTime = 15;
		deltaObjectFoundTime = -100;

		isGameStarted = false;
        currentStars = 0;
		interfaceScale = Screen.height/(GUIController.width/GUIController.GUIBackgroundWidth * 675);

		divs = new Dictionary<GameObject, Div> ();
		
		AnimationBox.LoadAnimations ();
		AnimationController.Create();
		GamePullController.Create();
		GUIController.Create ();
		CameraController.ResizeCamera(CameraController.GetWidthInMeters(GameController.mapHeight));
		SlideController.Create (mapWidth, mapHeight);
		isLoaded = true;

			
		int levelIndex =  ( (((int) Settings.location))*3+Settings.levelPack) * 8 + (Settings.level);

		var cfg = Settings.configPoints.Split ('|') [levelIndex];
		Settings.forOneItem = int.Parse(cfg.Split ('_') [0]);
		Settings.forOneStar = Settings.forOneItem * 11;
		Settings.forTwoStars = Settings.forOneItem * 12;
		Settings.forThreeStars = Settings.forOneItem * (Settings.GetFor3StarsPoints (Settings.level, Settings.levelPack) +1);
        
        if (levelIndex == 0)
            Settings.forThreeStars = Settings.forOneItem * 8;
            
		Div.start = new Vector2 (-1200 / 100f, -800 / 100f);

		var background = new GUIButton ();
		background.texture =ResourcesController.LoadCompressedTexture ("$English/"+"img/level"+( ((int) Settings.location)*3+Settings.levelPack + 1)+"/Background");
		background.layer = -19;
		background.positionInMeters = new Vector2 (0, 0);
		background.sizeInMeters = new Vector2 (1200 / 50f, 800 / 50f);
		background.OnClick = () => {
			
			if (missclickCount >= 5)
				return;
			Missclick ();
		};


		lng = Settings.language;

		if (Settings.level == 1 || Settings.level == 4 || Settings.level == 7) {

			lng = Settings.Language.English;
            
		    CreateLevel (LevelsHolder.GetConfig("level"+( ((int) Settings.location)*3+Settings.levelPack + 1)+"folder" + (Settings.level + 1))
		                 ,LevelsHolder.GetConfig2("level"+( ((int) Settings.location)*3+Settings.levelPack + 1)+"folder" + (Settings.level + 1)));
		} else {
            /*
		CreateLevel ((Resources.Load ("$English/" + "img/level"+( ((int) Settings.location)*3+Settings.levelPack + 1)+"/" + (Settings.level + 1) + "/Config") as TextAsset).text
		             , (Resources.Load ("$English/" + "img/level"+( ((int) Settings.location)*3+Settings.levelPack + 1)+"/" + (Settings.level + 1) + "/Config2") as TextAsset).text
                     , Settings.TranslateText ((Resources.Load ("$"+Settings.language+"/" + "img/level"+( ((int) Settings.location)*3
                                        + Settings.levelPack + 1)+"/" + (Settings.level + 1) + "/Config3") as TextAsset).text));
                         */              
           
		CreateLevel (LevelsHolder.GetConfig("level"+( ((int) Settings.location)*3+Settings.levelPack + 1)+"folder" + (Settings.level + 1))
		             ,LevelsHolder.GetConfig2("level"+( ((int) Settings.location)*3+Settings.levelPack + 1)+"folder" + (Settings.level + 1))
                     ,LevelsHolder.GetConfig3("level"+( ((int) Settings.location)*3+Settings.levelPack + 1)+"folder" + (Settings.level + 1)));
                      
                      
                                       
        }
		
		scoreImage = new GUIImage ("", (187 / 2f - 20) * interfaceScale
		                           , (675/ 2f + 28) * interfaceScale,null
		                          , null, 0
		                          , 0,0 + interfaceLayer,true);
		scoreImage.textTransform = GUIController.CreateText (ref scoreImage.textObject);
		scoreImage.deltaTextPosition = new Vector2 (0, 0);
		scoreImage.textScaleInPixels = new Vector2 (4f, 4f) * interfaceScale;
		scoreImage.text = "0";
		scoreImage.SetText();
		scoreImage.SetPosition();
		scoreImage.SetSize();

		
		timeImage = new GUIImage ("", (187 / 2f - 20) * interfaceScale
		                           , (675/ 2f + 100) * interfaceScale,null
		                           , null, 0
		                           , 0,0 + interfaceLayer,true);
		timeImage.textTransform = GUIController.CreateText (ref timeImage.textObject);
		timeImage.deltaTextPosition = new Vector2 (0, 0);
		timeImage.textScaleInPixels = new Vector2 (4f, 4f) * interfaceScale;
		timeImage.text = "0";
		timeImage.SetText();
		timeImage.SetPosition();
		timeImage.SetSize();


		//skillGreen = new Div("Interface/GreenAlpha", -221.63f + 22 * interfaceScale,  505 * interfaceScale, 141 * interfaceScale, 141 * interfaceScale,-0.75f, ref divs);
		
		
		var skill = new GUIImage ("Interface/SkillGrey" + "___COMPRESSED", 187 * interfaceScale / 2f
		                          , 577 * interfaceScale,null
		                          , null, 136 * interfaceScale
		                          , 136 * interfaceScale,-0.25f + interfaceLayer,true);
		
		var skill1 = new GUIImage ("Interface/Green" + "___COMPRESSED", 187 * interfaceScale / 2f
		                           , 577 * interfaceScale,null
		                           , null, 141 * interfaceScale
		                           , 141 * interfaceScale,-0.5f + interfaceLayer,true);

		var skill2 = new GUIButton ("Interface/SkillTop" + "___COMPRESSED", 187 * interfaceScale / 2f
		                           , 577 * interfaceScale,null
		                           , null, 141 * interfaceScale
		                           , 141 * interfaceScale,0f + interfaceLayer,true);
		skill2.OnClick = () => {

			Skill ();
		};
		
		skill2.OnButtonDown = () => {

			skill2.texture =ResourcesController.LoadCompressedTexture ("Interface/SkillTopPushed" + "___COMPRESSED");
		};
		
		skill2.OnButtonUp = () => {
			
			skill2.texture =ResourcesController.LoadCompressedTexture ("Interface/SkillTop" + "___COMPRESSED");
		};


		skillGreen = new GUIImage ("Interface/SkillAlpha" + "___COMPRESSED", 187 * interfaceScale / 2f
		                          , 577 * interfaceScale,null
		                           , null, 136 * interfaceScale
		                           , 136 * interfaceScale,-0.1f + interfaceLayer,true,true);
		
		skillCooldownTime = skillCooldownMaxTime;

		score = 0;
		time = 0;



		underInterface = new GUIImage ("Interface/UnderInterface" + "___COMPRESSED", 187 * interfaceScale / 2f
		                                   , 675 * interfaceScale / 2f,null
		                                   , null, 187 * interfaceScale
		                                   , 675 * interfaceScale,-0.75f + interfaceLayer,true);
		
		var box1 = new GUIImage ("Interface/BoxBlack" + "___COMPRESSED", 187 * interfaceScale / 2f
		                         , (23 + 74/2f) * interfaceScale,null,null
		                         , 152 * interfaceScale
		                         , 74 * interfaceScale, -0.5f + interfaceLayer, true);
		
		var box2 = new GUIImage ("Interface/BoxBlack" + "___COMPRESSED", 187 * interfaceScale / 2f
		                         , (100 + 74/2f) * interfaceScale,null,null
		                         , 152 * interfaceScale
		                         , 74 * interfaceScale, -0.5f + interfaceLayer, true);
		
		var box3 = new GUIImage ("Interface/BoxBlack" + "___COMPRESSED", 187 * interfaceScale / 2f
		                         , (177 + 74/2f) * interfaceScale,null,null
		                         , 152 * interfaceScale
		                         , 74 * interfaceScale, -0.5f + interfaceLayer, true);
		
		var box4 = new GUIImage ("Interface/BoxBlack" + "___COMPRESSED", 187 * interfaceScale / 2f
		                         , (254 + 74/2f) * interfaceScale,null,null
		                         , 152 * interfaceScale
		                         , 74 * interfaceScale, -0.5f + interfaceLayer, true);
		

		missclickCount = 0;
		deltaMisslickTime = 0;


		CameraController.ResizeCamera( Mathf.Min(CameraController.GetWidthInMeters(mapHeight), mapWidth + interfaceWidth) );
		CameraController.cameraPosition = new Vector2(-((GameController.mapWidth - CameraController.widthInMeters)/2 + GameController.interfaceWidth), 0);

		GUIController.CreateSettings (true, () => {
                                               
                                               CreateExitConfirmMenu (() => {

                                                    if (currentStars == 3) {

			                                            Settings.frescoIndex = -1;
			                                            ScenePassageController.instance.LoadScene ("FrescoScene");
		                                            } else {

			                                            ScenePassageController.instance.LoadScene ("SelectLevelScene");
		                                            }

                                                   AudioController.instance.RemoveAudio (music);
                                               });


                                          }
                                          , 0
                                          , () => {

                                              ScenePassageController.instance.Pause (-3f, () => {

                                                                                            pauseText = new GUIImage ();
		                                                                                    pauseText.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/Interface/Pause");
		                                                                                    pauseText.layer = -2.5f;
		                                                                                    pauseText.left = Screen.width / 2f;
		                                                                                    pauseText.top = Screen.height / 2f;
		                                                                                    pauseText.sizeInPixels = new Vector2 (-1, -1);

                                                                                            isPaused = true;
                                                                                        }
                                              );
                                          }
                                          , () => {

                                              pauseText.Destroy ();
                                              ScenePassageController.instance.UnPause (() => {

                                              });
                                              isPaused = false;
                                          }
        );


        music = AudioController.instance.CreateAudio ("gameplay", true, true);
        AudioController.instance.PauseAudio (MainMenuController.musicTheme);

		ScenePassageController.OnSceneLoaded ();
	}


	private void Skill () {

		if (missclickCount >= 5)
			return;

		if (skillCooldownTime > 0 || foundCount == maxToFind)
			return;

        AudioController.instance.CreateAudio ("skill");

		isCameraMinimizing = true;
		afterCameraMinimized = UseSkill;
		skillCooldownMaxTime = 60;
		skillCooldownTime = skillCooldownMaxTime;
	}

	private void UseSkill () {

		int id;
		do {

			id = Random.Range (0, 4);
		} while (objectsToFind[id] == -1);

		var skill = new GUIImage ();
		skill.texture =ResourcesController.LoadCompressedTexture ("Interface/StarSkill");
		skill.layer = 3;
		skill.positionInMeters = skillGreen.positionInMeters;
		skill.sizeInMeters = new Vector2 (100 / 50f, 100 / 50f);

        var toFindObject = objectsToFind[id];

		StartCoroutine (MoveObject (skill,divs[GameObject.Find("ToFindX"+objectsToFind[id])].positionInMeters.x,divs[GameObject.Find("ToFindX"+objectsToFind[id])].positionInMeters.y, () => {

			OnObjectClick(toFindObject);
		}));
	}


	void Start() {


#if UNITY_IPHONE
		platform = Platform.Phone;
#endif
		
#if UNITY_ANDROID
		platform = Platform.Phone;
#endif

#if UNITY_STANDALONE_OSX
		platform = Platform.PC;
#endif

#if UNITY_STANDALONE_WIN
		platform = Platform.PC;
#endif
		
#if UNITY_EDITOR
		platform = Platform.PC;
#endif	

		Create();

	}

	private IEnumerator ScaleMissclick (GUIImage obj) {

		for (int q = 2; q >= 0; q--) {
			for (int i = 0; i < 20; i++) {

				obj.sizeInMeters *= 0.98f;
				yield return new WaitForSeconds (1 / 20f);
			}

			if (q != 0)
				obj.texture =ResourcesController.LoadCompressedTexture ("Interface/StopDDOS/"+q);

			obj.sizeInMeters = new Vector2 (-1,-1);
		}

		RemoveDDOS ();
		yield return null;
	}


	private void Missclick () {

		missclickCount ++;
		deltaMisslickTime = 0.5f;

		if (missclickCount >= 5) {
			
            AudioController.instance.CreateAudio ("dontTryToGuess");

            isCameraMinimizing = true;
            afterCameraMinimized = () => { };

            SlideController.mode = SlideController.Mode.ReadOnly;

			blackDDOS = new GUIImage ();
			blackDDOS.texture =ResourcesController.LoadCompressedTexture ("Interface/Black");
			blackDDOS.layer = -4;
			blackDDOS.positionInMeters = new Vector2 (0, 0);
			blackDDOS.sizeInMeters = new Vector2 (1200 / 50f, 1200 / 50f);
			
			madgardDDOS = new GUIImage ();
			madgardDDOS.texture =ResourcesController.LoadCompressedTexture ("Interface/StopDDOS/Madgard");
			madgardDDOS.layer = -3;
			madgardDDOS.positionInMeters = new Vector2 (- 200 / 50f, 130 / 50f);
			madgardDDOS.sizeInMeters = new Vector2 (-1, -1);

			textDDOS = new GUIImage ();
			textDDOS.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language.ToString ()+"/Interface/StopDDOS/Text");
			textDDOS.layer = -3;
			textDDOS.positionInMeters = new Vector2 ( 200 / 50f, 130 / 50f);
			textDDOS.sizeInMeters = new Vector2 (-1, -1);
			
			secondsDDOS = new GUIImage ();
			secondsDDOS.texture =ResourcesController.LoadCompressedTexture ("Interface/StopDDOS/3");
			secondsDDOS.layer = -3;
			secondsDDOS.positionInMeters = new Vector2 ( 0 / 50f, -165 / 50f);
			secondsDDOS.sizeInMeters = new Vector2 (-1, -1);
			
			StartCoroutine (ScaleMissclick (secondsDDOS));
		}


	}

	private void RemoveDDOS () {

		
		blackDDOS.Destroy ();
		madgardDDOS.Destroy ();
		textDDOS.Destroy ();
		secondsDDOS.Destroy ();
		missclickCount = 0;

        SlideController.mode = SlideController.Mode.SlideAndZoom;

	}


	
	private IEnumerator MoveGUIObject (Vector2 startPostion, Vector2 endPosition, GUIObject target) {
		
		for (int i = 0; i <= 25; i++) {
			
			target.positionInMeters = Vector2.Lerp (startPostion, endPosition, i*i/(25*25f));
			yield return new WaitForSeconds (0.01f);
		}
		
		CreateStarsParticles (endPosition);
	}
	
	private void CreateStarsParticles (Vector2 position) {
		
		GameObject starsParticles = Instantiate (Resources.Load ("Prefabs/StarsParticles")) as GameObject;
		starsParticles.transform.position = new Vector3 (position.x, GUIController.layer - 2f, position.y);
		
	}
	
	private IEnumerator AnimateStars() {
		
		yield return new WaitForSeconds (0.5f);
		
		
		if (currentStars > 0) {

            AudioController.instance.CreateAudio ("zvezda");
			
			starLeftResults.layer = -2;
			starLeftResults.positionInMeters = new Vector2 ((139 + 140 - 746 / 2f) / 50f, -(263 - 668 / 2f) / 50f);
			StartCoroutine (MoveGUIObject (starLeftResults.positionInMeters
			                               , new Vector2 ((164 + 140 - 20 + 154 / 2f - 746 / 2f) / 50f, -(301 - 20 + 159 / 2f - 668 / 2f) / 50f)
			                               , starLeftResults));
		}
        
        if (currentStars == 0)
            yield return null;

		yield return new WaitForSeconds (1f);
		
		if (currentStars > 1) {

            AudioController.instance.CreateAudio ("zvezda");
			
			starCentreResults.layer = -2;
			starCentreResults.positionInMeters = new Vector2 ((369 + 140 - 746 / 2f) / 50f, -(176 - 668 / 2f) / 50f);
			StartCoroutine (MoveGUIObject (starCentreResults.positionInMeters
			                               , new Vector2 ((307 + 140 - 20 + 160 / 2f - 746 / 2f) / 50f, -(243 - 20 + 155 / 2f - 668 / 2f) / 50f)
			                               , starCentreResults));
		}

        if (currentStars == 1)
            yield return null;

		yield return new WaitForSeconds (1f);
		
		if (currentStars > 2) {
			
            AudioController.instance.CreateAudio ("zvezda");

			starRightResults.layer = -2;
			starRightResults.positionInMeters = new Vector2 ((594 + 140 - 746 / 2f) / 50f, -(247 - 668 / 2f) / 50f);
			StartCoroutine (MoveGUIObject (starRightResults.positionInMeters
			                               , new Vector2 ((460 + 140 - 20 + 154 / 2f - 746 / 2f) / 50f, -(301 - 20 + 159 / 2f - 668 / 2f) / 50f)
			                               , starRightResults));
		}
		
	}
	
	private IEnumerator AddScore (int maxScore) {
		
		yield return new WaitForSeconds (1f);
		
		while (scoreResultsOn + (int) (maxScore /100f) < maxScore) {
			
			scoreResultsOn += (int) (maxScore /100f);
			scoreResults.text = scoreResultsOn.ToString ();
			
			scoreResults.positionInMeters = new Vector2 ((368 - 746/2f) / 50f  + 0.8f , -(456  - 668/2f) / 50f)+ new Vector2 (140 / 50f
			                                                                                                          - scoreResults.textObject.GetComponent < Renderer > ().bounds.max.x / 2f, 0) ;
			
			yield return new WaitForSeconds (0.02f);
		}
		
		scoreResults.text = maxScore.ToString ();
		StartCoroutine (AnimateStars ());
	}
	
	
	private IEnumerator MoveResults (int stars) {
		
		moveResultsIndex = 0;
		
		/*
		for (int i = 0; i <= 50; i++) {

			dragonResults.positionInMeters = new Vector2 ((100 - 746/2f) / 50f, -(668/2f  - 668/2f) / 50f) - new Vector2 ( ((50 - i) * 10 + 60) / 50f, 0);
			SetResultsPosition ( ((50 - i) * 10 + 140) / 50f, stars);
			yield return new WaitForSeconds (0.01f);
		}

		AnimateResults ();
*/
		yield return null;
	}
	
	
	private IEnumerator RotateLight () {
		
		while (true) {
			
			lightResults.gameObject.transform.rotation = Quaternion.Euler (new Vector3 (90,(Time.timeSinceLevelLoad * 20f) % 360,0));
			yield return new WaitForSeconds (0.01f);
		}
		
	}
	
	private void AnimateResults() {
		
		lightResults = new GUIImage ();
		lightResults.texture =ResourcesController.LoadCompressedTexture ("Interface/LevelCompleted/Light");
		lightResults.layer = -2.5f - 2f;
		lightResults.positionInMeters = new Vector2 ((746/2f - 746/2f) / 50f, -(668/2f  - 668/2f) / 50f);
		lightResults.sizeInMeters = new Vector2 (1800 / 50f, 1800 / 50f);
		
		StartCoroutine (AddScore (Settings.scoreEnded));
		StartCoroutine (RotateLight ());
	}
	
	private void SetResultsPosition (float delta, int stars) {
		
		backgroundResults.positionInMeters = new Vector2 (0, 0) + new Vector2 (delta, 0);
		signResults.positionInMeters = new Vector2 ((746/2f - 746/2f) / 50f, -(190 - 668/2f) / 50f) + new Vector2 (delta, 0);
		
		if (stars > 0) 
			starLeftResults.positionInMeters = new Vector2 ((164 - 20 + 154 / 2f - 746 / 2f) / 50f, -(301 - 20 + 159 / 2f - 668 / 2f) / 50f) + new Vector2 (delta, 0);
		
		if (stars > 1) 
			starCentreResults.positionInMeters = new Vector2 ((307 - 20 + 160 / 2f - 746 / 2f) / 50f, -(243 - 20 + 155 / 2f - 668 / 2f) / 50f) + new Vector2 (delta, 0);
		
		if (stars > 2) 
			starRightResults.positionInMeters = new Vector2 ((460 - 20 + 154 / 2f - 746 / 2f) / 50f, -(301 - 20 + 159 / 2f - 668 / 2f) / 50f) + new Vector2 (delta, 0);
		
		exitResults.positionInMeters = new Vector2 ((681 - 746/2f) / 50f, -(67  - 668/2f) / 50f) + new Vector2 (delta, 0);
		scoreResults.positionInMeters = new Vector2 ((368 - 746 / 2f) / 50f, -(456 - 668 / 2f) / 50f) + new Vector2 (delta - 0.45f, 0);//- new Vector2 (scoreResults.textObject.GetComponent < Renderer > ().bounds.max.x / 2f, 0);
		okResults.positionInMeters = new Vector2 ((746/2f - 746/2f) / 50f, -(581  - 668/2f) / 50f) + new Vector2 (delta, 0);
	}
	
	private void CreateResults (int score, int stars) {

        GooglePlayServicesController.ReportProgress ("Pass1Level", 100);

        isLevelEnding = true;
        musicWin = AudioController.instance.CreateAudio ("pobeda", false, true);

		blackResults = new GUIImage ();
		blackResults.texture =ResourcesController.LoadCompressedTexture ("Interface/Black");
		blackResults.layer = -5 ;
		blackResults.positionInMeters = new Vector2 (0, 0);
		blackResults.sizeInMeters = new Vector2 (1200 / 50f, 1200 / 50f)*1.5f;
		
		backgroundResults = new GUIImage ();
		backgroundResults.texture =ResourcesController.LoadCompressedTexture ("Interface/LevelCompleted/BackgroundCompleted");
		backgroundResults.layer = -3 ;
		backgroundResults.positionInMeters = new Vector2 (0, 0);
		backgroundResults.sizeInMeters = new Vector2 (746 / 50f, 668 / 50f);
		
		var signResultsTexture = (ResourcesController.LoadCompressedTexture ("$" + Settings.language + "/" + "Interface/LevelCompleted/LevelCompleted"));
		signResults = new GUIImage ();
		signResults.texture = signResultsTexture;
		signResults.layer = -2f;
		signResults.positionInMeters = new Vector2 ((746/2f - 746/2f) / 50f, -(226 - 668/2f) / 50f);
		signResults.sizeInMeters = new Vector2 (signResultsTexture.width / 50f, signResultsTexture.height / 50f);
		
		if (stars > 0) {
			starLeftResults = new GUIImage ();
			starLeftResults.texture =ResourcesController.LoadCompressedTexture ("Interface/LevelCompleted/StarLeftCompleted");
			starLeftResults.layer = -2f - 10 - 2f;
			starLeftResults.positionInMeters = new Vector2 ((164 - 20 + 154 / 2f - 746 / 2f) / 50f, -(301 - 20 + 159 / 2f - 668 / 2f) / 50f);
			starLeftResults.sizeInMeters = new Vector2 (154 / 50f, 159 / 50f);
		}
		
		if (stars > 1) {
			starCentreResults = new GUIImage ();
			starCentreResults.texture =ResourcesController.Load ("Interface/LevelCompleted/StarCentreCompleted") as Texture;
			starCentreResults.layer = -2f - 10 - 2f;
			starCentreResults.positionInMeters = new Vector2 ((307 - 20 + 160 / 2f - 746 / 2f) / 50f, -(243 - 20 + 155 / 2f - 668 / 2f) / 50f);
			starCentreResults.sizeInMeters = new Vector2 (160 / 50f, 155 / 50f);
		}
		
		if (stars > 2) {
			starRightResults = new GUIImage ();
			starRightResults.texture =ResourcesController.LoadCompressedTexture ("Interface/LevelCompleted/StarRightCompleted");
			starRightResults.layer = -2f - 10 - 2f;
			starRightResults.positionInMeters = new Vector2 ((460 - 20 + 154 / 2f - 746 / 2f) / 50f, -(301 - 20 + 159 / 2f - 668 / 2f) / 50f);
			starRightResults.sizeInMeters = new Vector2 (154 / 50f, 159 / 50f);
		}
		
		exitResults = new GUIButton ();
		exitResults.texture =ResourcesController.LoadCompressedTexture ("Interface/LevelCompleted/Exit");
		exitResults.layer = -0.5f;
		exitResults.positionInMeters = new Vector2 ((681 - 746/2f) / 50f, -(67  - 668/2f) / 50f);
		exitResults.sizeInMeters = new Vector2 (64 / 50f, 50 / 50f);
		exitResults.OnClick = () => {
			
            AudioController.instance.CreateAudio ("click");
			RemoveResults ();
		};
		exitResults.OnButtonDown = () => {
			
			exitResults.texture =ResourcesController.LoadCompressedTexture ("Interface/LevelCompleted/ExitSelected");
		};
		exitResults.OnButtonUp = () => {
			
			exitResults.texture =ResourcesController.LoadCompressedTexture ("Interface/LevelCompleted/Exit");
		};
		
		scoreResults = new GUIImage ();
		scoreResults.layer = 0f - 2f;
		scoreResults.sizeInMeters = new Vector2 (0,0);
		scoreResults.textTransform = GUIController.CreateText (ref scoreResults.textObject);
		scoreResults.deltaTextPosition = new Vector2 (0, 0);
		scoreResults.textScale = new Vector2 (0.16f, 0.16f);
		scoreResults.text = "0";
		scoreResults.positionInMeters = new Vector2 ((368 - 746/2f) / 50f, -(306  - 668/2f) / 50f);
		scoreResults.positionInMeters -= new Vector2 (scoreResults.textObject.GetComponent < Renderer > ().bounds.max.x / 2f, 0);
		
		
		dragonResults = new GUIImage ();
		dragonResults.texture =ResourcesController.LoadCompressedTexture ("Interface/LevelCompleted/Dragon");
		dragonResults.layer = -2f;
		dragonResults.positionInMeters = new Vector2 ((-50 - 746/2f) / 50f, -(668/2f  - 668/2f) / 50f);
		dragonResults.sizeInMeters = new Vector2 (439 / 50f, 579 / 50f);
		
		
		okResults = new GUIButton ();
		okResults.texture =ResourcesController.LoadCompressedTexture ("Interface/LevelCompleted/Ok");
		okResults.layer = -2f;
		okResults.positionInMeters = new Vector2 ((746/2f - 746/2f) / 50f, -(581  - 668/2f) / 50f);
		okResults.sizeInMeters = new Vector2 (198 / 50f, 96 / 50f);
		okResults.OnClick = () => {
			
            AudioController.instance.CreateAudio ("click");

			RemoveResults ();
		};
		
		
		scoreResultsOn = 0;
		
	}
	
	private void RemoveResults () {
		
		if (backgroundResults == null)
			return;

        AudioController.instance.RemoveAudio (musicWin);
		StopAllCoroutines ();
		
		backgroundResults.Destroy ();
		signResults.Destroy ();
        
        if (starLeftResults != null)
		    starLeftResults.Destroy ();

        if (starCentreResults != null)
		    starCentreResults.Destroy ();

        if (starRightResults != null)
		    starRightResults.Destroy ();

		exitResults.Destroy ();
		scoreResults.Destroy ();
		dragonResults.Destroy ();
		lightResults.Destroy ();
		blackResults.Destroy ();
		okResults.Destroy ();
		
		backgroundResults = null;
		signResults = null;
		starLeftResults = null;
		starCentreResults = null;
		starRightResults = null;
		exitResults = null;
		scoreResults = null;
		lightResults = null;
		dragonResults = null;
		blackResults = null;
		okResults = null;

		CreateSpam ();
	}


	private void CreateSpam () {

        RemoveSpam ();
        /*
		spam = new GUIButton ();
		spam.texture =ResourcesController.LoadCompressedTexture ("Interface/Spam");
		spam.layer = 0;
		spam.positionInMeters = new Vector2 (0, 0);
		spam.sizeInMeters = new Vector2 (1200 / 50f, 675 / 50f);
		spam.OnClick = () => {
			
            AudioController.instance.CreateAudio ("click");
			RemoveSpam ();
		};
        */

	}

	private void RemoveSpam () {

		if (currentStars == 3) {

			Settings.frescoIndex = -1;
			ScenePassageController.instance.LoadScene ("FrescoScene");
		} else {

			ScenePassageController.instance.LoadScene ("SelectLevelScene");
		}

		//spam.Destroy ();
	}

    private static void CreateExitConfirmMenu (GameController.Action onEnd) {

        if (backgroundExitConfirm != null)
            return;
		
		backgroundExitConfirm = new GUIImage ();
		backgroundExitConfirm.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/ExitConfirm/BackgroundLevel");
		backgroundExitConfirm.layer = -2;
		backgroundExitConfirm.positionInMeters = new Vector2 (0, 0) + CameraController.cameraPosition;
		backgroundExitConfirm.sizeInMeters = new Vector2 (513 / 50f, 346 / 50f);

		yesExitConfirm  = new GUIButton ();
		yesExitConfirm.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/ExitConfirm/Yes");
		yesExitConfirm.layer = 0;
		yesExitConfirm.positionInMeters = new Vector2 ((131 - 513 / 2f) / 50f, -(220 - 346 / 2f) / 50f) + CameraController.cameraPosition;
		yesExitConfirm.sizeInMeters = new Vector2 (238 / 50f, 207 / 50f);
		yesExitConfirm.OnButtonDown = () => {
			
			yesExitConfirm.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/ExitConfirm/YesPushed");
		};
		yesExitConfirm.OnButtonUp = () => {
			
			//yesExitConfirm.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/StartButton");
		};
		yesExitConfirm.OnClick = () => {

            RemoveExitConfirmMenu ();
            onEnd ();
        };

		noExitConfirm  = new GUIButton ();
		noExitConfirm.texture =ResourcesController.LoadCompressedTexture ("$"+Settings.language+"/"+"Interface/ExitConfirm/No");
		noExitConfirm.layer = 0;
		noExitConfirm.positionInMeters = new Vector2 ((390 - 513 / 2f) / 50f, -(220 - 346 / 2f) / 50f) + CameraController.cameraPosition;
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

        if (backgroundExitConfirm == null)
            return;

		backgroundExitConfirm.Destroy ();
		yesExitConfirm.Destroy ();
		noExitConfirm.Destroy ();

        backgroundExitConfirm = null;
	}



	void FixedUpdate() {

		GUIController.Update (Time.fixedDeltaTime);
	
		if (!isGameStarted) 
			return;

        if (isPaused)
            return;

        if (foundCount != maxToFind) {

		    time += Time.fixedDeltaTime;

		    if (skillCooldownTime > 0) {
			    skillCooldownTime -= Time.fixedDeltaTime;
		    } else {

			    skillCooldownTime = 0;
		    }
        }

		if (isCameraMinimizing) {

			SlideController.ResizeCamera (1.01f * CameraController.widthInMeters);
			if (SlideController.mapWidth  + interfaceWidth - 0.01f <= CameraController.widthInMeters || SlideController.mapHeight - 0.01f <= CameraController.heightInMeters) {

				isCameraMinimizing = false;
                
                afterCameraMinimized ();
			}
		}

		if (moveResultsIndex <= 50) {

			
			dragonResults.positionInMeters = new Vector2 ((100 - 746/2f) / 50f, -(668/2f  - 668/2f) / 50f) - new Vector2 ( ((50 - moveResultsIndex) * 10 + 60) / 50f, 0);
			SetResultsPosition ( ((50 - moveResultsIndex) * 10 + 140) / 50f, Settings.starsEnded);
			
			if (moveResultsIndex == 50) {
				AnimateResults ();
			}
			
			moveResultsIndex ++;
		}
	}

	void Update () {
		
		if (!isGameStarted) 
			return;

        if (!isPaused) {

		    deltaMisslickTime -= Time.deltaTime;
		    deltaObjectFoundTime -= Time.deltaTime;

		    if (deltaMisslickTime <= 0 && missclickCount < 5)
			    missclickCount = 0;

		    AnimationController.Update(Time.deltaTime);
        }

        if (isLevelEnding) {
            
            if (Vector2.Distance (CameraController.cameraPosition, new Vector2 ((746/2f - 746/2f) / 50f, -(668/2f  - 668/2f) / 50f)) > 0.1f) {

                CameraController.cameraPosition += (-CameraController.cameraPosition + new Vector2 ((746/2f - 746/2f) / 50f, -(668/2f  - 668/2f) / 50f))
                    .normalized * 0.2f;
            }
        }

		SlideController.frictionDelta = CameraController.widthInMeters/Screen.width;

		if (!isCameraMinimizing) 
			SlideController.SlideControll();
		
		if (platform == Platform.PC) {
			
			if (Input.GetMouseButtonUp(0)) {
				
				GUIController.OnClick(Input.mousePosition);
				GameController.OnButtonUp(Input.mousePosition);
			}
			
			if (Input.GetMouseButtonDown(0)) 
				GameController.OnButtonDown(Input.mousePosition);

		}
		
		if (platform == Platform.Phone) {
			if (Input.GetKeyDown(KeyCode.Escape))
				ScenePassageController.instance.LoadScene("SelectLevelScene");
		}


	}
}
