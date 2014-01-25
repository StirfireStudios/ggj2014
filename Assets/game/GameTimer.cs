using UnityEngine;
using System.Collections;

public class GameTimer : MonoBehaviour
{
	Rect advanceRect;
	bool alreadyAdvanced = false;

	void Awake()
	{
		float inset = 8;
		float size = 64;
		advanceRect = new Rect(Screen.width - size - inset, inset, size, size);
	}

	void OnGUI()
	{
		if (GUI.Button(advanceRect, ">") || Input.GetKeyDown(KeyCode.Period))
		{
			if (!alreadyAdvanced)
			{
				MessagePasser.send("game-tick", null);
			}
			alreadyAdvanced = true;
		}
		else
		{
			alreadyAdvanced = false;
		}
	}
}
