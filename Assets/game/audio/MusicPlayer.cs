using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour {

	public bool enable = true;
	public float fadedelay;
	public float fadeamount;

	IEnumerator fadeout(AudioSource clip) {
		while (clip.volume > 0) {
			clip.volume = clip.volume - fadeamount;
			yield return new WaitForSeconds(fadedelay);
		}
		clip.Pause();
	}

	IEnumerator fadein(AudioSource clip) {
		_current = clip;
		clip.volume = 0;
		clip.Play();
		while (clip.volume < 1.0f) {
			clip.volume = clip.volume + fadeamount;
			yield return new WaitForSeconds(fadedelay);
		}
		clip.volume = 1.0f;
	}

	void Awake() {
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
			enable = true;
		}
	}

	// Use this for initialization
	void Start () {
		_clips = new Dictionary<string, AudioSource>();
		foreach(AudioSource player in GetComponentsInChildren<AudioSource>()) {
			if (player.name != "Music") {
				_clips[player.name] = player;
			}
		}

		MessagePasser.subscribe("music-start", OnStart);
		MessagePasser.subscribe("music-stop", OnStop);	
	}

	private string getFilename(string arg){
		return arg + Player.Instance.characterName;
	}

	public void OnStart(string message, object arg) {
		if (!enable)
			return;

		string file = getFilename((string)arg);
		if (_clips.ContainsKey(file)) {
			if (_current != null) {
				StartCoroutine("fadein", _clips[file]);
			} else {
				_clips[file].Play();
				_current = _clips[file];
			}
		} else {
			Debug.LogWarning("MusicPlayer: Could not find audio file: " + file);
		}
	}
	
	public void OnStop(string message, object arg) {
		string file = getFilename((string)arg);
		if (_clips.ContainsKey(file)) {
			StartCoroutine("fadeout", _clips[file]);
		} else {
			Debug.LogWarning("MusicPlayer: Could not find audio file: " + file);
		}
	}

	// Update is called once per frame
	void Update () {

	}
	
	private Dictionary<string, AudioSource> _clips;
	private AudioSource _current;
}
