using UnityEngine;
using System.Collections;

public class ApproachTrigger : MonoBehaviour
{
	public string conversationName;

	void OnTriggerEnter()
	{
		MessagePasser.send("approach-start", conversationName);
	}

	void OnTriggerExit()
	{
		Debug.Log("Player left "+conversationName);
		MessagePasser.send("approach-end", conversationName);
	}
}
