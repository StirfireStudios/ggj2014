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
			Tick();
		}

		if (Input.GetKeyDown(KeyCode.Period))
		{
			if (!alreadyAdvanced)
			{
				Tick();
			}
			alreadyAdvanced = true;
		}
		else
		{
			alreadyAdvanced = false;
		}
	}

	void Tick()
	{
		MessagePasser.send("game-tick", null);
		timer = 0;
		tickCount++;
		if (tickCount == ticksToEndGame - 1)
		{
			MessagePasser.send("penultimate-tick", null);
		}
		if (tickCount >= ticksToEndGame)
		{
			Debug.Log("Time end");
			MessagePasser.send("time-end", null);
			tickCount = 0;
		}
	}
}
