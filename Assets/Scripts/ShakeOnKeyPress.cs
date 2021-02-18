using System;
using EZCameraShake;
using UnityEngine;

public class ShakeOnKeyPress : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			CameraShaker.Instance.ShakeOnce(this.Magnitude, this.Roughness, 0f, this.FadeOutTime);
		}
	}
    
	public float Magnitude = 2f;
    
	public float Roughness = 10f;
    
	public float FadeOutTime = 5f;
}
