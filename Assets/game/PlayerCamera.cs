using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {
	public const string CameraEnableChannel = "camera-enable";
	public const string CameraDisableMessage = "disable";
	public const string CameraEnableMessage = "enable";

	public bool DisableControllerInEditor = false;
	public bool TestTouchInEditor = false;
	public float MaxCameraTilt = 80.0f;
	public float JoyStickCameraTiltSensitivity = 3.0f;
	public float MouseCameraTiltSensitivity = 10.0f;

	void Awake() {
		MessagePasser.subscribe(Player.ControllerConnectionChannel, OnControllerStatus);
		MessagePasser.subscribe(PlayerCamera.CameraEnableChannel, OnCameraEnableMessage);
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
			DisableControllerInEditor = false;
			TestTouchInEditor = false;
		}
	}

	// Use this for initialization
	void Start () {
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor) {
			_axis = "Joystick4";
		} 
		
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			Input.simulateMouseWithTouches = false;
		}
	}

	// Use this for initialization
	void Start () {
	}

	void tiltCameraBy(float amount) {
		_cameraTilt += amount;
		_cameraTilt = Mathf.Clamp (_cameraTilt, -1 * MaxCameraTilt, MaxCameraTilt);
		transform.eulerAngles = new Vector3(_cameraTilt, 0);
	}

	float getCameraPositionAngle(Vector2 position) {
		double temp = camera.fieldOfView / Screen.height; // degrees per pixel;
		return (float)temp * (position.y - Screen.height / 2);
	}
	
	void Update () {
		if (!_enabled) {
			// Reset all touch offsets and states to zero.
			return;
		}

		if (_controller && !TestTouchInEditor) {
			tiltCameraBy(Input.GetAxis(_axis) * JoyStickCameraTiltSensitivity);
		} else {
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || TestTouchInEditor) {
				if (Input.touchCount == 2 || Input.GetButton("Fire1")) {
					Vector2 position; 
					if (Input.GetButton("Fire1"))
						position = Input.mousePosition;
					else
						position = Input.GetTouch(0).position;

					if (_touchStarted) {
						float diffAngle = _touchStartAngle - getCameraPositionAngle(position);
						transform.eulerAngles = new Vector3(Mathf.Clamp(_touchStartCameraAngle + diffAngle, -1 * MaxCameraTilt, MaxCameraTilt), 0);
					} else {

						_touchStarted = true;
						_touchStartAngle = getCameraPositionAngle(position);
						_touchStartCameraAngle = transform.eulerAngles.x;
						if (_touchStartCameraAngle > MaxCameraTilt) {
							_touchStartAngle = _touchStartAngle - 360.0f;
						}
						Debug.Log ("Touch Start: " + _touchStartCameraAngle);
					}
				} else if (_touchStarted) {
					_touchStarted = false;
				}
			} else {
				tiltCameraBy(-1 * Input.GetAxis("Mouse Y") * MouseCameraTiltSensitivity);
			}
		}
	}

	public void OnControllerStatus(string message, object arg) {
		if (DisableControllerInEditor) {
			_controller = false;
			return;
		}
		_controller = Player.ControllerConnected == (string)arg;
	}

	public void OnCameraEnableMessage(string message, object arg) {
		_enabled = arg.Equals(PlayerCamera.CameraEnableMessage);
	}

	private float _touchStartCameraAngle = 0f;
	private float _touchStartAngle = 0f;
	private bool _touchStarted = false;
	private float _cameraTilt = 0.0f;
	private bool _controller = false;
	private bool _enabled = true;
	private string _axis = "Joystick5";
}
