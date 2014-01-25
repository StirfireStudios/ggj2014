using UnityEngine;
using System.Collections;
using Twee;

public class Passenger : MonoBehaviour
{
	public string characterName;

	DialogueMachine baseMachine;

	void Awake()
	{
		tweeChar = TweeTree.Instance.getCharacter(characterName);
		if (tweeChar == null)
		{
			Debug.LogWarning("Could not find character named "+characterName);
			Destroy(this);
		}
		else
		{
			baseMachine = new DialogueMachine(tweeChar.Start);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		DialogueDisplay.ShowText(baseMachine.Text);
	}

	void OnTriggerExit(Collider other)
	{
		DialogueDisplay.HideText();
	}

	private TweeCharacter tweeChar;
}
