using System;
using UnityEngine;

public class Barrel : MonoBehaviour
{
	private void OnCollisionEnter(Collision other)
	{
		int layer = other.gameObject.layer;
		LayerMask.NameToLayer("Bullet");
	}
    
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Bullet"))
		{
			return;
		}
		this.done = true;
		base.Invoke("Explode", 0.2f);
	}
    
	private void Explode()
	{
		UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.explosion, base.transform.position, Quaternion.identity);
		UnityEngine.Object.Destroy(base.gameObject);
	}
    
	private bool done;
}
