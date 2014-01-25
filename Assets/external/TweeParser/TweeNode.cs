using UnityEngine;
using System.Collections.Generic;
using System;

public class TweeNode {

	public string Name {
		get {
			return name;
		}
	}

	public TweeNode(string text) {
		string[] lines = text.Split(new string[] {"\n"}, System.StringSplitOptions.None);
		name = lines[0].Substring(2).Trim();

	}

	private string name;

}