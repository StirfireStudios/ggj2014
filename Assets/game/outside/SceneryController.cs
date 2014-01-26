using UnityEngine;
using System.Collections;

public class SceneryController : MonoBehaviour
{
	public Material[] baseMaterials;
	public float[] speeds;
	public float globalSpeed = 1;

	Material[] materials;
	float timer = 0;

	void Start()
	{
		materials = new Material[baseMaterials.Length];

		MeshRenderer[] rends = GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < baseMaterials.Length; i++)
		{
			materials[i] = new Material(baseMaterials[i]);
			foreach (MeshRenderer child in rends)
			{
				if (child.sharedMaterial.name == materials[i].name)
				{
					child.sharedMaterial = materials[i];
				}
			}
		}
	}

	void Update()
	{
		timer = timer + Time.deltaTime;
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].SetTextureOffset("_MainTex", new Vector2(timer * speeds[i] * globalSpeed, 0));
		}
	}
}
