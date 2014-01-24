using UnityEngine;
using System.Collections;

public class AnimatedSprite : MonoBehaviour
{
	public float frameTime = 1/20f;
	public string[] frames;

	TexturePackerSprite spriteScript;
	int currentIndex = 0;
	float timer = 0;

	void Awake()
	{
		//add sprite script if not already present
		spriteScript = GetComponent<TexturePackerSprite>();
		if (spriteScript == null)
		{
			spriteScript = gameObject.AddComponent<TexturePackerSprite>();
		}
		//play animation
		PlayAnimation(frames);
	}

	void Update()
	{
		if (frames != null && frames.Length > 0)
		{
			timer -= Time.deltaTime;
			if (timer <= 0)
			{
				timer += frameTime;
				currentIndex = (currentIndex + 1) % frames.Length;
				//switch to new frame
				spriteScript.spriteName = frames[currentIndex];
				spriteScript.setupSprite();
			}
		}
	}

	public void PlayAnimation(string[] frames)
	{
		this.frames = frames;
		currentIndex = 0;
		timer = frameTime;
		if (frames != null && frames.Length > 0)
		{
			//switch to first frame
			spriteScript.spriteName = frames[currentIndex];
			spriteScript.setupSprite();
		}
	}
}
