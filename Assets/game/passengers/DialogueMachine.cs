using UnityEngine;
using System.Collections;
using Twee;

public class DialogueMachine
{
	TweeNode currentNode;
	bool paused = false;

	public DialogueMachine(TweeNode startNode)
	{
		currentNode = startNode;
	}

	public void OnTick()
	{
		if (paused)
		{
			return;
		}

		TweeLink link = currentNode.Links[1];
		currentNode = link.Node;
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
