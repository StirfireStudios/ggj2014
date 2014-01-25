using UnityEngine;
using System.Collections.Generic;
using System;

public class TweeTree : MonoBehaviour {

	static public TweeTree Instance {
		get {
			if (_instance == null)
				new GameObject("TweeTree", typeof(TweeTree));
			return _instance;
		}
	}

	static public TweeCharacter findOrAddCharacter(string tag, Dictionary <string, TweeCharacter> characters) {
		string[] sections = tag.Split(new string[] {":"}, System.StringSplitOptions.None);
		if (sections.Length > 1) 
			tag = sections[1];
		else
			tag = sections[0];
		if (characters.ContainsKey(tag)) {
			return characters[tag];
		} else {
			TweeCharacter character = new TweeCharacter(tag);
			characters[tag] = character;
			return character;
		}
	}

	public void Awake() {
		if (_instance != null && _instance != this)
			Destroy(gameObject);
		_instance = this;
		DontDestroyOnLoad(gameObject);
		_nodes = new Dictionary<string, TweeNode>();
		_characters = new Dictionary<string, TweeCharacter>();

		TextAsset[] scripts = Resources.LoadAll<TextAsset>("Story");
		foreach(TextAsset script in scripts) {
			ParseAsset(script);
		}

		foreach(TweeNode node in _nodes.Values) {
			// Link all the nodes
			foreach(TweeLink link in node.Links) {
				if (_nodes.ContainsKey(link.NodeName)) {
					link.Node = _nodes[link.NodeName];						
				} else {
					Debug.Log("WARNING: \""+node.Name+"\" contains a link to '" + link.NodeName + "' that couldn't be resolved");
				}
			}

			// search for character start and end nodes
			if (node.isDialogStart && node.Speaker != null) {
				_characters[node.Speaker.Name].Start = node;
			}

			if (node.isDialogForPlayerApproach && node.Speaker != null && node.Player != null) {
				_characters[node.Speaker.Name].addAppoachFor(node.Player, node);
			}

			if (node.isDialogForPlayerDepart && node.Speaker != null && node.Player != null) {
				_characters[node.Speaker.Name].addDepartFor(node.Player, node);
			}
		}

		foreach(TweeCharacter character in _characters.Values) {
			Debug.Log("Character: " + character.Name);
		}
	}

	private void ParseAsset(TextAsset script) {
		string[] lines = script.text.Split(new string[] {"\n"}, System.StringSplitOptions.None);
		System.Text.StringBuilder nodetext = new System.Text.StringBuilder();
		foreach(string line in lines) {
			if (line.StartsWith("::")) {
				if (nodetext.Length > 0) {
					ParseNode(nodetext.ToString(), script.name);
					nodetext = new System.Text.StringBuilder();
				}
			}
			nodetext.AppendLine(line);
		}
		if (nodetext.Length > 0)
			ParseNode(nodetext.ToString(), script.name);
	}

	private void ParseNode(string text, string filename) {
		TweeNode node = new TweeNode(text, _characters);
		if (_nodes.ContainsKey(node.Name))
			Debug.Log("WARNING: We already have a node called \""+node.Name+"\"");
		else 
			_nodes.Add(node.Name, node);

		if (node.Player == null) {
			if (filename.ToLower().Contains("jessie")) {
				node.Player = findOrAddCharacter("Jessie", _characters);
			} else if (filename.ToLower().Contains("don")) {
				node.Player = findOrAddCharacter("Don", _characters);
			}
		}
	}

	public TweeNode getNode(string name) {
		return _nodes[name];
	}

	public bool nodeExists(string name) {
		return _nodes.ContainsKey(name);
	}

	public TweeCharacter getCharacter(string name) {
		if (_characters.ContainsKey(name)) {
			return _characters[name];
		} else {
			return null;
		}
	}

	public string[] getCharacterNames() {
		string[] names = new string[_characters.Count];
		int index = 0;
		foreach(string key in _characters.Keys) {
			names[index] = key;
			index++;
		}
		return names;
	}

	private static TweeTree _instance;
	private Dictionary<string, TweeNode> _nodes;
	private Dictionary<string, TweeCharacter> _characters;
}