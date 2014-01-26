using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

	static Dictionary<string, Passenger> lookup;

	public bool sitting = false;
	public float turn = 0;
	public string characterName;
	public string conversationName;

	TweeCharacter tweeChar;
	DialogueMachine baseMachine;
	DialogueMachine approachMachine;
	DialogueMachine currentMachine;
	
	TextMesh text;
	Animator anim;

	bool inApproach = false;
	float currentTurn;
	float targetTurn;

	void Awake()
	{
		if (lookup == null)
		{
			lookup = new Dictionary<string, Passenger>();
		}
		lookup.Add(characterName, this);
	}
	
	void Start()
	{
		text = GetComponentInChildren<TextMesh>();

		MessagePasser.subscribe("game-tick", OnTick);
		MessagePasser.subscribe("approach-start", OnApproach);
		MessagePasser.subscribe("approach-end", OnApproach);
		MessagePasser.subscribe("time-end", OnTimeEnd);

		anim = GetComponent<Animator>();
		Reset();
	}

	void Reset()
	{
		inApproach = false;

		anim.SetBool("sit", sitting);
		anim.SetFloat("turning", turn);
		currentTurn = turn;
		targetTurn = turn;
		
		tweeChar = TweeTree.Instance.getCharacter(characterName);
		if (tweeChar == null)
		{
			Debug.LogWarning("Could not find character named "+characterName);
			Destroy(this);
			return;
		}
		else
		{
			TweeCharacter player = TweeTree.Instance.getCharacter(Player.Instance.characterName);
			if (player == null)
			{
				Debug.Log("Couldn't find player");
			}
			baseMachine = new DialogueMachine(tweeChar.getStartFor(player), "base", characterName);
			currentMachine = baseMachine;
			
			TweeNode approachNode = tweeChar.getApproachFor(player);
			if (approachNode == null)
			{
				Debug.Log("No approach node found for "+tweeChar.Name);
			}
			else
			{
				setupApproach(approachNode);
				foreach (TweeCharacter ass in tweeChar.Associated)
				{
					Passenger pass = lookup[ass.Name];
					pass.setupApproach(approachNode);
				}
			}
		}
		
		updateText();
	}

	public void setupApproach(TweeNode start)
	{
		approachMachine = new DialogueMachine(start, "approach", characterName);
	}

	public void OnApproach(string message, string arg)
	{
		if (arg != conversationName || approachMachine == null)
		{
			return;
		}

		if (message == "approach-start")
		{
			Debug.Log("Starting approach for "+arg);
			inApproach = true;
			currentMachine = approachMachine;
			updateText();
			MessagePasser.send("player-stop", null);
		}
		else
		{
			Debug.Log("Ending approach for "+characterName);
			inApproach = false;
			approachMachine = null;
			currentMachine = baseMachine;
			DialogueDisplay.HideText();
			DialogueDisplay.HideOptions();
		}
	}

	public void OnTick(string message, string arg)
	{
		if (currentMachine != null)
		{
			if (currentMachine.CurrentNode != null && currentMachine.CurrentNode.Speaker != null)
			{
				if (currentMachine.CurrentNode.Speaker.Name == Player.Instance.characterName)
				{
					DialogueDisplay.HideText();
				}
			}
			currentMachine.Advance();
			if (inApproach)
			{
				if (approachMachine.Finished)
				{
					inApproach = false;
					approachMachine = null;
					currentMachine = baseMachine;
					DialogueDisplay.HideText();
					DialogueDisplay.HideOptions();
				}
			}
		}

		updateText();
	}

	void Update()
	{
		currentTurn = Mathf.Lerp(currentTurn, targetTurn, 0.1f);
		anim.SetFloat("turning", currentTurn);
	}

	void updateText()
	{
		if (currentMachine == null || currentMachine.CurrentNode == null)
		{
			targetTurn = turn;
			return;
		}
		TweeNode node = currentMachine.CurrentNode;
		TweeCharacter speaker = node.Speaker;

		if (speaker != null && Player.Instance.characterName == speaker.Name)
		{
			DialogueDisplay.ShowText(currentMachine.Text);
			if (node.Target != null)
			{
				Player.PointAt(lookup[node.Target.Name].transform);
			}
		}

		if (speaker != null && tweeChar.Name == speaker.Name)
		{
			text.text = wrapLine(currentMachine.Text);

			if (inApproach)
			{
				Player.PointAt(transform);
			}

			if (node.Target != null)
			{
				Transform target = null;
				if (node.Target.Name == Player.Instance.characterName)
				{
					target = Player.Instance.transform;
				}
				else if (lookup.ContainsKey(node.Target.Name))
				{
					target = lookup[node.Target.Name].transform;
				}

				if (target != null)
				{
					Vector3 offset = target.position - transform.position;
					offset = transform.InverseTransformDirection(offset);
					Quaternion look = Quaternion.LookRotation(offset);
					float angle = look.eulerAngles.y - 90;
					if (angle > 180)
					{
						angle -= 360;
					}
					targetTurn = angle / 90;
				}
			}
		}
		else
		{
			targetTurn = turn;
			text.text = "";
		}
	}

	void OnTimeEnd(string message, string arg)
	{
		Reset();
	}

	public static Transform getPasenger(string name)
	{
		return lookup[name].transform;
	}
}
