using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public static Player Instance;

	public Transform cameraTrans;
	public string[] names;
	public string characterName;

	void Awake()
	{
		Instance = this;
		characterName = names[1];

		MessagePasser.subscribe("time-end", OnTimeEnd);
	}

	void OnTimeEnd(string message, string arg)
	{
		characterName = names[2];
		PlayerSpawn.MoveTo(transform, characterName);
		cameraTrans.localPosition = Vector3.up * 0.45f;
	}
}
