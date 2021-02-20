using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class DetectWeapons : MonoBehaviour
{
	//public void Pickup()
	//{
	//	if (this.hasGun || !this.HasWeapons())
	//	{
	//		return;
	//	}
	//	this.gun = this.GetWeapon();
	//	this.gunScript = (Pickup)this.gun.GetComponent(typeof(Pickup));
	//	if (this.gunScript.pickedUp)
	//	{
	//		this.gun = null;
	//		this.gunScript = null;
	//		return;
	//	}
	//	UnityEngine.Object.Destroy(this.gun.GetComponent<Rigidbody>());
	//	this.scale = this.gun.transform.localScale;
	//	this.gun.transform.parent = this.weaponPos;
	//	this.gun.transform.localScale = this.scale;
	//	this.hasGun = true;
	//	this.gunScript.PickupWeapon(true);
	//	//AudioManager.Instance.Play("GunPickup");
	//	this.gun.layer = LayerMask.NameToLayer("Equipable");
	//}

	public void ForceDrop(GameObject go)
    {
		go.SetActive(false);
		this.gunScript.Drop();
    }

	public void Shoot(Vector3 dir)
	{
		if (!this.hasGun)
		{
			return;
		}
		this.gunScript.Use(dir);
	}
    
	public void StopUse()
	{
		if (!this.hasGun)
		{
			return;
		}
		this.gunScript.StopUse();
	}

	public void StartUse()
	{
		if (!this.hasGun)
		{
			return;
		}
		this.gunScript.Use(Vector3.zero);
	}


    
	//public void Throw(Vector3 throwDir)
	//{
	//	if (!this.hasGun)
	//	{
	//		return;
	//	}
	//	if (this.gun.GetComponent<Rigidbody>())
	//	{
	//		return;
	//	}
	//	this.gunScript.StopUse();
	//	this.hasGun = false;
	//	Rigidbody rigidbody = this.gun.AddComponent<Rigidbody>();
	//	rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
	//	rigidbody.maxAngularVelocity = 20f;
	//	rigidbody.AddForce(throwDir * this.throwForce);
	//	rigidbody.AddRelativeTorque(new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f) * 0.4f), ForceMode.Impulse);
	//	this.gun.layer = LayerMask.NameToLayer("Gun");
	//	this.gunScript.Drop();
	//	this.gun.transform.parent = null;
	//	this.gun.transform.localScale = this.scale;
	//	this.gun = null;
	//	this.gunScript = null;
	//}
	public void PutAway()
	{
		if (!this.hasGun)
		{
			return;
		}
		if (this.gun.GetComponent<Rigidbody>())
		{
			return;
		}
		this.gun.SetActive(false);
		this.gunScript.StopUse();
		this.hasGun = false;
		this.gun.transform.parent = null;
		this.gun.transform.localScale = this.scale;
		this.gun = null;
		this.gunScript = null;
	}

	public void Fire(Vector3 dir)
	{
		this.gunScript.Use(dir);
	}
    
	private void Update()
	{
		//GetInput();
		if (!this.hasGun)
		{
			return;
		}
		this.gun.transform.localRotation = Quaternion.Slerp(this.gun.transform.localRotation, this.desiredRot, Time.deltaTime * this.speed);
		this.gun.transform.localPosition = Vector3.SmoothDamp(this.gun.transform.localPosition, this.desiredPos, ref this.posVel, 1f / this.speed);
		this.gunScript.OnAim();

	}

	int previousIndex = -1;

	//private void GetInput()
 //   {
	//	if(Input.GetKeyDown(KeyCode.Alpha1))
 //       {
	//		indexOfWeapon = 0;
	//	}
	//	if(Input.GetKeyDown(KeyCode.Alpha2))
 //       {
	//		indexOfWeapon = 1;
	//	}
	//	if (Input.GetKeyDown(KeyCode.Alpha3))
	//	{
	//		indexOfWeapon = 2;
	//	}

	//	if (indexOfWeapon != previousIndex)
	//		Pickup();
	//	previousIndex = indexOfWeapon;
	//}
    
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

	int indexOfWeapon;

	public GameObject GetWeapon()
	{
		return null;

		if (this.weapons.Count == 1)
		{
			switch (indexOfWeapon)
			{
				case 0:
					return this.weapons[0];
				case 1:
					return this.weapons[1];
				case 2:
					return this.weapons[2];
			}
		}
		GameObject result = null;
		float num = float.PositiveInfinity;
		foreach (GameObject gameObject in this.weapons)
		{
			float num2 = Vector3.Distance(base.transform.position, gameObject.transform.position);
			if (num2 < num)
			{
				num = num2;
				result = gameObject;
			}
		}
		return result;
	}
    
	public void ForcePickup(GameObject gun)
	{
		this.gunScript = (Pickup)gun.GetComponent(typeof(Pickup));
		this.gun = gun;
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
    
	public bool HasWeapons()
	{
        return this.weapons.Count > 0;
    }

    public Pickup GetWeaponScript()
	{
		return this.gunScript;
	}
    
	public bool HasGun()
	{
		return this.hasGun;
	}
    
	public Transform weaponPos;
    
	public List<GameObject> weapons;
    
	private bool hasGun;
    
	private GameObject gun;
    
	private Pickup gunScript;
    
	private float speed = 10f;
    
	private Quaternion desiredRot = Quaternion.Euler(0f, 0f, 0f);
    
	private Vector3 desiredPos = Vector3.zero;
    
	private Vector3 posVel;
    
	private float throwForce = 1000f;
    
	private Vector3 scale;
}
