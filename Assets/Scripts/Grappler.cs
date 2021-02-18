using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using UnityEngine;

public class Grappler : Pickup
{
	private void Start()
	{
		this.tip = base.transform.GetChild(0);
		this.lr = base.GetComponent<LineRenderer>();
		this.lr.positionCount = this.positions;
		this.aim.transform.parent = null;
		this.aim.SetActive(false);
	}
    
	public override void Use(Vector3 attackDirection)
	{
		if (this.grappling)
		{
			return;
		}
		this.grappling = true;
		Transform playerCamTransform = PlayerMovement.Instance.GetPlayerCamTransform();
		Transform transform = PlayerMovement.Instance.transform;
		RaycastHit[] array = Physics.RaycastAll(playerCamTransform.position, playerCamTransform.forward, 70f, this.whatIsGround);
		if (array.Length < 1)
		{
			if (this.nearestPoint == Vector3.zero)
			{
				return;
			}
			this.grapplePoint = this.nearestPoint;
		}
		else
		{
			this.grapplePoint = array[0].point;
		}
		this.joint = transform.gameObject.AddComponent<SpringJoint>();
		this.joint.autoConfigureConnectedAnchor = false;
		this.joint.connectedAnchor = this.grapplePoint;
		this.joint.maxDistance = Vector2.Distance(this.grapplePoint, transform.position) * 0.8f;
		this.joint.minDistance = Vector2.Distance(this.grapplePoint, transform.position) * 0.25f;
		this.joint.spring = 4.5f;
		this.joint.damper = 7f;
		this.joint.massScale = 4.5f;
		this.endPoint = this.tip.position;
		this.offsetMultiplier = 2f;
		this.lr.positionCount = this.positions;
		AudioManager.Instance.PlayPitched("Grapple", 0.2f);
	}
    
	public override void OnAim()
	{
		if (this.grappling)
		{
			return;
		}
		Transform playerCamTransform = PlayerMovement.Instance.GetPlayerCamTransform();
		List<RaycastHit> list = Physics.RaycastAll(playerCamTransform.position, playerCamTransform.forward, 70f, this.whatIsGround).ToList<RaycastHit>();
		if (list.Count > 0)
		{
			this.aim.SetActive(false);
			this.aim.transform.localScale = Vector3.zero;
			return;
		}
		int num = 50;
		int num2 = 10;
		float d = 0.035f;
		bool flag = list.Count > 0;
		int num3 = 0;
		while (num3 < num2 && !flag)
		{
			for (int i = 0; i < num; i++)
			{
				float f = 6.2831855f / (float)num * (float)i;
				float d2 = Mathf.Cos(f);
				float d3 = Mathf.Sin(f);
				Vector3 a = playerCamTransform.right * d2 + playerCamTransform.up * d3;
				list.AddRange(Physics.RaycastAll(playerCamTransform.position, playerCamTransform.forward + a * d * (float)num3, 70f, this.whatIsGround));
			}
			if (list.Count > 0)
			{
				break;
			}
			num3++;
		}
		this.nearestPoint = this.FindNearestPoint(list);
		if (list.Count > 0 && !this.grappling)
		{
			this.aim.SetActive(true);
			this.aim.transform.position = Vector3.SmoothDamp(this.aim.transform.position, this.nearestPoint, ref this.aimVel, 0.05f);
			Vector3 target = 0.025f * list[0].distance * Vector3.one;
			this.aim.transform.localScale = Vector3.SmoothDamp(this.aim.transform.localScale, target, ref this.scaleVel, 0.2f);
			return;
		}
		this.aim.SetActive(false);
		this.aim.transform.localScale = Vector3.zero;
	}
    
	private Vector3 FindNearestPoint(List<RaycastHit> hits)
	{
		Transform playerCamTransform = PlayerMovement.Instance.GetPlayerCamTransform();
		Vector3 result = Vector3.zero;
		float num = float.PositiveInfinity;
		for (int i = 0; i < hits.Count; i++)
		{
			if (hits[i].distance < num)
			{
				num = hits[i].distance;
				result = hits[i].collider.ClosestPoint(playerCamTransform.position + playerCamTransform.forward * num);
			}
		}
		return result;
	}
    
	public override void StopUse()
	{
		UnityEngine.Object.Destroy(this.joint);
		this.grapplePoint = Vector3.zero;
		this.grappling = false;
	}
    
	private void LateUpdate()
	{
		this.DrawGrapple();
	}
    
	private void DrawGrapple()
	{
		if (this.grapplePoint == Vector3.zero || this.joint == null)
		{
			this.lr.positionCount = 0;
			return;
		}
		this.endPoint = Vector3.Lerp(this.endPoint, this.grapplePoint, Time.deltaTime * 15f);
		this.offsetMultiplier = Mathf.SmoothDamp(this.offsetMultiplier, 0f, ref this.offsetVel, 0.1f);
		Vector3 position = this.tip.position;
		float num = Vector3.Distance(this.endPoint, position);
		this.lr.SetPosition(0, position);
		this.lr.SetPosition(this.positions - 1, this.endPoint);
		float num2 = num;
		float num3 = 1f;
		for (int i = 1; i < this.positions - 1; i++)
		{
			float num4 = (float)i / (float)this.positions;
			float num5 = num4 * this.offsetMultiplier;
			float num6 = (Mathf.Sin(num5 * num2) - 0.5f) * num3 * (num5 * 2f);
			Vector3 normalized = (this.endPoint - position).normalized;
			float num7 = Mathf.Sin(num4 * 180f * 0.017453292f);
			float num8 = Mathf.Cos(this.offsetMultiplier * 90f * 0.017453292f);
			
		}
	}
    
	public Vector3 GetGrapplePoint()
	{
		return this.grapplePoint;
	}
    
	private Transform tip;
    
	private bool grappling;
    
	public LayerMask whatIsGround;
    
	private Vector3 grapplePoint;
    
	private SpringJoint joint;
    
	private LineRenderer lr;
    
	private Vector3 endPoint;
    
	private float offsetMultiplier;
    
	private float offsetVel;
    
	public GameObject aim;
    
	private int positions = 100;
    
	private Vector3 aimVel;
    
	private Vector3 scaleVel;
    
	private Vector3 nearestPoint;
}
