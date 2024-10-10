using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationController {
	
	private static List <ObjectAnimation> animations = new List<ObjectAnimation>();
	private static List <ObjectAnimation> animationsToRemove = new List<ObjectAnimation>();

	public static void Create() {

		animations = new List<ObjectAnimation>();
	}

	public static void Add(ObjectAnimation objectAnimation) {

		animations.Add(objectAnimation);
	}
	
	public static void Remove(ObjectAnimation objectAnimation) {

		animationsToRemove.Add(objectAnimation);
	}

	public static void Update (float deltaTime) {

		foreach (var animation in animations) {
			animation.Update(deltaTime);
		}

		foreach (var animation in animationsToRemove) {
			animations.Remove(animation);
		}

	}

}
