using UnityEngine;
using System.Collections.Generic;
using System;

public class TweeLink {
	public string NodeName {
		get {
			return _stringNode;
		}
	}
	
	public TweeNode Node {
		get {
			return _node;
		}
		set {
			_node = value;
		}
	}

	public string Text {
		get {
			return _text;
		}
	}

	public TweeLink (string text) {
		string[] parts = text.Split(new string[] {"|"}, StringSplitOptions.None);
		if (parts.Length > 1) {
			_text = parts[0];
			_stringNode = parts[1];
		} else {
			_stringNode = parts[0];
		}
	}

	private string _text;
	protected string _stringNode;
	protected TweeNode _node;
}
