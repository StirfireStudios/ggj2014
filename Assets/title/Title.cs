using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

	void OnGUI () {
		float middleX = Screen.width / 2;
		float middleY = Screen.height / 2;
		float width = 150;
		float height = 100;
		if (GUI.Button (new Rect (middleX - width/2,middleY - height/2 - 10,width,height), "Begin")) {
			Application.LoadLevel ("game");
		}
		if (GUI.Button (new Rect (middleX - width/2,middleY - height/2 + height + 5,width,height), "Credits")) {
			Application.LoadLevel ("credits");
		}
	}
}
