using System;
using UnityEngine;

public class Respawn : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		MonoBehaviour.print(other.gameObject.layer);
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			Transform root = other.transform.root;
			root.transform.position = this.respawnPoint.position;
			root.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}
    
	public Transform respawnPoint;
}
