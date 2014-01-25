using UnityEngine;
using System.Collections;
using System.Text;

public class Passenger : MonoBehaviour
{

	const int speechWidth = 25;

	private string wrapLine(string s)
	{
		string[] words = s.Split(' ');

		StringBuilder builder = new StringBuilder();
		int currentLength = 0;
		foreach (string current in words)
		{
			int l = current.Length;
			if (currentLength + l > speechWidth)
			{
				builder.Append("\n");
				currentLength = 0;
			}
			builder.Append(current);
			builder.Append(' ');
			currentLength += l;
		}
		return builder.ToString();
	}

	public bool sitting = false;
	public string characterName;
	
	DialogueMachine baseMachine;
	TweeCharacter tweeChar;
	
	TextMesh text;
	
	void Awake()
	{
		tweeChar = TweeTree.Instance.getCharacter(characterName);
		if (tweeChar == null)
		{
			Debug.LogWarning("Could not find character named "+characterName);
			Destroy(this);
			return;
		}
		else
		{
			baseMachine = new DialogueMachine(tweeChar.Start);
		}

		text = GetComponentInChildren<TextMesh>();
		updateText();

		MessagePasser.subscribe("game-tick", OnTick);
	}

	public void OnTick(string message, Hashtable args)
	{
		baseMachine.Advance();
		updateText();
	}

	void updateText()
	{
		TweeNode node = baseMachine.CurrentNode;
		TweeCharacter speaker = node.Speaker;
		if (tweeChar.Name == speaker.Name)
		{
			text.text = wrapLine(baseMachine.Text);
		}
		else
		{
			text.text = "";
		}
	}
}
