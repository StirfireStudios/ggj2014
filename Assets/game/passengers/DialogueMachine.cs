using UnityEngine;
using System.Collections;
using Twee;

public class DialogueMachine
{
	TweeNode currentNode;

	public DialogueMachine(TweeNode startNode)
	{
		if (startNode == null)
		{
			Debug.LogWarning("Making machine from null start node");
		}
		currentNode = startNode;
	}

	public void Advance()
	{
		if (currentNode != null && currentNode.Links.Length > 0)
		{
			TweeLink link = currentNode.Links[0];
			currentNode = link.Node;
		}
		else
		{
			currentNode = null;
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
			return currentNode.Text;
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
