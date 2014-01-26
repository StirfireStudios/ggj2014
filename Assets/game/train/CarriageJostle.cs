using UnityEngine;
using System.Collections;

public class CarriageJostle : MonoBehaviour {

	public AnimationCurve curve;
	public float strength;
	public float speed;

	Transform trans;
	Vector3 basePos;
	float timer;

	void Start()
	{
		trans = transform;
		basePos = trans.position;
		timer = Random.value * 3;
	}

	void Update()
	{
		timer = timer + Time.deltaTime * speed;
		trans.position = basePos + Vector3.up * strength * curve.Evaluate(timer);
	}
}
