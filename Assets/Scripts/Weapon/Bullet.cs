﻿using System;
using Audio;
using UnityEngine;

public class Bullet : MonoBehaviour
{
#pragma warning disable 0618
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
	}
    
	private void OnCollisionEnter(Collision other)
	{
		if (this.done)
		{
			return;
		}
		this.done = true;
		if (this.explosive)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			//((Explosion)UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.explosion, other.contacts[0].point, Quaternion.identity).GetComponentInChildren(typeof(Explosion))).player = this.player;
			return;
		}
		this.BulletExplosion(other.contacts[0]);
		UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.bulletHitAudio, other.contacts[0].point, Quaternion.identity);
		int layer = other.gameObject.layer;

		if(layer == LayerMask.NameToLayer("PP"))
        {
			return;
        }

		if (layer == LayerMask.NameToLayer("Player"))
		{
			this.HitPlayer(other.gameObject);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (layer == LayerMask.NameToLayer("Enemy"))
		{

			if (this.col == Color.blue)
			{
				AudioManager.Instance.Play("Hitmarker");
			}
			UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.enemyHitAudio, other.contacts[0].point, Quaternion.identity);
			HitEnemy(other.gameObject);
			if (other.gameObject.GetComponent<Rigidbody>())
			{
				other.gameObject.GetComponent<Rigidbody>().AddForce(-base.transform.right * 1500f);
			}
			UnityEngine.Object.Destroy(base.gameObject);
			return; 
		}
		if (layer == LayerMask.NameToLayer("Bullet"))
		{
			if (other.gameObject.name == base.gameObject.name)
			{
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			UnityEngine.Object.Destroy(other.gameObject);
			this.BulletExplosion(other.contacts[0]);
		}


		UnityEngine.Object.Destroy(base.gameObject);
	}
    
	private void HitEnemy(GameObject enemy)
	{
		((Health)enemy.transform.root.GetComponent(typeof(Health))).TakeDamage((int)damage);
	}

	private void HitPlayer(GameObject player)
    {
		player.GetComponent<Health>().TakeDamage((int)damage);
	}

	private void Update()
	{
		if (!this.explosive)
		{
			return;
		}
		this.rb.AddForce(Vector3.up * Time.deltaTime * 1000f);
	}
    
	private void BulletExplosion(ContactPoint contact)
	{
		Vector3 point = contact.point;
		Vector3 normal = contact.normal;
		ParticleSystem component = UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.bulletDestroy, point + normal * 0.05f, Quaternion.identity).GetComponent<ParticleSystem>();
		component.transform.rotation = Quaternion.LookRotation(normal);
		component.startColor = Color.blue;
	}
    
	public void SetBullet(float damage, float push, Color col)
	{
		this.damage = damage;
		this.push = push;
		this.col = col;
		if (this.changeCol)
		{
			SpriteRenderer[] componentsInChildren = base.GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].color = col;
			}
		}
		TrailRenderer componentInChildren = base.GetComponentInChildren<TrailRenderer>();
		if (componentInChildren == null)
		{
			return;
		}
		componentInChildren.startColor = col;
		componentInChildren.endColor = col;
	}
    
	public bool changeCol;
    
	public bool player;

	public Vector3 rigidbody_velocity;
    
	private float damage;
    
	private float push;
    
	private bool done;
    
	private Color col;
    
	public bool explosive;
    
	private GameObject limbHit;
    
	private Rigidbody rb;
#pragma warning restore 0618
}
