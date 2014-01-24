using UnityEngine;
using System.Collections;

public class DialogueDisplay : MonoBehaviour
{
	static DialogueDisplay _instance;
	static DialogueDisplay Instance
	{
		get
		{
			if (_instance == null)
			{
				new GameObject("Dialogue Display", typeof(DialogueDisplay));
			}
			return _instance;
		}
	}

	Rect displayRect;
	string currentText;

	void Awake()
	{
		if (_instance != null)
		{
			Destroy(gameObject);
			return;
		}
		_instance = this;

		float inset = 16;
		float height = Screen.height / 4;

		displayRect = new Rect(inset, Screen.height - inset - height, Screen.width - inset*2, height);
	}

	void OnGUI()
	{
		if (currentText != null)
		{
			GUI.skin.box.wordWrap = true;
			GUI.skin.box.fontSize = 32;
			GUI.Box(displayRect, currentText);
		}
	}

	public static void ShowText(string text)
	{
		Instance.currentText = text;
	}

	public static void HideText()
	{
		Instance.currentText = null;
	}
}
