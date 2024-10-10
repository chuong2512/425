using UnityEngine;
using System.Collections;

public class ScenePassageController : MonoBehaviour {

	public static ScenePassageController instance = null;

	private static bool isAlphaIncreasing;
	private string sceneToLoad = "";
	private static float alphaSpeed = 1f;
	private static float maxAlpha = 1f;

    private static GameController.Action onEnd;

	public static void OnSceneLoaded () {

        alphaSpeed = 1f;
		isAlphaIncreasing = false;

        onEnd = () => {

        };
	}

	public void LoadScene (string scene, GameController.ActionCallback beforeEnd = null) {

        alphaSpeed = 1f;
        maxAlpha = 1f;

        gameObject.GetComponent <Renderer> ().material.color = new Color (0,0,0,0);
        
		gameObject.transform.position = new Vector3 (0,GUIController.layer + 0.5f,0);
		sceneToLoad = scene;
		isAlphaIncreasing = true;
        
        if (beforeEnd == null) {

            beforeEnd = (a) => { 
                a();
            };
        }

        onEnd = () => {
            

            beforeEnd (() => {
                
                AudioController.instance.ClearSounds ();
			    Application.LoadLevel (sceneToLoad);
			    sceneToLoad = "";
            });
        };

	}

    
	public void Pause (float layer, GameController.Action onOver) {

        alphaSpeed = 2f;
        maxAlpha = 0.88f;

		gameObject.transform.position = new Vector3 (0,GUIController.layer + layer,0);
		isAlphaIncreasing = true;

        sceneToLoad = "Pause";

        onEnd = () => {
            
            onOver ();
			sceneToLoad = "Paused";
        };

	}
    
	public void UnPause (GameController.Action onOver = null) {
       
        if (sceneToLoad != "Paused" && sceneToLoad != "Pause")
            return;

        alphaSpeed = 2f;
		isAlphaIncreasing = false;

        
        onEnd = () => {

            if (onOver != null)
                onOver ();
        };

	}


	// Use this for initialization
	void Start () {

		if (instance != null) {

			Destroy(gameObject);
			return;
		}

		instance = this;
		DontDestroyOnLoad (gameObject);
		isAlphaIncreasing = false;
		gameObject.transform.position = new Vector3 (0,19.5f,0);
        onEnd = () => { };
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		
		if (gameObject.GetComponent <Renderer> ().material.color.a >= maxAlpha && (sceneToLoad != "" && sceneToLoad != "Paused")) {

            gameObject.GetComponent <Renderer> ().material.color = new Color (0, 0, 0, maxAlpha);
            onEnd ();
		}
		
		if (isAlphaIncreasing && gameObject.GetComponent <Renderer> ().material.color.a < maxAlpha) {

			gameObject.GetComponent <Renderer> ().material.color += new Color (0,0,0,Time.fixedDeltaTime*alphaSpeed);

		}
		
		if (!isAlphaIncreasing && gameObject.GetComponent <Renderer> ().material.color.a > 0) {
			
			gameObject.GetComponent <Renderer> ().material.color -= new Color (0,0,0,Time.fixedDeltaTime*alphaSpeed);

			if (gameObject.GetComponent <Renderer> ().material.color.a <= 0) {

                onEnd ();
				gameObject.transform.position = new Vector3 (0,-10,0);
			}
		}

		                                                                                   
			

	}
}
