using System;
using UnityEngine;

public class Lava : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			Health player_health = other.GetComponent<Health>();

			player_health.TakeDamage(player_health.GetCurrentHealth());
		}
	}
}
