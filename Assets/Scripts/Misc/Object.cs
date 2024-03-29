﻿using System;
using UnityEngine;

public class Object : MonoBehaviour
{
	private void OnCollisionEnter(Collision other)
	{
		float num = other.relativeVelocity.magnitude * 0.025f;
		if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && this.hitReady && num > 0.8f)
		{
			this.hitReady = false;
			Vector3 normalized = base.GetComponent<Rigidbody>().velocity.normalized;
			UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.enemyHitAudio, other.contacts[0].point, Quaternion.identity);
			((RagdollController)other.transform.root.GetComponent(typeof(RagdollController))).MakeRagdoll(normalized * 350f);
			Rigidbody component = other.gameObject.GetComponent<Rigidbody>();
			if (component)
			{
				component.AddForce(normalized * 1100f);
			}
			((Enemy)other.transform.root.GetComponent(typeof(Enemy))).DropGun(Vector3.up);
		}
		if (!this.ready)
		{
			return;
		}
		this.ready = false;
		//AudioSource component2 = UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.objectImpactAudio, base.transform.position, Quaternion.identity).GetComponent<AudioSource>();
		Rigidbody component3 = base.GetComponent<Rigidbody>();
		float num2 = 1f;
		if (component3)
		{
			num2 = component3.mass;
		}
		if (num2 < 0.3f)
		{
			num2 = 0.5f;
		}
		if (num2 > 1f)
		{
			num2 = 1f;
		}
		//float volume = component2.volume;
		if (num > 1f)
		{
			num = 1f;
		}
		//component2.volume = num * num2;
		base.Invoke("GetReady", 0.1f);
	}
    
	private void GetReady()
	{
		this.ready = true;
	}
    
	private bool ready = true;
    
	private bool hitReady = true;
}
