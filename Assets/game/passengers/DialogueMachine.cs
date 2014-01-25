using UnityEngine;
using System.Collections;
using Twee;

public class DialogueMachine
{
	TweeNode currentNode;
	bool paused = false;

	public DialogueMachine(TweeNode startNode)
	{
		if (startNode == null)
		{
			Debug.LogWarning("Making machine from null start node");
		}
		currentNode = startNode;

		MessagePasser.subscribe("game-tick", OnTick);
	}

	public void OnTick(string message, Hashtable args)
	{
		if (paused)
		{
			return;
		}

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

	public void Pause()
	{
		paused = true;
	}

	public void Resume()
	{
		paused = false;
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
