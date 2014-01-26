using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

	public float MaxCameraTilt = 80.0f;
	public float JoyStickCameraPanSensitivity = 3.0f;
	public float MouseCameraPanSensitivity = 10.0f;

	public float JoyStickCameraTiltSensitivity = 3.0f;
	public float MouseCameraTiltSensitivity = 10.0f;

	void Awake() {
		MessagePasser.subscribe("controller-status", OnControllerStatus);
		MessagePasser.subscribe("camera", OnCameraMessage);
	}

	// Use this for initialization
	void Start () {
	
	}

	void tiltCameraBy(float amount) {
		_cameraTilt += amount;
		_cameraTilt = Mathf.Clamp (_cameraTilt, -1 * MaxCameraTilt, MaxCameraTilt);
		transform.eulerAngles = new Vector3(_cameraTilt, 0);
		Debug.Log ("Camera Tilt: " + _cameraTilt);
	}

	void panCameraBy(float amount) {
		_cameraPan += amount;
		_cameraPan = _cameraPan % 360.0f;
		Debug.Log ("Camera Pan: " + _cameraPan);
		transform.eulerAngles = new Vector3(0, _cameraPan);
	}

	// Update is called once per frame
	void Update () {
		if (!_enabled) {
			// Reset all stuffs to zero.
			return;
		}

		Vector3 vcamera;

		if (_controller) {
			tiltCameraBy(Input.GetAxis("RightVertical") * JoyStickCameraTiltSensitivity);
			panCameraBy(Input.GetAxis("RightHorizontal") * JoyStickCameraPanSensitivity);
		} else {
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {

			} else {
				tiltCameraBy(-1 * Input.GetAxis("Mouse Y") * MouseCameraTiltSensitivity);
				panCameraBy(-1 * Input.GetAxis("Mouse X") * MouseCameraPanSensitivity);
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

	private float _cameraTilt = 0.0f;
	private float _cameraPan = 0.0f;
	private bool _controller = false;
	private bool _enabled = true;
}
