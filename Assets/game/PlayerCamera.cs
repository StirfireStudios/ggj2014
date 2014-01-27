using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {
	public const string CameraEnableChannel = "camera-enable";
	public const string CameraDisableMessage = "disable";
	public const string CameraEnableMessage = "enable";


	public float MaxCameraTilt = 80.0f;
	public float JoyStickCameraTiltSensitivity = 3.0f;
	public float MouseCameraTiltSensitivity = 10.0f;

	void Awake() {
		MessagePasser.subscribe("controller-status", OnControllerStatus);
		MessagePasser.subscribe(PlayerCamera.CameraEnableChannel, OnCameraEnableMessage);

	}

	// Use this for initialization
	void Start () {
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor) {
			_axis = "Joystick4";
		} 
	}

	void tiltCameraBy(float amount) {
		_cameraTilt += amount;
		_cameraTilt = Mathf.Clamp (_cameraTilt, -1 * MaxCameraTilt, MaxCameraTilt);
		transform.eulerAngles = new Vector3(_cameraTilt, 0);
	}

	void Update () {
		if (!_enabled) {
			// Reset all touch offsets and states to zero.
			return;
		}

		if (_controller) {
			tiltCameraBy(Input.GetAxis(_axis) * JoyStickCameraTiltSensitivity);
		} else {
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {

			} else {
				tiltCameraBy(-1 * Input.GetAxis("Mouse Y") * MouseCameraTiltSensitivity);
			}
		}
	}

	public void OnControllerStatus(string message, string arg) {
		if (arg == "connected")
			_controller = true;
		else if (arg == "disconnected")
			_controller = false;
	}

	public void OnCameraEnableMessage(string message, string arg) {
		_enabled = arg.Equals(PlayerCamera.CameraEnableMessage);
	}

	private float _cameraTilt = 0.0f;
	private bool _controller = false;
	private bool _enabled = true;
	private string _axis = "Joystick5";
}
