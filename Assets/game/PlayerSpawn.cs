using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawn : MonoBehaviour {

	static Dictionary<string, Transform> lookup;

	public string characterName = "Don";

	void Awake()
	{
		if (lookup == null)
		{
			lookup = new Dictionary<string, Transform>();
		}
		lookup.Add(characterName, transform);
	}

	public static void MoveTo(Transform target, string name)
	{
		Transform dest = lookup[name];
		Vector3 pos = dest.position;
		pos.y = target.position.y;
		target.position = pos;
		target.rotation = dest.rotation;
	}
}
