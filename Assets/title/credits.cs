using UnityEngine;
using System.Collections;

public class credits : MonoBehaviour {

	public void OnGUI () {
		float width = 150;
		float height = 100;

		float x = Screen.width - width;
		float y = Screen.height - height;
		if (GUI.Button (new Rect (x,y,width,height), "Main Menu")) {
			Application.LoadLevel ("title");
		}
	}

	public void Awake() {
		TextAsset creditsFile = Resources.Load<TextAsset>("credits");
		text = GetComponent<GUIText>();
		text.richText = true;
		text.text = creditsFile.text;
		starty = -text.GetScreenRect().height / 2;
		starty = starty - Screen.height / 2;
		text.pixelOffset = new Vector2(0, starty);
		maxy = text.GetScreenRect().height / 2 + Screen.height / 2;
	}

	public void Update() {
		Vector2 offset = text.pixelOffset;
		offset.y += 2;
		if (text.pixelOffset.y > maxy) {
			offset.y = starty;
		}
		text.pixelOffset = offset;
	}

	private float maxy, starty;
	private GUIText text;
}
