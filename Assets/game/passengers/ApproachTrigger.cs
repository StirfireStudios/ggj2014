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
		MessagePasser.send("approach-end", conversationName);
	}
}
