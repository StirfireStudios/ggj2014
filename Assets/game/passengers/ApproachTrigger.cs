using UnityEngine;
using System.Collections;

public class ApproachTrigger : MonoBehaviour
{
	public string conversationName;

	void OnCollisionEnter()
	{
		MessagePasser.send("approach-start", conversationName);
	}

	void OnCollisionExit()
	{
		MessagePasser.send("approach-end", conversationName);
	}
}
