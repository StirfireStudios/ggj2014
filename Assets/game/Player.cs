using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public static Player Instance;

	public string characterName;

	void Awake()
	{
		Instance = this;
	}
}
