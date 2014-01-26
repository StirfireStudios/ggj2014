using UnityEngine;
using System.Collections;

public class GameTimer : MonoBehaviour
{
	public float tickLength = 5;
	public int ticksToEndGame = 30;

	Rect advanceRect;
	bool alreadyAdvanced = false;

	float timer = 0;
	int tickCount = 0;

	void Awake()
	{
		float inset = 8;
		float size = 64;
		advanceRect = new Rect(Screen.width - size - inset, inset, size, size);
	}

	void Update()
	{
		timer = timer + Time.deltaTime;
		if (timer >= tickLength)
		{
			MessagePasser.send("game-tick", null);
			timer = timer - tickLength;
			tickCount++;
			if (tickCount >= ticksToEndGame)
			{
				MessagePasser.send("time-end", null);
				tickCount = 0;
			}
		}
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
