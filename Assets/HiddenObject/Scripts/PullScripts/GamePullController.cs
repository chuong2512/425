using UnityEngine;
using System.Collections;

public class GamePullController {

	private static GameObject tempGameObject;

	public static void Create() {

		PullController.Create();
	}

	public static GameObject CreateUnit() {

		tempGameObject = PullController.GetObject("Unit");
		if (tempGameObject == null) {
			tempGameObject = GameObject.Instantiate(Resources.Load("Prefabs/Unit") as GameObject) as GameObject;
		}
		return tempGameObject;
	}
	
	
	public static GameObject CreateTower() {
		
		tempGameObject = PullController.GetObject("Tower");
		if (tempGameObject == null) {
			tempGameObject = GameObject.Instantiate(Resources.Load("Prefabs/Unit") as GameObject) as GameObject;
			tempGameObject.name = "Tower";
		}
		return tempGameObject;
	}
	
	public static GameObject CreateTowerPlace() {
		
		tempGameObject = PullController.GetObject("TowerPlace");
		if (tempGameObject == null) {
			tempGameObject = GameObject.Instantiate(Resources.Load("Prefabs/Unit") as GameObject) as GameObject;
			tempGameObject.name = "TowerPlace";
		}
		return tempGameObject;
	}
	
	public static GameObject CreateButton() {
		
		tempGameObject = null;//= PullController.GetObject("GUIButton");
		if (tempGameObject == null) {
			tempGameObject = GameObject.Instantiate(Resources.Load("Prefabs/Unit") as GameObject) as GameObject;
			tempGameObject.name = "GUIButton";
		}
		return tempGameObject;
	}
	
	public static GameObject CreateImage() {
		
		tempGameObject = PullController.GetObject("Image");
		if (tempGameObject == null) {
			tempGameObject = GameObject.Instantiate(Resources.Load("Prefabs/Unit") as GameObject) as GameObject;
			tempGameObject.name = "Image";
		}
		return tempGameObject;
	}
	
	public static GameObject CreateImageAlpha() {
		
		tempGameObject = PullController.GetObject("ImageAlpha");
		if (tempGameObject == null) {
			tempGameObject = GameObject.Instantiate(Resources.Load("Prefabs/UnitAlpha") as GameObject) as GameObject;
			tempGameObject.name = "ImageAlpha";
		}
		return tempGameObject;
	}
	
	
	public static GameObject CreateMissile() {
		
		tempGameObject = PullController.GetObject("Missile");
		if (tempGameObject == null) {
			tempGameObject = GameObject.Instantiate(Resources.Load("Prefabs/Unit") as GameObject) as GameObject;
			tempGameObject.name = "Missile";
		}
		return tempGameObject;
	}
	
	
	public static GameObject CreateHealthBarRed() {
		
		tempGameObject = PullController.GetObject("HealthBarRed");
		if (tempGameObject == null) {
			tempGameObject = GameObject.Instantiate(Resources.Load("Prefabs/HealthBarRed") as GameObject) as GameObject;
			tempGameObject.name = "HealthBarRed";
		}
		return tempGameObject;
	}
	
	public static GameObject CreateHealthBarGreen() {
		
		tempGameObject = PullController.GetObject("HealthBarGreen");
		if (tempGameObject == null) {
			tempGameObject = GameObject.Instantiate(Resources.Load("Prefabs/HealthBarGreen") as GameObject) as GameObject;
			tempGameObject.name = "HealthBarGreen";
		}
		return tempGameObject;
	}
	
	public static GameObject CreateShadow() {
		
		tempGameObject = PullController.GetObject("Shadow");
		if (tempGameObject == null) {
			tempGameObject = GameObject.Instantiate(Resources.Load("Prefabs/Shadow") as GameObject) as GameObject;
			tempGameObject.name = "Shadow";
		}
		return tempGameObject;
	}

	public static GameObject CreateAudio() {
		
		tempGameObject = PullController.GetObject("AudioUndestroyable");
		if (tempGameObject == null) {
			tempGameObject = GameObject.Instantiate(Resources.Load("Prefabs/Audio") as GameObject) as GameObject;
			tempGameObject.name = "Audio";
		}
		return tempGameObject;
	}


	public static void DestroyUnit(GameObject gameObject) {

		PullController.AddObject("Unit",gameObject);
	}
	
	public static void DestroyButton(GameObject gameObject) {
		
		PullController.AddObject("GUIButton",gameObject);
	}

	public static void DestroyImage(GameObject gameObject) {
		
		PullController.AddObject("Image",gameObject);
	}

	public static void DestroyImageAlpha(GameObject gameObject) {
		
		PullController.AddObject("ImageAlpha",gameObject);
	}
	
	public static void DestroyTower(GameObject gameObject) {
		
		PullController.AddObject("Tower",gameObject);
	}
	
	public static void DestroyTowerPlace(GameObject gameObject) {
		
		PullController.AddObject("TowerPlace",gameObject);
	}
	
	public static void DestroyMissile(GameObject gameObject) {
		
		PullController.AddObject("Missile",gameObject);
	}
	
	public static void DestroyHealthBarRed(GameObject gameObject) {
		
		PullController.AddObject("HealthBarRed",gameObject);
	}
	
	public static void DestroyHealthBarGreen(GameObject gameObject) {
		
		PullController.AddObject("HealthBarGreen",gameObject);
	}
	
	public static void DestroyShadow(GameObject gameObject) {
		
		PullController.AddObject("Shadow",gameObject);
	}
	
	public static void DestroyAudio(GameObject gameObject) {
		
		PullController.AddObject("AudioUndestroyable",gameObject);
	}


}
