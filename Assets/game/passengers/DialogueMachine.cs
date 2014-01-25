using UnityEngine;
using System.Collections;

public class DialogueMachine
{
	TweeNode currentNode;
	int currentSection = 0;

	public DialogueMachine(TweeNode startNode)
	{
		if (startNode == null)
		{
			Debug.LogWarning("Making machine from null start node");
			return;
		}
		currentNode = startNode;
	}

	public void Advance()
	{
		if (currentNode == null)
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
				currentNode = link.Node;
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
}
