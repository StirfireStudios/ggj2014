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
	public float turn = 0;
	public string characterName;
	
	DialogueMachine baseMachine;
	TweeCharacter tweeChar;
	
	TextMesh text;
	Animator anim;
	
	void Start()
	{
		string[] names = TweeTree.Instance.getCharacterNames();
		foreach (string name in names)
		{
			Debug.Log(name);
		}

		tweeChar = TweeTree.Instance.getCharacter(characterName);
		if (tweeChar == null)
		{
			Debug.LogWarning("Could not find character named "+characterName);
			Destroy(this);
			return;
		}
		else
		{
			TweeCharacter player = TweeTree.Instance.getCharacter("Jessie");
			if (player == null)
			{
				Debug.Log("Couldn't find player");
			}
			baseMachine = new DialogueMachine(tweeChar.getStartFor(player));
		}

		text = GetComponentInChildren<TextMesh>();
		updateText();

		MessagePasser.subscribe("game-tick", OnTick);

		anim = GetComponent<Animator>();
		anim.SetBool("sit", sitting);
		anim.SetFloat("turning", turn);
	}

	public void OnTick(string message, Hashtable args)
	{
		baseMachine.Advance();
		updateText();
		anim.SetBool(Animator.StringToHash("sit"), sitting);
	}

	void updateText()
	{
		if (baseMachine.CurrentNode == null)
		{
			return;
		}
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
