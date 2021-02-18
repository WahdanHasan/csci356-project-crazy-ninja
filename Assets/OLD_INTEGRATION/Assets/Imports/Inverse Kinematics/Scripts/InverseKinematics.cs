using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class InverseKinematics : MonoBehaviour {

	public Transform thigh;
	public Transform crus;
	public Transform foot;
	public Transform knee;
	public Transform look;
	[Space(20)]
	public Vector3 uppperArm_OffsetRotation;
	public Vector3 crus_OffsetRotation;
	public Vector3 foot_OffsetRotation;
	[Space(20)]
	public bool footMatchesTargetRotation = true;
	[Space(20)]
	public bool debug;

	float angle;
	float thigh_Length;
	float crus_Length;
	float arm_Length;
	float targetDistance;
	float adyacent;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(thigh != null && crus != null && foot != null && knee != null && look != null){
			thigh.LookAt (look, knee.position - thigh.position);
			thigh.Rotate (uppperArm_OffsetRotation);

			Vector3 cross = Vector3.Cross (knee.position - thigh.position, crus.position - thigh.position);



			thigh_Length = Vector3.Distance (thigh.position, crus.position);
			crus_Length =  Vector3.Distance (crus.position, foot.position);
			arm_Length = thigh_Length + crus_Length;
			targetDistance = Vector3.Distance (thigh.position, look.position);
			targetDistance = Mathf.Min (targetDistance, arm_Length - arm_Length * 0.001f);

			adyacent = ((thigh_Length * thigh_Length) - (crus_Length * crus_Length) + (targetDistance * targetDistance)) / (2*targetDistance);

			angle = Mathf.Acos (adyacent / thigh_Length) * Mathf.Rad2Deg;

			thigh.RotateAround (thigh.position, cross, -angle);

			crus.LookAt(look, cross);
			crus.Rotate (crus_OffsetRotation);

			if(footMatchesTargetRotation){
				foot.rotation = look.rotation;
				foot.Rotate (foot_OffsetRotation);
			}
			
			if(debug){
				if (crus != null && knee != null) {
					Debug.DrawLine (crus.position, knee.position, Color.blue);
				}

				if (thigh != null && look != null) {
					Debug.DrawLine (thigh.position, look.position, Color.red);
				}
			}
					
		}
		
	}

	void OnDrawGizmos(){
		if (debug) {
			if(thigh != null && knee != null && foot != null && look != null && knee != null){
				Gizmos.color = Color.gray;
				Gizmos.DrawLine (thigh.position, crus.position);
				Gizmos.DrawLine (crus.position, foot.position);
				Gizmos.color = Color.red;
				Gizmos.DrawLine (thigh.position, look.position);
				Gizmos.color = Color.blue;
				Gizmos.DrawLine (crus.position, knee.position);
			}
		}
	}

}
