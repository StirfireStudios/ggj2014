using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

	public MeshRenderer rend;

	Material mat;
	float timer;
	bool going;
	bool fadingOut;

	void Start()
	{
		rend = GetComponent<MeshRenderer>();
		mat = new Material(rend.sharedMaterial);
		rend.sharedMaterial = mat;

		MessagePasser.subscribe("penultimate-tick", OnPenultimate);

		going = true;
		timer = 0;
		fadingOut = true;
	}

	void OnPenultimate(string message, object arg)
	{
		going = true;
		timer = 0;
		fadingOut = false;
	}

	// Update is called once per frame
	void Update ()
	{
		if (going)
		{
			timer = timer + Time.deltaTime;
			float alpha = timer / 5;
			if (fadingOut)
			{
				mat.color = new Color(1, 1, 1, 1 - alpha);
				if (timer >= 5)
				{
					going = false;
					mat.color = new Color(1, 1, 1, 0);
				}
			}
			else
			{
				mat.color = new Color(1, 1, 1, alpha);
				if (timer >= 5)
				{
					fadingOut = true;
					timer = 0;
				}
			}
		}
	}
}
