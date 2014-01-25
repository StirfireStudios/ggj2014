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

	DialogueMachine baseMachine;
	TweeCharacter tweeChar;
	DialogueMachine approachMachine;
	
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

		anim = GetComponent<Animator>();
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
			TweeCharacter player = TweeTree.Instance.getCharacter(Player.Instance.charcterName);
			if (player == null)
			{
				Debug.Log("Couldn't find player");
			}
			baseMachine = new DialogueMachine(tweeChar.getStartFor(player));
			
			TweeNode approachNode = tweeChar.getApproachFor(player);
			if (approachNode == null)
			{
				Debug.Log("No approach node found for "+tweeChar.Name);
			}
			else
			{
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
		approachMachine = new DialogueMachine(start);
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
			updateText();
		}
		else
		{
			Debug.Log("Ending approach for "+arg);
			inApproach = false;
			approachMachine = null;
		}
	}

	public void OnTick(string message, string arg)
	{
		if (inApproach)
		{
			if (approachMachine != null)
			{
				approachMachine.Advance();
				if (approachMachine.Finished)
				{
					Debug.Log("Ending approach for "+arg);
					inApproach = false;
					approachMachine = null;
				}
			}
		}
		if (!inApproach)
		{
			if (baseMachine != null)
			{
				baseMachine.Advance();
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
		if (inApproach)
		{
			if (approachMachine == null || approachMachine.CurrentNode == null)
			{
				targetTurn = turn;
				return;
			}
		}
		else
		{
			if (baseMachine == null || baseMachine.CurrentNode == null)
			{
				targetTurn = turn;
				return;
			}
		}
		TweeNode node = baseMachine.CurrentNode;
		if (inApproach)
		{
			node = approachMachine.CurrentNode;
		}
		TweeCharacter speaker = node.Speaker;
		if (speaker != null && tweeChar.Name == speaker.Name)
		{
			text.text = wrapLine(baseMachine.Text);

			if (node.Target != null)
			{
				Transform target = null;
				if (node.Target.Name == "Player")
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
}
