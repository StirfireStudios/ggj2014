using UnityEngine;
using System.Collections.Generic;
using System;

public class TweeCharacter {
	public string Name { get { return _name; } }

	public TweeNode getStartFor(TweeCharacter character) {
		if (_start.ContainsKey(character)) {
			return _start[character];
		}
		return null;
	}

	public TweeNode getApproachFor(TweeCharacter character) {
		if (_approachStart.ContainsKey(character)) {
			return _approachStart[character];
		}
		return null;
	}

	public TweeNode getDepartFor(TweeCharacter character) {
		if (_departStart.ContainsKey(character)) {
			return _departStart[character];
		}
		return null;
	}

	public void addStartFor(TweeCharacter character, TweeNode node) {
		if (_start.ContainsKey(character)) {
			Debug.Log("Warning: multiple Starts for " + character.Name + " to " + this._name + " Previous node: " + _start[character].Name + " New Node: " + node.Name);
			return;
		}
		_start[character] = node;
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
		_start = new Dictionary<TweeCharacter, TweeNode>();
		_approachStart = new Dictionary<TweeCharacter, TweeNode>();
		_departStart = new Dictionary<TweeCharacter, TweeNode>();
	}

	private string _name;
	private Dictionary <TweeCharacter, TweeNode> _start;
	private Dictionary <TweeCharacter, TweeNode> _approachStart;
	private Dictionary <TweeCharacter, TweeNode> _departStart;
}
