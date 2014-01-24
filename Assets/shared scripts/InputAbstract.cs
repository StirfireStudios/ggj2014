using UnityEngine;
using System.Collections;

public class InputAbstract
{

	public static Vector2? getTouch(bool instantaneous = false)
	{
		Vector2? result = null;
		if (Input.GetMouseButton(0))
		{
			result = Input.mousePosition;
			if (instantaneous && !Input.GetMouseButtonDown(0))
			{
				result = null;
			}
		}
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			result = touch.position;
			if (instantaneous && touch.phase != TouchPhase.Began)
			{
				result = null;
			}
		}
		//make resolution independant
		if (result != null)
		{
			Vector2 castResult = (Vector2)result;
			//divide both by height to maintain aspect
			castResult.x /= Screen.height;
			castResult.y /= Screen.height;
			result = castResult;
		}
		return result;
	}

}
