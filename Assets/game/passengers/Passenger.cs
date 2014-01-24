using UnityEngine;
using System.Collections;

public class Passenger : MonoBehaviour
{
	public string messageText;

	void OnTriggerEnter(Collider other)
	{
		DialogueDisplay.ShowText(messageText);
	}

	void OnTriggerExit(Collider other)
	{
		DialogueDisplay.HideText();
	}
}
