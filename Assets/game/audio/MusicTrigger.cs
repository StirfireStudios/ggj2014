using UnityEngine;
using System.Collections;

public class MusicTrigger : MonoBehaviour {
	public string trackName;
	
	void OnTriggerEnter() {
		MessagePasser.send("music-start", trackName);
	}
	
	void OnTriggerExit() {
		MessagePasser.send("music-stop", trackName);
	}
}
