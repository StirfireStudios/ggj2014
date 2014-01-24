using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviourDisabler : MonoBehaviour
{
	static BehaviourDisabler _instance;
	public static BehaviourDisabler Instance
	{
		get
		{
			if (_instance == null)
			{
				new GameObject("BehaviourDisabler", typeof(BehaviourDisabler));
			}
			return _instance;
		}
	}

	Dictionary<string, List<Behaviour>> subscribers;

	void Awake()
	{
		_instance = this;
		subscribers = new Dictionary<string, List<Behaviour>>();
	}

	public static void Register(string channel, Behaviour target)
	{
		Dictionary<string, List<Behaviour>> subscribers = Instance.subscribers;
		if (!subscribers.ContainsKey(channel))
		{
			subscribers.Add(channel, new List<Behaviour>());
		}
		subscribers[channel].Add(target);
	}

	public static void Broadcast(string channel, bool state)
	{
		Dictionary<string, List<Behaviour>> subscribers = Instance.subscribers;
		if (!subscribers.ContainsKey(channel))
		{
			return;
		}
		foreach (Behaviour target in subscribers[channel])
		{
			if (target != null)
			{
				target.enabled = state;
			}
		}
	}
}
