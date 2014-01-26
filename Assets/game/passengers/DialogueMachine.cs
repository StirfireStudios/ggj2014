using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueMachine
{
	TweeNode currentNode;
	int currentSection = 0;
	int currentDelay = 0;
	bool awaitingPlayer = false;
	string typeName;
	string charName;

	public DialogueMachine(TweeNode startNode, string typeName, string charName)
	{
		this.typeName = typeName;
		this.charName = charName;
		if (startNode == null)
		{
			Debug.LogWarning("Making machine from null start node");
			return;
		}
		currentNode = startNode;

		MessagePasser.subscribe("player-choice", OnPlayerChoice);
		MessagePasser.subscribe("approach-end", OnApproachEnd);
	}

	public void OnPlayerChoice(string message, string arg)
	{
		if (currentNode == null || !awaitingPlayer)
		{
			return;
		}
		foreach (TweeLink link in currentNode.Links)
		{
			if (link.NodeName == arg)
			{
				Debug.Log("Player chose "+link.Text);
				awaitingPlayer = false;
				currentSection = 0;
				currentNode = link.Node;
				DialogueDisplay.HideOptions();
				DialogueDisplay.ShowText(currentNode.Sections[0].Text);
				if (currentNode.Target != null)
				{
					Player.PointAt(Passenger.getPasenger(currentNode.Target.Name));
				}
			}
		}
	}

	public void OnApproachEnd(string message, string arg)
	{
		awaitingPlayer = false;
	}

	public void Advance()
	{
		if (currentNode == null || awaitingPlayer)
		{
			return;
		}
		TweeNodeSection section = currentNode.Sections[currentSection];
		if (currentDelay > section.Delay)
		{
			currentSection++;
			currentDelay = 0;
		}
		else
		{
			currentDelay++;
		}
		if (currentSection >= currentNode.Sections.Length)
		{
			currentSection = 0;
			if (currentNode.Links.Length > 0)
			{
				TweeLink link = currentNode.Links[0];
				if (link.Text == null || link.Text == "")
				{
					//default link
					currentNode = link.Node;
				}
				else
				{
					//player links
					DialogueDisplay.ShowOptions(currentNode.Links);
					if (currentNode.Target != null)
					{
						Player.PointAt(Passenger.getPasenger(currentNode.Speaker.Name));
					}
					awaitingPlayer = true;
				}
			}
			else
			{
				currentNode = null;
			}
		}
	}

	public string Text
	{
		get
		{
			if (currentNode == null)
			{
				return "";
			}
			if (currentNode.Speaker != null && currentNode.Speaker.Name != Player.Instance.characterName)
			{
			    if (currentDelay < currentNode.Sections[currentSection].Delay)
				{
					return "";
				}
			}
			return currentNode.Sections[currentSection].Text;
		}
	}

	public TweeNode CurrentNode
	{
		get
		{
			return currentNode;
		}
	}

	public bool Finished
	{
		get
		{
			return currentNode == null;
		}
	}
}
