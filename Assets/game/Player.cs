using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public static Player Instance;

	public Transform cameraTrans;
	public Behaviour movement;
	public Behaviour steering;
	public string[] names;
	public string characterName;

	Transform targetTrans;

	void Awake()
	{
		Instance = this;
		characterName = names[0];

		MessagePasser.subscribe("time-end", OnTimeEnd);
		MessagePasser.subscribe("player-stop", OnApproach);
		MessagePasser.subscribe("approach-end", OnApproach);
	}

	void Start()
	{
		PlayerSpawn.MoveTo(transform, characterName);
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

	void OnApproach(string message, string arg)
	{
		if (message == "player-stop")
		{
			movement.enabled = false;
			steering.enabled = false;
		}
		else if (message == "approach-end")
		{
			movement.enabled = true;
			steering.enabled = true;
			targetTrans = null;
		}
	}

	void OnTimeEnd(string message, string arg)
	{
		characterName = names[1];
		PlayerSpawn.MoveTo(transform, characterName);
		cameraTrans.localPosition = Vector3.up * 0.45f;
	}

	public static void PointAt(Transform target)
	{
		Instance.targetTrans = target;
	}
}
