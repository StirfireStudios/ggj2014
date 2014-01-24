using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class TexturePackerLoader
{
	private static TexturePackerLoader _instance;
	private static TexturePackerLoader Instance {
		get {
			if (_instance == null)
			{
				_instance = new TexturePackerLoader();
			}
			return _instance;
		}

	}

	Dictionary<string, IDictionary> atlasCache;
	Dictionary<string, string[]> textureNamesCache;
	Dictionary<string, Dictionary<string, Mesh>> meshCache;

	private TexturePackerLoader()
	{
		atlasCache = new Dictionary<string, IDictionary>();
		textureNamesCache = new Dictionary<string, string[]>();
		meshCache = new Dictionary<string, Dictionary<string, Mesh>>();
	}

	private static IDictionary loadAtlas(TextAsset data)
	{
		//check for cached
		if (Application.isPlaying)
		{
			if (Instance.atlasCache.ContainsKey(data.name))
			{
				return Instance.atlasCache[data.name];
			}
		}
		IDictionary atlas = Json.Deserialize(data.text) as IDictionary;
		//store in cache if playing
		if (Application.isPlaying)
		{
			Instance.atlasCache.Add(data.name, atlas);
		}
		return atlas;
	}

	public static string[] getTextureNames(TextAsset data)
	{
		//check for cached
		if (Application.isPlaying)
		{
			if (Instance.textureNamesCache.ContainsKey(data.name))
			{
				return Instance.textureNamesCache[data.name];
			}
		}
		IDictionary atlas = loadAtlas(data);
		IDictionary frames = atlas["frames"] as IDictionary;
		List<string> names = new List<string>();
		foreach (string key in frames.Keys)
		{
			names.Add(key);
		}
		string[] result = names.ToArray();
		//store in cache if playing
		if (Application.isPlaying)
		{
			Instance.textureNamesCache.Add(data.name, result);
		}
		return result;
	}

	public static Mesh getMesh(TextAsset data, string sprite, bool centreX, bool centreY)
	{
		//check for cached
		if (Application.isPlaying)
		{
			if (Instance.meshCache.ContainsKey(data.name))
			{
				Dictionary<string, Mesh> meshes = Instance.meshCache[data.name];
				if (meshes.ContainsKey(sprite))
				{
					return meshes[sprite];
				}
			}
		}
		IDictionary atlas = loadAtlas(data);
		//get metadata
		IDictionary metaData = atlas["meta"] as IDictionary;
		IDictionary textureSizeData = metaData["size"] as IDictionary;
		float textureW = (float)textureSizeData["w"];
		float textureH = (float)textureSizeData["h"];

		IDictionary frames = atlas["frames"] as IDictionary;
		IDictionary spriteData = frames[sprite] as IDictionary;
		if (spriteData == null)
		{
			Debug.LogWarning("No sprite data found for " + sprite);
			return null;
		}

		Vector2[] uv = new Vector2[4];
		{
			IDictionary frameData = spriteData["frame"] as IDictionary;
			float x = (float)frameData["x"];
			float y = textureH - (float)frameData["y"];
			float w = (float)frameData["w"];
			float h = (float)frameData["h"];
			
			bool rotated = (bool)spriteData["rotated"];
			if (!rotated)
			{
				h = -h;
				uv[0] = new Vector2(x, y) / textureH;
				uv[1] = new Vector2(x+w, y) / textureH;
				uv[2] = new Vector2(x+w, y+h) / textureH;
				uv[3] = new Vector2(x, y+h) / textureH;
			}
			else
			{
				uv[0] = new Vector2(x+h, y) / textureH;
				uv[1] = new Vector2(x+h, y-w) / textureH;
				uv[2] = new Vector2(x, y-w) / textureH;
				uv[3] = new Vector2(x, y) / textureH;
			}
		}
		Vector3[] vert = new Vector3[4];
		{
			IDictionary sourceData = spriteData["spriteSourceSize"] as IDictionary;
			float x = (float)sourceData["x"];
			float y = (float)sourceData["y"];
			float w = (float)sourceData["w"];
			float h = (float)sourceData["h"];
			Vector3 offset = new Vector3(x, y, 0);
			
			if (centreX)
			{
				offset.x -= w/2;
			}
			if (centreY)
			{
				offset.y -= h/2;
			}
			
			vert[0] = new Vector3(offset.x, offset.y + h);
			vert[1] = new Vector3(offset.x + w, offset.y + h);
			vert[2] = new Vector3(offset.x + w, offset.y);
			vert[3] = new Vector3(offset.x, offset.y);
		}
		Vector3[] norm = new Vector3[4];
		{
			for (int i = 0; i < norm.Length; i++)
			{
				norm[i] = Vector3.forward;
			}
		}
		int[] tri = new int[] {0, 1, 2, 0, 2, 3};

		Mesh mesh = new Mesh();
		mesh.vertices = vert;
		mesh.uv = uv;
		mesh.normals = norm;
		mesh.triangles = tri;
		mesh.RecalculateBounds();
		//store in cache if playing
		if (Application.isPlaying)
		{
			if (!Instance.meshCache.ContainsKey(data.name))
			{
				Dictionary<string, Mesh> cache = new Dictionary<string, Mesh>();
				Instance.meshCache.Add(data.name, cache);
			}
			Dictionary<string, Mesh> meshes = Instance.meshCache[data.name];
			meshes.Add(sprite, mesh);
		}
		return mesh;
	}

}