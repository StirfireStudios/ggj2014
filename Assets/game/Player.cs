using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public static Player Instance;

	public string charcterName;

	void Awake()
	{
		Instance = this;
	}
}
