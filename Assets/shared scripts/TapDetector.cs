using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TapDetector : MonoBehaviour
{
	static TapDetector _instance;
	static TapDetector Instance
	{
		get
		{
			if (_instance == null)
			{
				new GameObject("TapDetector", typeof(TapDetector));
			}
			return _instance;
		}
	}

	public delegate bool TapCallback(Vector2 location);

	public float tapMaxTime = 1/5f;
	public float tapMaxDistance = 0.01f;

	float touchTimer;
	Vector2? touchStart;
	Vector2? lastTouch;

	List<TapCallback> subscriberCallbacks;
	List<int> subscriberPriorities;

	void Awake()
	{
		if (_instance != null)
		{
			Destroy(gameObject);
			return;
		}
		_instance = this;

		subscriberCallbacks = new List<TapCallback>();
		subscriberPriorities = new List<int>();
	}

	void Update()
	{
		Vector2? currentTouch = InputAbstract.getTouch();
		if (touchStart == null)
		{
			if (currentTouch != null)
			{
				//start touch
				touchStart = currentTouch;
				lastTouch = currentTouch;
				touchTimer = 0;
			}
		}
		else
		{
			touchTimer += Time.deltaTime;
			//check for touch end
			if (currentTouch == null)
			{
				//check for tap
				if (touchTimer <= tapMaxTime)
				{
					Vector2 offset = (Vector2)lastTouch - (Vector2)touchStart;
					if (offset.sqrMagnitude <= tapMaxDistance*tapMaxDistance)
					{
						//was a tap, so broadcast
						Broadcast((Vector2)lastTouch);
					}
				}
				//reset touch
				touchStart = null;
			}
			else
			{
				lastTouch = currentTouch;
			}
		}
	}

	public static void Subscribe(TapCallback callback, int priority)
	{
		//add callback and priority
		Instance.subscriberCallbacks.Add(callback);
		Instance.subscriberPriorities.Add(priority);
		//bubble down by priority
		for(int i = Instance.subscriberCallbacks.Count-1; i > 0; i--)
		{
			if (Instance.subscriberPriorities[i] > Instance.subscriberPriorities[i-1])
			{
				//bubble down
				int swapP = Instance.subscriberPriorities[i-1];
				Instance.subscriberPriorities[i-1] = Instance.subscriberPriorities[i];
				Instance.subscriberPriorities[i] = swapP;
				TapCallback swapC = Instance.subscriberCallbacks[i-1];
				Instance.subscriberCallbacks[i-1] = Instance.subscriberCallbacks[i];
				Instance.subscriberCallbacks[i] = swapC;
			}
			else
			{
				//sorted, so end
				return;
			}
		}
	}

	public static void Unsubscribe(TapCallback callback)
	{
		int index = Instance.subscriberCallbacks.IndexOf(callback);
		if (index >= 0)
		{
			Instance.subscriberCallbacks.RemoveAt(index);
			Instance.subscriberPriorities.RemoveAt(index);
		}
	}

	void Broadcast(Vector2 location)
	{
		foreach (TapCallback callback in Instance.subscriberCallbacks)
		{
			if (callback(location))
			{
				//subscriber consumed the tap, so stop braodcasting
				return;
			}
		}
	}
}
