using UnityEngine;
using System.Collections;

public class BackgroundTrees : MonoBehaviour {

	public Transform player;

	Transform trans;

	void Start()
	{
		trans = transform;
	}

	void Update()
	{
		trans.rotation = player.rotation;
		Vector3 newPos = player.position;
		newPos.x = 0;
		newPos.z = 0;
		trans.position = newPos;
	}
}
