using UnityEngine;
using System.Collections;

public class SpeechText : MonoBehaviour
{
	public Transform target;
	public bool reverse;
	public bool local;

	Transform trans;
	TextMesh text;
	
	void Start()
	{
		if (target == null)
		{
			target = Camera.main.transform;
		}
		trans = transform;

		text = GetComponent<TextMesh>();
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

		Vector3 offset = trans.position - target.position;
		text.color = new Color(1, 1, 1, 1 - offset.sqrMagnitude * 0.03f);
	}
}
