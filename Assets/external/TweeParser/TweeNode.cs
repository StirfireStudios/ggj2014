using UnityEngine;
using System.Collections.Generic;
using System;

public class TweeNode {
	[Flags]
	enum NodeFlags {
		DialogNode = 0x01, // This is a dialog node
		StartApproach = 0x02, // This dialog starts when the player approaches
		StartLeave = 0x04, // This dialog starts when the player leaves
		Start = 0x08, // this is the start node for this character's conversation
		EventNode = 0x80 // this is an event node
	};

	public string Name {
		get {
			return _name;
		}
	}

	public string[] Tags {
		get {
			return _tags;
		}
	}

	public TweeLink[] Links {
		get {
			return _links;
		}
	}

	public TweeNodeSection[] Sections {
		get {
			return _sections;
		}
	}

	public TweeCharacter Speaker {
		get {
			return _speaker;
		}
	}

	public TweeCharacter Target {
		get {
			return _target;
		}
	}

	public TweeCharacter Player {
		get {
			return _player;
		}

		set {
			_player = value;
		}
	}

	private bool hasFlags(NodeFlags flags) {
		return (_flags & flags) == flags;
	}

	public bool isDialog { get { return hasFlags(NodeFlags.DialogNode); } }
	public bool isDialogStart { get { return hasFlags(NodeFlags.Start); } }
	public bool isDialogForPlayerApproach { get { return hasFlags(NodeFlags.StartApproach); } }
	public bool isDialogForPlayerDepart { get { return hasFlags(NodeFlags.StartLeave); } }


	public bool isEvent { get { return hasFlags(NodeFlags.EventNode); } }

	private string getTextInBrackets(string text) {
		return getTextInBrackets(text, false);
	}

	private string getTextInBrackets(string text, bool twobrackets) {
		text = text.Trim ();
		int index = text.IndexOf("[");
		if (twobrackets)
			return text.Substring(index+2, text.Length - (index + 4)).Trim();
		else
			return text.Substring(index+1, text.Length - (index + 2)).Trim();
	}

	public TweeNode(string text, Dictionary <string, TweeCharacter> characters) {
		_flags = 0;
		string[] lines = text.Split(new string[] {"\n"}, System.StringSplitOptions.None);
		if (lines[0].Contains("[")) {
			int index = lines[0].IndexOf("[");
			_name = lines[0].Substring(2, index - 2).Trim();
			_tags = getTextInBrackets(lines[0]).Split(new string[] {" "}, System.StringSplitOptions.RemoveEmptyEntries);
			foreach(string tag in _tags) {
				if (String.Equals(tag, "Start:Approach", StringComparison.OrdinalIgnoreCase)) {
					_flags |= NodeFlags.StartApproach;
				} else if (String.Equals(tag, "Start:Leave", StringComparison.OrdinalIgnoreCase)) {
					_flags |= NodeFlags.StartLeave;
				} else if (String.Equals(tag, "StartNode", StringComparison.OrdinalIgnoreCase)) {
					_flags |= NodeFlags.Start;
				} else if (String.Equals(tag, "Event", StringComparison.OrdinalIgnoreCase)) {
					_flags |= NodeFlags.EventNode;
				} else if (tag.StartsWith("S:") || tag.StartsWith("s:")) {
					_speaker = TweeTree.findOrAddCharacter(tag, characters);
				} else if (tag.StartsWith("T:") || tag.StartsWith("t:")) {
					_target = TweeTree.findOrAddCharacter(tag, characters);
				} else if (tag.StartsWith("P:") || tag.StartsWith("p:")) {
					_player = TweeTree.findOrAddCharacter(tag, characters);
				}
			} 
			if (!isDialog && !isEvent) {
				_flags |= NodeFlags.DialogNode;
			}

/*			string output = "Node is: ";
			if (isDialog)
				output += "dialog, ";
			if (isDialogStart)
				output += "start of conversation, ";
			if (isDialogForPlayerApproach)
				output += "for the player approaching, ";
			if (isDialogForPlayerLeave)
				output += "for the player leaving, ";

			Debug.Log(output.Substring(0, output.Length - 2));*/
		} else {
			_name = lines[0].Substring(2).Trim();
		}

		System.Text.StringBuilder bodytext = new System.Text.StringBuilder();
		List<TweeLink> links = new List<TweeLink>();
		List<TweeNodeSection> sections = new List<TweeNodeSection>();
		for(int i = 1; i < lines.Length; i++) {
			if (lines[i].Contains("[")) {
				TweeLink link = new TweeLink(getTextInBrackets(lines[i], true));
				links.Add (link);
			} else {
				if (lines[i].StartsWith("--") && bodytext.Length > 0) {
					sections.Add(new TweeNodeSection(bodytext.ToString()));
					bodytext = new System.Text.StringBuilder();
				}
				bodytext.AppendLine(lines[i]);
			}
		}
		if (bodytext.Length > 0) {
			sections.Add(new TweeNodeSection(bodytext.ToString()));
		}

		_sections = sections.ToArray();

		if (links.Count > 0) {
			_links = links.ToArray();
		} else {
			_links = new TweeLink[0];
		}
	}

	private TweeNodeSection[] _sections;
	private string _name;
	private NodeFlags _flags;
	private string[] _tags;
	private TweeCharacter _speaker;
	private TweeCharacter _target;
	private TweeCharacter _player;
	private TweeLink[] _links;
}