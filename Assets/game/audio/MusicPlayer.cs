using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour {

	public float fadeTime = 1;

	IEnumerator fadeout(AudioSource clip) {
		float delta = 1 / fadeTime;
		while (clip.volume > 0) {
			clip.volume = clip.volume - delta * Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		clip.Pause();
	}

	IEnumerator fadein(AudioSource clip) {
		_current = clip;
		clip.volume = 0;
		clip.Play();
		float delta = 1 / fadeTime;
		while (clip.volume < 1.0f) {
			clip.volume = clip.volume + delta * Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		clip.volume = 1.0f;
	}

	// Use this for initialization
	void Start ()
	{
		_clips = new Dictionary<string, AudioSource>();
		foreach(AudioSource player in GetComponentsInChildren<AudioSource>()) {
			if (player.name != "Music") {
				_clips[player.name] = player;
			}
		}

		MessagePasser.subscribe("music-start", OnStart);
		MessagePasser.subscribe("music-stop", OnStop);
		MessagePasser.subscribe("time-end", OnTimeEnd);
	}

	private string getFilename(string arg){
		return arg + Player.Instance.characterName;
	}

	public void OnStart(string message, string arg) {
		string file = getFilename(arg);
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
	
	public void OnStop(string message, string arg) {
		string file = getFilename(arg);
		if (_clips.ContainsKey(file)) {
			StartCoroutine("fadeout", _clips[file]);
		} else {
			Debug.LogWarning("MusicPlayer: Could not find audio file: " + file);
		}
	}


	public void OnTimeEnd(string message, string arg)
	{
		StartCoroutine(characterChange());
	}

	IEnumerator characterChange()
	{
		yield return new WaitForEndOfFrame();
		foreach (string key in _clips.Keys)
		{
			if (!key.Contains(Player.Instance.characterName))
			{
				StartCoroutine("fadeout", _clips[key]);
			}
		}
	}
	
	private Dictionary<string, AudioSource> _clips;
	private AudioSource _current;
}
