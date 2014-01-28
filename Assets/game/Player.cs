using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public static Player Instance;

	public const string ControllerConnectionChannel = "controller-status";
	public const string ControllerConnected = "connected";
	public const string ControllerDisconnected = "disconnected";

	public Transform cameraTrans;
	public string[] names;
	public string characterName;
	Transform targetTrans;

	IEnumerator CheckForControllers() {
		while(true) {
			string[] controllers = Input.GetJoystickNames();
			if (!_controller && controllers.Length > 0) {
				_controller = true;
				MessagePasser.send(Player.ControllerConnectionChannel, Player.ControllerConnected);
			} else if (_controller && controllers.Length == 0) {
				_controller = false;
				MessagePasser.send(Player.ControllerConnectionChannel, Player.ControllerDisconnected);
			}
			yield return new WaitForSeconds(1f);
		}
	}
	
	void Awake() {
		Instance = this;
		characterName = names[0];

		MessagePasser.subscribe("time-end", OnTimeEnd);
		MessagePasser.subscribe("player-stop", OnApproach);
		MessagePasser.subscribe("approach-end", OnApproach);
		MessagePasser.subscribe("penultimate-tick", OnPenultimate);
	}

	void Start()
	{
		PlayerSpawn.MoveTo(transform, characterName);
		StartCoroutine(CheckForControllers());
	}

	void Update()
	{
		if (targetTrans != null)
		{
			Vector3 offset = targetTrans.position - transform.position;
			offset.y = 0;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(offset), 0.1f);
		}
	}

	void OnApproach(string message, object arg)
	{
		if (message == "player-stop") {
			MessagePasser.send(PlayerMove.PlayerMoveEnableChannel, PlayerMove.PlayerMoveDisableMessage);
		}
		else if (message == "approach-end")
		{
			MessagePasser.send(PlayerMove.PlayerMoveEnableChannel, PlayerMove.PlayerMoveEnableMessage);
			targetTrans = null;
		}
	}

	void OnPenultimate(string message, object arg)
	{
		characterName = names[1];
	}

	void OnTimeEnd(string message, object arg)
	{
		characterName = names[1];
		PlayerSpawn.MoveTo(transform, characterName);
		//adjust height to be child
		cameraTrans.localPosition = Vector3.up * 0.4f;
		Vector3 pos = transform.position;
		pos.y = 0.45f;
		transform.position = pos;
		CharacterController col = GetComponent<CharacterController>();
		col.height = 0.9f;
		//col.center = Vector3.down * 0.15f;
	}

	public static void PointAt(Transform target)
	{
		Instance.targetTrans = target;
	}
	private bool _controller = false;
}
