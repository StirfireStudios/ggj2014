using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueMachine
{
	TweeNode currentNode;
	int currentSection = 0;
	bool awaitingPlayer = false;

	public DialogueMachine(TweeNode startNode)
	{
		if (startNode == null)
		{
			Debug.LogWarning("Making machine from null start node");
			return;
		}
		currentNode = startNode;

		MessagePasser.subscribe("player-choice", OnPlayerChoice);
	}

	public void OnPlayerChoice(string message, string arg)
	{
		if (currentNode == null)
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
				DialogueDisplay.ShowText(link.Text);
			}
		}
	}

	public void Advance()
	{
		if (currentNode == null || awaitingPlayer)
		{
			return;
		}
		currentSection++;
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
					Debug.Log("Starting player choice. Base text: "+link.Text);
					//player links
					DialogueDisplay.ShowOptions(currentNode.Links);
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
				return null;
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
