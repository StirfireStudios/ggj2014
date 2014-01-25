using UnityEngine;
using System.Collections.Generic;
using System;

public class TweeCharacter {
	public string Name { get { return _name; } }

	public TweeNode Start { get { return _start; } set { _start = value; } }

	public TweeNode getApproachFor(TweeCharacter character) {
		if (_approachStart.ContainsKey(character)) {
			return _approachStart[character];
		}
		return null;
	}

	public void addAppoachFor(TweeCharacter character, TweeNode node) {
		if (_approachStart.ContainsKey(character)) {
			Debug.Log("Warning: multiple Start:Approaches for " + character.Name + " to " + this._name + " Previous node: " + _approachStart[character].Name + " New Node: " + node.Name);
			return;
		}
		_approachStart[character] = node;
	}

	public void addDepartFor(TweeCharacter character, TweeNode node) {
		if (_departStart.ContainsKey(character)) {
			Debug.Log("Warning: multiple Start:Leave for " + character.Name + " to " + this._name + " Previous node: " + _approachStart[character].Name + " New Node: " + node.Name);
			return;
		}
		_departStart[character] = node;
	}

	public TweeCharacter (string name) {
		_name = name;
		_approachStart = new Dictionary<TweeCharacter, TweeNode>();
		_departStart = new Dictionary<TweeCharacter, TweeNode>();
	}

	private string _name;
	private TweeNode _start;
	private Dictionary <TweeCharacter, TweeNode> _approachStart;
	private Dictionary <TweeCharacter, TweeNode> _departStart;
}
