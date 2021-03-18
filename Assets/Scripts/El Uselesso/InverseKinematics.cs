using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class InverseKinematics : MonoBehaviour {

	public Transform Upper;
	public Transform Middle;
	public Transform Bottom;
	public Transform Joint_Orienter;
	public Transform look;
	[Space(20)]
	public Vector3 uppperArm_OffsetRotation;
	public Vector3 Middle_OffsetRotation;
	public Vector3 Bottom_OffsetRotation;
	[Space(20)]
	public bool BottomMatchesTargetRotation = true;
	[Space(20)]
	public bool debug;

	float angle;
	float Upper_Length;
	float Middle_Length;
	float arm_Length;
	float targetDistance;
	float adyacent;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(Upper != null && Middle != null && Bottom != null && Joint_Orienter != null && look != null){
			Upper.LookAt (look, Joint_Orienter.position - Upper.position);
			Upper.Rotate (uppperArm_OffsetRotation);

			Vector3 cross = Vector3.Cross (Joint_Orienter.position - Upper.position, Middle.position - Upper.position);



			Upper_Length = Vector3.Distance (Upper.position, Middle.position);
			Middle_Length =  Vector3.Distance (Middle.position, Bottom.position);
			arm_Length = Upper_Length + Middle_Length;
			targetDistance = Vector3.Distance (Upper.position, look.position);
			targetDistance = Mathf.Min (targetDistance, arm_Length - arm_Length * 0.001f);

			adyacent = ((Upper_Length * Upper_Length) - (Middle_Length * Middle_Length) + (targetDistance * targetDistance)) / (2*targetDistance);

			angle = Mathf.Acos (adyacent / Upper_Length) * Mathf.Rad2Deg;

			Upper.RotateAround (Upper.position, cross, -angle);

			Middle.LookAt(look, cross);
			Middle.Rotate (Middle_OffsetRotation);

			if(BottomMatchesTargetRotation){
				Bottom.rotation = look.rotation;
				Bottom.Rotate (Bottom_OffsetRotation);
			}
			
			if(debug){
				if (Middle != null && Joint_Orienter != null) {
					Debug.DrawLine (Middle.position, Joint_Orienter.position, Color.blue);
				}

				if (Upper != null && look != null) {
					Debug.DrawLine (Upper.position, look.position, Color.red);
				}
			}
					
		}
		
	}

	void OnDrawGizmos(){
		if (debug) {
			if(Upper != null && Joint_Orienter != null && Bottom != null && look != null && Joint_Orienter != null){
				Gizmos.color = Color.gray;
				Gizmos.DrawLine (Upper.position, Middle.position);
				Gizmos.DrawLine (Middle.position, Bottom.position);
				Gizmos.color = Color.red;
				Gizmos.DrawLine (Upper.position, look.position);
				Gizmos.color = Color.blue;
				Gizmos.DrawLine (Middle.position, Joint_Orienter.position);
			}
		}
	}

}
