using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	public const string PlayerMoveEnableChannel = "movement-enable";
	public const string PlayerMoveDisableMessage = "disable";
	public const string PlayerMoveEnableMessage = "enable";


	public bool DisableControllerInEditor = false;
	public bool TestTouchInEditor = false;
	public float JoyStickCameraPanSensitivity = 3.0f;
	public float MouseCameraPanSensitivity = 10.0f;
	public float TouchCameraPanSensitivity = 0.15f;

	public float JoyStickPlayerMoveSensitivity = 3.0f;
	public float KeyboardPlayerMoveSensitivity = 10.0f;
	public float TouchPlayerMoveSensitivity = 0.01f;

	void Awake() {
		MessagePasser.subscribe(Player.ControllerConnectionChannel, OnControllerStatus);
		MessagePasser.subscribe(PlayerMove.PlayerMoveEnableChannel, OnMoveEnableMessage);
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
			DisableControllerInEditor = false;
			TestTouchInEditor = false;
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor) {
			_panAxis = "Joystick3";
		} 
		
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			Input.simulateMouseWithTouches = false;
		}
		_playerPan = transform.eulerAngles.y;
	}

	void panPlayerBy(float amount) {
		_playerPan += amount;
		transform.eulerAngles = new Vector3(0, _playerPan);
	}

	void movePlayerBy(Vector3 amount) {
		CharacterController controller = GetComponent<CharacterController>();
		controller.Move(transform.TransformDirection(amount));
//		transform.localPosition += transform.TransformDirection(amount);
	}

	Vector2 getCameraPositionAngles(Vector2 position) {
		double temp = Camera.main.fieldOfView / Screen.height; // degrees per pixel;
		return new Vector2((float)temp * (Screen.width / 2 - position.x), (float)temp * (position.y - Screen.height / 2));
	}

	// Update is called once per frame
	void Update () {
		if (!_enabled)
		{
			return;
		}
		if (_controller && !TestTouchInEditor) {
			panPlayerBy(Input.GetAxis(_panAxis) * JoyStickCameraPanSensitivity);
			movePlayerBy(new Vector3(Input.GetAxis ("Horizontal") * JoyStickPlayerMoveSensitivity, 0, Input.GetAxis("Vertical") * JoyStickPlayerMoveSensitivity));
		} else {
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || TestTouchInEditor) {
				if (Input.touchCount > 0 || Input.GetButton("Fire2") || Input.GetButton("Fire1")) {
					Vector2 position; 
					if (Input.GetButton("Fire2") || Input.GetButton("Fire1"))
						position = Input.mousePosition;
					else
						position = Input.GetTouch(0).position;

					if (_touchCount == 0) {
						_touchCount = Input.touchCount;
					}

					if (_touchCount > 1 || Input.GetButton("Fire2")) {
						Vector2 diffAngles = _touchStartAngles - getCameraPositionAngles(position);
						transform.eulerAngles = new Vector3(0, _touchStartAngles.x + diffAngles.x);
					} else {
						Vector2 diffAngles = _touchStartAngles - getCameraPositionAngles(position);
						panPlayerBy(diffAngles.x * TouchCameraPanSensitivity);
						movePlayerBy(new Vector3(0, 0, -diffAngles.y * TouchPlayerMoveSensitivity));
					}
				} else if (_touchCount > 0) {
					_touchCount = 0;
					_playerPan = transform.eulerAngles.y;
				}
			} else {
				panPlayerBy(Input.GetAxis("Mouse X") * MouseCameraPanSensitivity);
				movePlayerBy(new Vector3(Input.GetAxis ("Horizontal") * KeyboardPlayerMoveSensitivity, 0, Input.GetAxis("Vertical")  * KeyboardPlayerMoveSensitivity));
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
	
	public void OnMoveEnableMessage(string message, object arg) {
		_enabled = arg.Equals(PlayerCamera.CameraEnableMessage);
	}
	
	private Vector2 _touchStartAngles;
	private int _touchCount = 0;
	private float _playerPan;
	private string _panAxis = "Joystick4";
	private string _moveAxis = "Vertical";
	private bool _controller = false;
	private bool _enabled = true;
}
