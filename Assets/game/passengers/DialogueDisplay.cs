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
	TweeLink[] currentLinks;

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
		if (currentLinks != null)
		{
			string choiceMade = null;
			for (int i = 0; i < currentLinks.Length; i++)
			{
				Rect button = new Rect(10, Screen.height - 74 * (i+1), Screen.width - 20, 64);
				if (GUI.Button(button, currentLinks[i].Text))
				{
					choiceMade = currentLinks[i].NodeName;
				}
			}
			if (choiceMade != null)
			{
				MessagePasser.send("player-choice", choiceMade);
			}
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

	public static void ShowOptions(TweeLink[] links)
	{
		Instance.currentLinks = links;
		HideText();
	}

	public static void HideOptions()
	{
		Instance.currentLinks = null;
	}
}
