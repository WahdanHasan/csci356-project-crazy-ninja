using System;
using UnityEngine;

public class Enigma : MonoBehaviour
{
	private void Update()
	{
		float z = Mathf.PingPong(Time.time, 1f);
		Vector3 axis = new Vector3(1f, 1f, z);
		base.transform.Rotate(axis, 0.5f);
	}
    
	private void OnTriggerEnter(Collider collide)
	{
		if (collide.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			if (PlayerMovement.Instance.IsDead())
			{
				return;
			}
			Game.Instance.Win();
			MonoBehaviour.print("Player won");
		}
	}
}
