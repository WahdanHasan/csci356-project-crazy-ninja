using System;
using UnityEngine;

public class Grappling : MonoBehaviour
{
	public static Grappling Instance { get; set; }
    
	private void Start()
	{
		Grappling.Instance = this;
		this.lr = base.GetComponentInChildren<LineRenderer>();
		//this.lr.positionCount = 0;
	}
    
	private void Update()
	{
		this.DrawLine();
		if (this.connectedTransform == null)
		{
			return;
		}
		Vector2 vector = (this.connectedTransform.position - base.transform.position).normalized;
		Mathf.Atan2(vector.y, vector.x);
		if(this.joint == null);
	}
    
	private void DrawLine()
	{
		if (this.connectedTransform == null || this.joint == null)
		{
			this.ClearLine();
			return;
		}
		this.desiredPos = this.connectedTransform.position + this.offsetPoint;
		this.endPoint = Vector3.SmoothDamp(this.endPoint, this.desiredPos, ref this.ropeVel, 0.03f);
		this.offsetMultiplier = Mathf.SmoothDamp(this.offsetMultiplier, 0f, ref this.offsetVel, 0.12f);
		int num = 100;
		this.lr.positionCount = num;
		Vector3 position = base.transform.position;
		this.lr.SetPosition(0, position);
		this.lr.SetPosition(num - 1, this.endPoint);
		float num2 = 15f;
		float num3 = 0.5f;
		for (int i = 1; i < num - 1; i++)
		{
			float num4 = (float)i / (float)num;
			float num5 = num4 * this.offsetMultiplier;
			float num6 = (Mathf.Sin(num5 * num2) - 0.5f) * num3 * (num5 * 2f);
			Vector3 normalized = (this.endPoint - position).normalized;
			float num7 = Mathf.Sin(num4 * 180f * 0.017453292f);
			float num8 = Mathf.Cos(this.offsetMultiplier * 90f * 0.017453292f);
			
		}
	}
    
	private void ClearLine()
	{
		//this.lr.positionCount = 0;
	}
    
	public void Use(Vector3 attackDirection)
	{
		if (!this.readyToUse)
		{
			return;
		}
		this.ShootRope(attackDirection);
		this.readyToUse = false;
	}
    
	public void StopUse()
	{
		if (this.joint == null)
		{
			return;
		}
		MonoBehaviour.print("STOPPING");
		this.connectedTransform = null;
		this.readyToUse = true;
	}
    
	private void ShootRope(Vector3 dir)
	{
		RaycastHit[] array = Physics.RaycastAll(base.transform.position, dir, 10f, this.whatIsSickoMode);
		GameObject gameObject = null;
		RaycastHit raycastHit = default(RaycastHit);
		foreach (RaycastHit raycastHit2 in array)
		{
			gameObject = raycastHit2.collider.gameObject;
			if (gameObject.layer != LayerMask.NameToLayer("Player"))
			{
				raycastHit = raycastHit2;
				break;
			}
			gameObject = null;
		}
		if (gameObject == null || raycastHit.collider == null)
		{
			return;
		}
		this.connectedTransform = raycastHit.collider.transform;
		this.joint = base.gameObject.AddComponent<SpringJoint>();
		UnityEngine.Object component = gameObject.GetComponent<Rigidbody>();
		this.offsetPoint = raycastHit.point - raycastHit.collider.transform.position;
		this.joint.connectedBody = gameObject.GetComponent<Rigidbody>();
		if (component == null)
		{
			this.joint.connectedAnchor = raycastHit.point;
		}
		else
		{
			this.joint.connectedAnchor = this.offsetPoint;
		}
		this.joint.autoConfigureConnectedAnchor = false;
		this.endPoint = base.transform.position;
		this.offsetMultiplier = 1f;
	}
    
	private LineRenderer lr;
    
	public LayerMask whatIsSickoMode;
    
	private Transform connectedTransform;
    
	private SpringJoint joint;
    
	private Vector3 offsetPoint;
    
	private Vector3 endPoint;
    
	private Vector3 ropeVel;
    
	private Vector3 desiredPos;
    
	private float offsetMultiplier;
    
	private float offsetVel;
    
	private bool readyToUse = true;
}
