using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
	public Transform target;
	public bool reverse;
	public bool local;
	Transform trans;
	
	void Start()
	{
		if (target == null)
		{
			target = Camera.main.transform;
		}
		trans = transform;
	}
	
	void Update()
	{
		Vector3 direction = target.forward;
		if (reverse)
		{
			direction = -direction;
		}
		if (local)
		{
			trans.localRotation = Quaternion.LookRotation(direction);
		}
		else
		{
			trans.rotation = Quaternion.LookRotation(direction);
		}
	}
}
