using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

	public float MaxCameraTilt = 90.0f;
	public float JoyStickCameraPanSensitivity = 1.0f;
	public float MouseCameraPanSensitivity = 15.0f;
	
	void Awake() {
		MessagePasser.subscribe("controller-status", OnControllerStatus);
		MessagePasser.subscribe("camera", OnCameraMessage);
	}

	// Use this for initialization
	void Start () {
	
	}

	void moveCameraBy(float amount) {
		_cameraPanY += Input.GetAxis("RightVertical") * JoyStickCameraPanSensitivity;
		Mathf.Clamp (_cameraPanY, -MaxCameraTilt, MaxCameraTilt);

		Debug.Log ("Transform by: " + _cameraPanY);
		transform.eulerAngles = new Vector3(_cameraPanY, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (!_enabled) {
			// Reset all stuffs to zero.
			return;
		}

		Vector3 vcamera;

		if (_controller) {
			moveCameraBy(Input.GetAxis("RightVertical") * JoyStickCameraPanSensitivity);
		} else {
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {

			} else {
				Debug.Log ("Mouse: " + Input.GetAxis("Mouse Y") * MouseCameraPanSensitivity);
	           moveCameraBy(Input.GetAxis("Mouse Y") * MouseCameraPanSensitivity);
			}
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
