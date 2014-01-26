using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

	public float MaxCameraPanY = 90.0f;
	public float CameraPanSensitivity = 1.0f;
	
	void Awake() {
		MessagePasser.subscribe("controller-status", OnControllerStatus);
		MessagePasser.subscribe("camera", OnCameraMessage);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!_enabled) {
			// Reset all stuffs to zero.
			return;
		}

		if (_controller) {
			_cameraPanY += Input.GetAxis("RightVertical") * CameraPanSensitivity;

			if (_cameraPanY < -MaxCameraPanY) {
				_cameraPanY = -MaxCameraPanY;
			} else if (_cameraPanY > MaxCameraPanY) {
				_cameraPanY = MaxCameraPanY;
			}

			Vector3 vcamera = new Vector3(_cameraPanY, 0);

			if (camera != null) {
				Debug.Log ("Transform Camera!");
				transform.eulerAngles = vcamera;
			}
		} else {
			// Mouse keyboard / touchscreen
		}
	}

	public void OnControllerStatus(string message, string arg) {
		if (arg == "connected")
			_controller = true;
		else if (arg == "disconnected")
			_controller = false;
	}

	public void OnCameraMessage(string message, string arg) {
		if (arg == "disable")
			_enabled = false;
		else if (arg == "enable")
			_enabled = true;
	}

	private float _cameraPanY = 0.0f;
	private bool _controller = false;
	private bool _enabled = true;
}
