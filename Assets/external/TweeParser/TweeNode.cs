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

	public string Text {
		get {
			return _text;
		}
	}

	private bool hasFlags(NodeFlags flags) {
		return (_flags & flags) == flags;
	}

	public bool isDialog { get { return hasFlags(NodeFlags.DialogNode); } }
	public bool isDialogStart { get { return hasFlags(NodeFlags.Start); } }
	public bool isForPlayerApproach { get { return hasFlags(NodeFlags.StartApproach); } }
	public bool isForPlayerLeave { get { return hasFlags(NodeFlags.StartLeave); } }

	private string getTextInBrackets(string text) {
		return getTextInBrackets(text, false);
	}

	private string getTextInBrackets(string text, bool twobrackets) {
		int index = text.IndexOf("[");
		if (twobrackets)
			return text.Substring(index+2, text.Length - (index + 3)).Trim();
		else
			return text.Substring(index+1, text.Length - (index + 2)).Trim();
	}
	
	public TweeNode(string text) {
		string[] lines = text.Split(new string[] {"\n"}, System.StringSplitOptions.None);
		if (lines[0].Contains("[")) {
			int index = lines[0].IndexOf("[");
			_name = lines[0].Substring(2, index - 2).Trim();
			_tags = getTextInBrackets(lines[0]).Split(new string[] {" "}, System.StringSplitOptions.RemoveEmptyEntries);
		} else {
			_name = lines[0].Substring(2).Trim();
		}

		System.Text.StringBuilder bodytext = new System.Text.StringBuilder();
		List<TweeLink> links = new List<TweeLink>();
		for(int i = 1; i < lines.Length; i++) {
			if (lines[i].Contains("[")) {
				TweeLink link = new TweeLink(getTextInBrackets(lines[i]));
				links.Add (link);
			} else {
				bodytext.Append(lines[i]);
			}
		}
		if (bodytext.Length > 0) {
			_text = bodytext.ToString();
		} else {
			_text = "";
		}

		if (links.Count > 0) {
			_links = links.ToArray();
		} else {
			_links = new TweeLink[0];
		}
	}

	private string _text;
	private string _name;
	private NodeFlags _flags;
	private string[] _tags;
	private string _speaker;
	private string _target;
	private TweeLink[] _links;
}