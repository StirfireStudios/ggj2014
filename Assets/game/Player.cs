using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public static Player Instance;

	public string characterName;

	IEnumerator CheckForControllers() {
		while(true) {
			string[] controllers = Input.GetJoystickNames();
			if (!_controller && controllers.Length > 0) {
				_controller = true;
				MessagePasser.send("controller-status", "connected");
			} else if (_controller && controllers.Length == 0) {
				_controller = false;
				MessagePasser.send("controller-status", "disconnected");
			}
			yield return new WaitForSeconds(1f);
		}
	}
	
	void Awake() {
		Instance = this;
	}

	void Start() {
		StartCoroutine(CheckForControllers());
	}

	private bool _controller = false;
}
