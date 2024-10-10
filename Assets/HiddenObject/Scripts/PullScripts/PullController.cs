using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PullController {

	private static Vector3 deadPosition = new Vector3 (0,-10,0);
	private static Vector3 deadRotation = new Vector3 (90,0,0);

	private static Dictionary < string, List <GameObject> > pull = new Dictionary<string, List<GameObject>>();
	private static List <GameObject> tempGameObjectList;
	private static GameObject tempGameObject;


	public static void Create() {

		var newPull = new Dictionary<string, List<GameObject>>();

        foreach (var list in pull) {

            if (list.Key.Contains ("Undestroyable"))
                newPull.Add(list.Key, list.Value);
        }

        pull = newPull;
	}

	public static void RemoveObjects(string type) {

		if (pull.ContainsKey(type)) {
			pull[type].Clear();
		}
	}

	public static GameObject GetObject(string type) {

		if (pull.TryGetValue(type, out tempGameObjectList)) {
			if (tempGameObjectList.Count > 0) {
				tempGameObject = tempGameObjectList[tempGameObjectList.Count-1]; 
				tempGameObjectList.Remove(tempGameObject);
				tempGameObject.SetActive(true);
				return tempGameObject;
			}
		}

		return null;
	}

	public static void AddObject(string type, GameObject gameObject) {
        
        if (gameObject == null)
            return;

		gameObject.transform.position = deadPosition;

        if (gameObject.GetComponent <Renderer> ()) {

            var color = gameObject.GetComponent <Renderer> ().material.color;
		    gameObject.GetComponent <Renderer> ().material.color = new Color (color.r, color.g, color.b, 0.99f);
        }
		gameObject.transform.rotation = Quaternion.Euler (deadRotation);
		gameObject.SetActive(false);

		if (pull.TryGetValue(type, out tempGameObjectList)) {
			tempGameObjectList.Add(gameObject);
		} else {
			pull[type] = new List<GameObject> ();
			pull[type].Add(gameObject);
		}
	}

}
