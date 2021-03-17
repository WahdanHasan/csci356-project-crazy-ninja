using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class DetectWeapons : MonoBehaviour
{

	public void Shoot(Vector3 dir)
	{
		if (!this.hasGun)
		{
			return;
		}
		this.gunScript.Use(dir);
	}
    
	private void Update()
	{
		//GetInput();
		if (!this.hasGun)
		{
			return;
		}
		//this.gun.transform.localRotation = Quaternion.Slerp(this.gun.transform.localRotation, this.desiredRot, Time.deltaTime * this.speed);
		//this.gun.transform.localPosition = Vector3.SmoothDamp(this.gun.transform.localPosition, this.desiredPos, ref this.posVel, 1f / this.speed);
		this.gunScript.OnAim();

	}
	private void Start()
	{
		this.weapons = new List<GameObject>();
	}
    
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Gun") && !this.weapons.Contains(other.gameObject))
		{
			this.weapons.Add(other.gameObject);
		}
	}
    
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Gun") && this.weapons.Contains(other.gameObject))
		{
			this.weapons.Remove(other.gameObject);
		}
	}
    
	public void ForcePickup(GameObject gun)
	{
		this.gunScript = (Pickup)gun.GetComponent(typeof(Pickup));
		if (this.gunScript.pickedUp)
		{
			gun = null;
			this.gunScript = null;
			return;
		}
		UnityEngine.Object.Destroy(gun.GetComponent<Rigidbody>());
		this.scale = gun.transform.localScale;
		gun.transform.parent = this.weaponPos;
		gun.transform.localScale = this.scale;
		this.hasGun = true;
		this.gunScript.PickupWeapon(true);
		gun.layer = LayerMask.NameToLayer("Equipable");
	}
    
	public float GetRecoil()
	{
		return this.gunScript.recoil;
	}
    
    
	public bool HasGun()
	{
		return this.hasGun;
	}
    
	public Transform weaponPos;
    
	public List<GameObject> weapons;
    
	private bool hasGun;
    
	private Pickup gunScript;
    
	private Vector3 scale;
}
