using UnityEngine;
using System.Collections.Generic;
using System;

namespace Twee {
	public class TweeTree : MonoBehaviour {

		static public TweeTree Instance {
			get {
				if (_instance == null)
					new GameObject("TweeTree", typeof(TweeTree));
				return _instance;
			}
		}

		public void Awake() {
			if (_instance != null && _instance != this)
				Destroy(gameObject);
			_instance = this;
			DontDestroyOnLoad(gameObject);
			_nodes = new Dictionary<string, TweeNode>();

			TextAsset[] scripts = Resources.LoadAll<TextAsset>("Story");
			foreach(TextAsset script in scripts) {
				ParseAsset(script);
			}

			foreach(TweeNode node in _nodes.Values) {
				foreach(TweeLink link in node.Links) {
					if (_nodes.ContainsKey(link.NodeName)) {
						link.Node = _nodes[link.NodeName];						
					} else {
						Debug.Log("WARNING: \""+node.Name+"\" contains a link to '" + link.NodeName + "' that couldn't be resolved");
					}
				}
			}
		}

		private void ParseAsset(TextAsset script) {
			string[] lines = script.text.Split(new string[] {"\n"}, System.StringSplitOptions.None);
			System.Text.StringBuilder nodetext = new System.Text.StringBuilder();
			foreach(string line in lines) {
				if (line.StartsWith("::")) {
					if (nodetext.Length > 0) {
						ParseNode(nodetext.ToString());
						nodetext = new System.Text.StringBuilder();
					}
				}
				nodetext.AppendLine(line);
			}
			if (nodetext.Length > 0)
				ParseNode(nodetext.ToString());
		}

		private void ParseNode(string text) {
			TweeNode node = new TweeNode(text);
			if (_nodes.ContainsKey(node.Name))
				Debug.Log("WARNING: We already have a node called \""+node.Name+"\"");
			else 
				_nodes.Add(node.Name, node);
		}

		public TweeNode getNode(string name) {
			return _nodes[name];
		}

		public bool nodeExists(string name) {
			return _nodes.ContainsKey(name);
		}

		private static TweeTree _instance;
		private Dictionary<string, TweeNode> _nodes;
		private Dictionary<string, TweeCharacter> _characters;
	}
}