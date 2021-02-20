using System;
using EZCameraShake;
using UnityEngine;

public class ShakeByDistance : MonoBehaviour
{
	private void Start()
	{
		this._shakeInstance = CameraShaker.Instance.StartShake(2f, 14f, 0f);
	}
    
	private void Update()
	{
		float num = Vector3.Distance(this.Player.transform.position, base.transform.position);
		this._shakeInstance.ScaleMagnitude = 1f - Mathf.Clamp01(num / this.Distance);
	}
    
	public GameObject Player;
    
	public float Distance = 10f;
    
	private CameraShakeInstance _shakeInstance;
}
