using UnityEngine;
using System.Collections;

public class TweeNodeSection {
	public int Delay { get { return _delay; } }
	public string Text { get { return _text; } }

	public TweeNodeSection(string data) {
		int startLine = 0;
		_delay = 1;
		string[] lines = data.Split(new string[] {"\n"}, System.StringSplitOptions.None);
		if ((lines[0].Length > 1) && lines[0].StartsWith("--")) {
			startLine = 1;
			System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(lines[0].Substring(2), @"[\d]+");
			if (matches.Count > 0) {
				string temp = "";
				foreach(System.Text.RegularExpressions.Match match in matches) {
					temp += match.ToString();
				}
				_delay = int.Parse(temp);
			}
		}
		System.Text.StringBuilder text = new System.Text.StringBuilder();
		for (int i = startLine; i < lines.Length; i++) {
			text.AppendLine(lines[i]);
		}
		_text = text.ToString().Trim();
	}

	private int _delay;
	private string _text;
}
