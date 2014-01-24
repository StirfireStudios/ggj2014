using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessagePasser : MonoBehaviour
{
	private static MessagePasser _instance;
	private static MessagePasser Instance
	{
		get
		{
			if (_instance == null)
			{
				new GameObject("MessagePasser", typeof(MessagePasser));
			}
			return _instance;
		}
	}

	public delegate void messageHandler(string messageType, Hashtable args);
	private Dictionary<string, List<messageHandler>> subscribers;

	void Awake()
	{
		subscribers = new Dictionary<string, List<messageHandler>>();
		_instance = this;
	}

	public static void subscribe(string messageType, messageHandler handler)
	{
		var subscribers = Instance.subscribers;
		if (!subscribers.ContainsKey(messageType))
		{
			subscribers.Add(messageType, new List<messageHandler>());
		}
		subscribers[messageType].Add(handler);
	}

	public static void unsubscribe(string messageType, messageHandler handler)
	{
		var subscribers = Instance.subscribers;
		if (!subscribers.ContainsKey(messageType))
		{
			Debug.LogWarning("Unsubscribing from "+messageType+" when no subcribers");
			return;
		}
		bool success = subscribers[messageType].Remove(handler);
		if (!success)
		{
			Debug.LogWarning("Failed to unsubscribe from "+messageType);
		}
	}

	public static void send(string messageType, Hashtable args)
	{
		var subscribers = Instance.subscribers;
		if (!subscribers.ContainsKey(messageType))
		{
			return;
		}
		foreach (messageHandler handler in subscribers[messageType])
		{
			handler(messageType, args);
		}
	}
}
