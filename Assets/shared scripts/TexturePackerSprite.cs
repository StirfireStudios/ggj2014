using UnityEngine;
using System.Collections;

public class TexturePackerSprite : MonoBehaviour
{

	public TextAsset atlas;
	public string spriteName;
	public bool regenrateOnAwake = false;
	public bool centreX = false;
	public bool centreY = false;
	
	void Awake()
	{
		if (regenrateOnAwake)
		{
			if (atlas != null && spriteName != null)
			{
				setupSprite();
			}
		}
	}

	public void setupSprite()
	{
		//check for renderer etc on target
		MeshRenderer renderer = GetComponent<MeshRenderer>();
		if (renderer == null)
		{
			gameObject.AddComponent<MeshRenderer>();
		}
		MeshFilter filter = GetComponent<MeshFilter>();
		if (filter == null)
		{
			filter = gameObject.AddComponent<MeshFilter>();
		}
		Mesh spriteMesh = TexturePackerLoader.getMesh(atlas, spriteName, centreX, centreY);
		filter.sharedMesh = spriteMesh;
	}
}