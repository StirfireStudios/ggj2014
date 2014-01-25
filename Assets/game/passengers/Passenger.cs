using UnityEngine;
using System.Collections;
using Twee;

public class Passenger : MonoBehaviour
{
	public string characterName;

	DialogueMachine baseMachine;

	void Awake()
	{
		TweeNode startNode = TweeTree.Instance.getNode("CFC1 John1-1");
		baseMachine = new DialogueMachine(startNode);
	}

	void OnTriggerEnter(Collider other)
	{
		DialogueDisplay.ShowText(baseMachine.Text);
	}

	void OnTriggerExit(Collider other)
	{
		DialogueDisplay.HideText();
	}
}
