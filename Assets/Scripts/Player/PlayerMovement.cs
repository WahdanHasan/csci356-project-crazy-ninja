using System;
using Audio;
using EZCameraShake;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public static PlayerMovement Instance { get; private set; }

	private void Awake()
	{
		PlayerMovement.Instance = this;
		this.rb = base.GetComponent<Rigidbody>();
	}

	private void Start()
	{
		this.psEmission = this.ps.emission;
		this.playerCollider = base.GetComponent<Collider>();
		this.detectWeapons = (DetectWeapons)base.GetComponentInChildren(typeof(DetectWeapons));
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		this.readyToJump = true;
		this.wallNormalVector = Vector3.up;
		this.CameraShake();
		if (this.spawnWeapon != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.spawnWeapon, base.transform.position, Quaternion.identity);
			this.detectWeapons.ForcePickup(gameObject);
		}
        this.jet = GameObject.FindGameObjectWithTag("Jetpack").GetComponent<Jetpack>();
        this.UpdateSensitivity();
	}

	public void UpdateSensitivity()
	{
		if (!GameState.Instance)
		{
			return;
		}
		this.sensMultiplier = GameState.Instance.GetSensitivity();
	}

	private void LateUpdate()
	{
		if (this.dead || this.paused)
		{
			return;
		}
		//this.DrawGrabbing();
		this.WallRunning();
	}

	private void FixedUpdate()
	{
        if (this.dead || Game.Instance.done || this.paused)
        {
            return;
        }
        this.Movement();
	}

	private void Update()
	{
		this.UpdateActionMeter();
		this.MyInput();
        if (this.dead || Game.Instance.done || this.paused)
        {
            return;
        }
        this.Look();
		//this.DrawGrabbing();
		this.UpdateTimescale();
		if (base.transform.position.y < -200f)
		{
			this.KillPlayer();
		}
	}

	private void MyInput()
	{
        if (this.dead || Game.Instance.done)
        {
            return;
        }
        this.x = Input.GetAxisRaw("Horizontal");
		this.y = Input.GetAxisRaw("Vertical");
		this.jumping = Input.GetButton("Jump");
		this.crouching = Input.GetButton("Crouch");
		if (Input.GetButtonDown("Cancel"))
		{
			this.Pause();
		}
        if (Input.GetButtonDown("LShift") && (Input.GetKey("w") || Input.GetKey("s")))
        {

            this.Dash(true);
        }
        else if (Input.GetButtonDown("LShift") && (Input.GetKey("a") || Input.GetKey("d")))
        {
            this.Dash(false);
        }
        
        if (this.paused)
		{
			return;
		}
		if (Input.GetButtonDown("Crouch"))
		{
			this.StartCrouch();
		}
		if (Input.GetButtonUp("Crouch"))
		{
			this.StopCrouch();
		}
		if (Input.GetButton("Fire1") && can_fire)
		{
			if (this.detectWeapons.HasGun())
			{
				this.detectWeapons.Shoot(this.HitPoint());
			}
		}

	}

	private void Pause()
	{
		if (this.dead)
		{
			return;
		}
		if (this.paused)
		{
			Time.timeScale = 1f;
			UIManger.Instance.DeadUI(false);
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			this.paused = false;
			return;
		}
		this.paused = true;
		Time.timeScale = 0f;
        UIManger.Instance.DeadUI(true);
        Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	private void UpdateTimescale()
	{
        if (Game.Instance.done || this.paused || this.dead)
        {
            return;
        }
        Time.timeScale = Mathf.SmoothDamp(Time.timeScale, this.desiredTimeScale, ref this.timeScaleVel, 0.15f);
	}

	private void StartCrouch()
	{
        if (!this.grounded)
        {
            return;
        }
		float d = 400f;
		base.transform.localScale = new Vector3(1f, 0.5f, 1f);
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 0.5f, base.transform.position.z);
		if (this.rb.velocity.magnitude > 0.1f && this.grounded)
		{
			this.rb.AddForce(this.orientation.transform.forward * d);
			AudioManager.Instance.Play("StartSlide");
			AudioManager.Instance.Play("Slide");
		}
        this.readyToJump = false;
	}

	private void StopCrouch()
	{
        if (!this.grounded)
        {
            return;
        }
        base.transform.localScale = new Vector3(1f, 1.5f, 1f);
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z);
        this.ResetJump();
	}

	private void FootSteps()
	{
		if (this.crouching || this.dead)
		{
			return;
		}
		if (this.grounded || this.wallRunning)
		{
			float num = 1.2f;
			float num2 = this.rb.velocity.magnitude;
			if (num2 > 20f)
			{
				num2 = 20f;
			}
			this.distance += num2;
			if (this.distance > 300f / num)
			{
				AudioManager.Instance.PlayFootStep();
				this.distance = 0f;
			}
		}
	}

	private void Movement()
	{
		if (this.dead)
		{
			return;
		}
		this.rb.AddForce(Vector3.down * Time.deltaTime * 10f);
		Vector2 vector = this.FindVelRelativeToLook();
		float num = vector.x;
		float num2 = vector.y;
		this.FootSteps();
		this.CounterMovement(this.x, this.y, vector);
		if (this.readyToJump && this.jumping)
		{
			this.Jump();
		}
		float num3 = this.walkSpeed;
		if (this.sprinting)
		{
			num3 = this.runSpeed;
		}
		if (this.x > 0f && num > num3)
		{
			this.x = 0f;
		}
		if (this.x < 0f && num < -num3)
		{
			this.x = 0f;
		}
		if (this.y > 0f && num2 > num3)
		{
			this.y = 0f;
		}
		if (this.y < 0f && num2 < -num3)
		{
			this.y = 0f;
		}
		float d = 1f;
		float d2 = 1f;
		if (!this.grounded)
		{
			d = 0.5f;
			d2 = 0.5f;
		}
        if (this.grounded && this.crouching)
		{
			d2 = 0.25f;
		}
		if (this.wallRunning)
		{
			d2 = 0.3f;
			d = 0.3f;
		}
		if (this.surfing)
		{
			d = 0.7f;
			d2 = 0.3f;
		}
		this.rb.AddForce(this.orientation.transform.forward * this.y * this.moveSpeed * Time.deltaTime * d * d2);
		this.rb.AddForce(this.orientation.transform.right * this.x * this.moveSpeed * Time.deltaTime * d);
		this.SpeedLines();
	}

	private void SpeedLines()
	{
		float num = Vector3.Angle(this.rb.velocity, this.playerCam.transform.forward) * 0.15f;
		if (num < 1f)
		{
			num = 1f;
		}
		float rateOverTimeMultiplier = this.rb.velocity.magnitude / num;
		if (this.grounded && !this.wallRunning)
		{
			rateOverTimeMultiplier = 0f;
		}
		this.psEmission.rateOverTimeMultiplier = rateOverTimeMultiplier;
	}

	private void CameraShake()
	{
		float num = this.rb.velocity.magnitude / 9f;
		CameraShaker.Instance.ShakeOnce(num, 0.1f * num, 0.25f, 0.2f);
		base.Invoke("CameraShake", 0.2f);
	}

	private void Jump()
	{
		if ((this.grounded || this.wallRunning || this.surfing) && this.readyToJump)
		{
			Vector3 velocity = this.rb.velocity;
			this.readyToJump = false;
			this.rb.AddForce(Vector2.up * this.jumpForce * 1.5f);
			this.rb.AddForce(this.normalVector * this.jumpForce * 0.5f);
			if (this.rb.velocity.y < 0.5f)
			{
				this.rb.velocity = new Vector3(velocity.x, 0f, velocity.z);
			}
			else if (this.rb.velocity.y > 0f)
			{
				this.rb.velocity = new Vector3(velocity.x, velocity.y / 2f, velocity.z);
			}
			if (this.wallRunning)
			{
				this.rb.AddForce(this.wallNormalVector * this.jumpForce * 3f);
			}
			base.Invoke("ResetJump", this.jumpCooldown);
			if (this.wallRunning)
			{
				this.wallRunning = false;
			}
			AudioManager.Instance.PlayJump();
		}
	}
    public void Dash(bool state)
    {
        if (this.grounded && jet.canDash == true)
        {
            if (state == true)
            {
                this.rb.AddForce(this.orientation.transform.forward * this.y * this.moveSpeed * Time.deltaTime * 50 * 50);
            }
            else if (state == false)
            {
                this.rb.AddForce(this.orientation.transform.right * this.x * this.moveSpeed * Time.deltaTime * 50 * 50);
            }

        }
    }


    private void Look()
	{
		float num = Input.GetAxis("Mouse X") * this.sensitivity * Time.fixedDeltaTime * this.sensMultiplier;
		float num2 = Input.GetAxis("Mouse Y") * this.sensitivity * Time.fixedDeltaTime * this.sensMultiplier;
		Vector3 eulerAngles = this.playerCam.transform.localRotation.eulerAngles;
		this.desiredX = eulerAngles.y + num;
		this.xRotation -= num2;
		this.xRotation = Mathf.Clamp(this.xRotation, -90f, 90f);
		this.FindWallRunRotation();
		this.actualWallRotation = Mathf.SmoothDamp(this.actualWallRotation, this.wallRunRotation, ref this.wallRotationVel, 0.2f);
		this.playerCam.transform.localRotation = Quaternion.Euler(this.xRotation, this.desiredX, this.actualWallRotation);
		this.orientation.transform.localRotation = Quaternion.Euler(0f, this.desiredX, 0f);
	}

	private void CounterMovement(float x, float y, Vector2 mag)
	{
		if (!this.grounded || this.jumping || this.exploded)
		{
			return;
		}
		float d = 0.16f;
		float num = 0.01f;
		if (this.crouching)
		{
			this.rb.AddForce(this.moveSpeed * Time.deltaTime * -this.rb.velocity.normalized * this.slideSlowdown);
			return;
		}
		if ((Math.Abs(mag.x) > num && Math.Abs(x) < 0.05f) || (mag.x < -num && x > 0f) || (mag.x > num && x < 0f))
		{
			this.rb.AddForce(this.moveSpeed * this.orientation.transform.right * Time.deltaTime * -mag.x * d);
		}
		if ((Math.Abs(mag.y) > num && Math.Abs(y) < 0.05f) || (mag.y < -num && y > 0f) || (mag.y > num && y < 0f))
		{
			this.rb.AddForce(this.moveSpeed * this.orientation.transform.forward * Time.deltaTime * -mag.y * d);
		}
		if (Mathf.Sqrt(Mathf.Pow(this.rb.velocity.x, 2f) + Mathf.Pow(this.rb.velocity.z, 2f)) > this.walkSpeed)
		{
			float num2 = this.rb.velocity.y;
			Vector3 vector = this.rb.velocity.normalized * this.walkSpeed;
			this.rb.velocity = new Vector3(vector.x, num2, vector.z);
		}
	}

	public Vector2 FindVelRelativeToLook()
	{
		float current = this.orientation.transform.eulerAngles.y;
		float target = Mathf.Atan2(this.rb.velocity.x, this.rb.velocity.z) * 57.29578f;
		float num = Mathf.DeltaAngle(current, target);
		float num2 = 90f - num;
		float magnitude = this.rb.velocity.magnitude;
		float num3 = magnitude * Mathf.Cos(num * 0.017453292f);
		return new Vector2(magnitude * Mathf.Cos(num2 * 0.017453292f), num3);
	}

	private void FindWallRunRotation()
	{
		if (!this.wallRunning)
		{
			this.wallRunRotation = 0f;
			return;
		}
		Vector3 normalized = new Vector3(0f, this.playerCam.transform.rotation.y, 0f).normalized;
		new Vector3(0f, 0f, 1f);
		float current = this.playerCam.transform.rotation.eulerAngles.y;
		if (Math.Abs(this.wallNormalVector.x - 1f) >= 0.1f)
		{
			if (Math.Abs(this.wallNormalVector.x - -1f) >= 0.1f)
			{
				if (Math.Abs(this.wallNormalVector.z - 1f) >= 0.1f)
				{
					if (Math.Abs(this.wallNormalVector.z - -1f) < 0.1f)
					{
					}
				}
			}
		}
		float target = Vector3.SignedAngle(new Vector3(0f, 0f, 1f), this.wallNormalVector, Vector3.up);
		float num = Mathf.DeltaAngle(current, target);
		this.wallRunRotation = -(num / 90f) * 15f;
		if (!this.readyToWallrun)
		{
			return;
		}
		if ((Mathf.Abs(this.wallRunRotation) >= 4f || this.y <= 0f || Math.Abs(this.x) >= 0.1f) && (Mathf.Abs(this.wallRunRotation) <= 22f || this.y >= 0f || Math.Abs(this.x) >= 0.1f))
		{
			this.cancelling = false;
			base.CancelInvoke("CancelWallrun");
			return;
		}
		if (this.cancelling)
		{
			return;
		}
		this.cancelling = true;
		base.CancelInvoke("CancelWallrun");
		base.Invoke("CancelWallrun", 0.2f);
	}

	private void WallRunning()
	{
		if (this.wallRunning)
		{
			this.rb.AddForce(-this.wallNormalVector * Time.deltaTime * this.moveSpeed);
			this.rb.AddForce(Vector3.up * Time.deltaTime * this.rb.mass * 100f * this.wallRunGravity);
		}
	}

	private bool IsFloor(Vector3 v)
	{
		return Vector3.Angle(Vector3.up, v) < this.maxSlopeAngle;
	}

	private bool IsSurf(Vector3 v)
	{
		float num = Vector3.Angle(Vector3.up, v);
		return num < 89f && num > this.maxSlopeAngle;
	}

	private bool IsWall(Vector3 v)
	{
		return Math.Abs(90f - Vector3.Angle(Vector3.up, v)) < 0.1f;
	}

	private bool IsRoof(Vector3 v)
	{
		return v.y == -1f;
	}

	private void StartWallRun(Vector3 normal)
	{
		if (this.grounded || !this.readyToWallrun)
		{
			return;
		}
		this.wallNormalVector = normal;
		float d = 20f;
		if (!this.wallRunning)
		{
			this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
			this.rb.AddForce(Vector3.up * d, ForceMode.Impulse);
		}
		this.wallRunning = true;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			this.KillEnemy(other);
		}
	}

	private void OnCollisionExit(Collision other)
	{
	}

	private void OnCollisionStay(Collision other)
	{
		int layer = other.gameObject.layer;
		if (this.whatIsGround != (this.whatIsGround | 1 << layer))
		{
			return;
		}
		for (int i = 0; i < other.contactCount; i++)
		{
			Vector3 normal = other.contacts[i].normal;
			if (this.IsFloor(normal))
			{
				if (this.wallRunning)
				{
					this.wallRunning = false;
				}
				if (!this.grounded && this.crouching)
				{
					AudioManager.Instance.Play("StartSlide");
					AudioManager.Instance.Play("Slide");
				}
				this.grounded = true;
				this.normalVector = normal;
				this.cancellingGrounded = false;
				base.CancelInvoke("StopGrounded");
			}
			if (this.IsWall(normal) && layer == LayerMask.NameToLayer("Ground"))
			{
				if (!this.onWall)
				{
					AudioManager.Instance.Play("StartSlide");
					AudioManager.Instance.Play("Slide");
				}
				this.StartWallRun(normal);
				this.onWall = true;
				this.cancellingWall = false;
				base.CancelInvoke("StopWall");
			}
			if (this.IsSurf(normal))
			{
				this.surfing = true;
				this.cancellingSurf = false;
				base.CancelInvoke("StopSurf");
			}
			this.IsRoof(normal);
		}
		float num = 3f;
		if (!this.cancellingGrounded)
		{
			this.cancellingGrounded = true;
			base.Invoke("StopGrounded", Time.deltaTime * num);
		}
		if (!this.cancellingWall)
		{
			this.cancellingWall = true;
			base.Invoke("StopWall", Time.deltaTime * num);
		}
		if (!this.cancellingSurf)
		{
			this.cancellingSurf = true;
			base.Invoke("StopSurf", Time.deltaTime * num);
		}
	}

	private void KillEnemy(Collision other)
	{
		if (this.grounded && !this.crouching)
		{
			return;
		}
		if (this.rb.velocity.magnitude < 3f)
		{
			return;
		}
		Enemy enemy = (Enemy)other.transform.root.GetComponent(typeof(Enemy));
		if (!enemy)
		{
			return;
		}
		if (enemy.IsDead())
		{
			return;
		}
		UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.enemyHitAudio, other.contacts[0].point, Quaternion.identity);
		RagdollController ragdollController = (RagdollController)other.transform.root.GetComponent(typeof(RagdollController));
		if (this.grounded && this.crouching)
		{
			ragdollController.MakeRagdoll(this.rb.velocity * 1.2f * 34f);
		}
		else
		{
			ragdollController.MakeRagdoll(this.rb.velocity.normalized * 250f);
		}
		this.rb.AddForce(this.rb.velocity.normalized * 2f, ForceMode.Impulse);
		enemy.DropGun(this.rb.velocity.normalized * 2f);
	}

	public float GetFallSpeed()
	{
		return this.rb.velocity.y;
	}

	public Collider GetPlayerCollider()
	{
		return this.playerCollider;
	}

	public Vector3 HitPoint()
	{
		RaycastHit[] array = Physics.RaycastAll(this.playerCam.transform.position, this.playerCam.transform.forward, (float)this.whatIsHittable);
		if (array.Length < 1)
		{
			return this.playerCam.transform.position + this.playerCam.transform.forward * 100f;
		}
		if (array.Length > 1)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
				{
					return array[i].point;
				}
			}
		}
		return array[0].point;
	}

	public float GetRecoil()
	{
		return this.detectWeapons.GetRecoil();
	}

	public void KillPlayer()
	{
        if (Game.Instance.done)
        {
            return;
        }
        //CameraShaker.Instance.ShakeOnce(3f * GameState.Instance.cameraShake, 2f, 0.1f, 0.6f);
        Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
        UIManger.Instance.DeadUI(true);
        Timer.Instance.Stop();
		this.dead = true;
		this.rb.freezeRotation = false;
		this.playerCollider.material = this.deadMat;
		//this.detectWeapons.Throw(Vector3.zero);
		this.paused = false;
		this.ResetSlowmo();
	}

	public void Slowmo(float timescale, float length)
	{
        if (!GameState.Instance.slowmo)
        {
            return;
        }
        base.CancelInvoke("Slowmo");
		this.desiredTimeScale = timescale;
		base.Invoke("ResetSlowmo", length);
		AudioManager.Instance.Play("SlowmoStart");
	}

	private void ResetSlowmo()
	{
		this.desiredTimeScale = 1f;
		AudioManager.Instance.Play("SlowmoEnd");
	}

	public bool HasGun()
	{
		return this.detectWeapons.HasGun();
	}

	public bool IsDead()
	{
		return this.dead;
	}

	public Rigidbody GetRb()
	{
		return this.rb;
	}

	private void UpdateActionMeter()
	{
		float target = 0.09f;
		if (this.rb.velocity.magnitude > 15f && (!this.dead || !Game.Instance.playing))
		{
			target = 1f;
		}
		this.actionMeter = Mathf.SmoothDamp(this.actionMeter, target, ref this.vel, 0.7f);
	}

	public float GetActionMeter()
	{
		return this.actionMeter * 22000f;
	}

	private void StopWall()
	{
		this.onWall = false;
		this.wallRunning = false;
	}

	private void StopSurf()
	{
		this.surfing = false;
	}

	private void StopGrounded()
	{
		this.grounded = false;
	}

	private void ResetJump()
	{
		this.readyToJump = true;
	}

	private void CancelWallrun()
	{
		MonoBehaviour.print("cancelled");
		base.Invoke("GetReadyToWallrun", 0.1f);
		this.rb.AddForce(this.wallNormalVector * 600f);
		this.readyToWallrun = false;
		AudioManager.Instance.PlayLanding();
	}

	public void SetMouse(Vector3 look_delta)
	{
		lock (this)
		{
			this.playerCam.transform.localRotation = Quaternion.Euler(this.xRotation, this.desiredX + look_delta.y, 1);
			this.orientation.transform.localRotation = Quaternion.Euler(this.xRotation, this.desiredX + look_delta.y, 1);
		}
	}

    private Jetpack jet;

	public GameObject spawnWeapon;

	private float sensitivity = 50f;

	private float sensMultiplier = 1f;

	private bool dead;

	public PhysicMaterial deadMat;

	public Transform playerCam;

	public Transform orientation;

	private float xRotation;

	public Rigidbody rb;

	private float moveSpeed = 4500f;

	private float walkSpeed = 20f;

	private float runSpeed = 10f;

	public bool grounded;

	public Transform groundChecker;

	public LayerMask whatIsGround;

	public LayerMask whatIsWallrunnable;

	private bool readyToJump;

	private float jumpCooldown = 0.25f;

	private float jumpForce = 550f;

	private float x;

	private float y;

	private bool jumping;

	private bool sprinting;

	private bool crouching;

	public LineRenderer lr;

	private Vector3 normalVector;

	private Vector3 wallNormalVector;

	private bool wallRunning;

	private DetectWeapons detectWeapons;

	public ParticleSystem ps;

	private ParticleSystem.EmissionModule psEmission;

	private Collider playerCollider;

	public bool exploded;

	public bool paused;

	public LayerMask whatIsGrabbable;

	private float distance;

	private float slideSlowdown = 0.2f;

	private float actualWallRotation;

	private float wallRotationVel;

	private float desiredX;

	private bool cancelling;

	private bool readyToWallrun = true;

	private float wallRunGravity = 1f;

	private float maxSlopeAngle = 35f;

	private float wallRunRotation;

	private bool onWall;

	private bool surfing;

	private bool cancellingGrounded;

	private bool cancellingWall;

	private bool cancellingSurf;

	public LayerMask whatIsHittable;

	private float desiredTimeScale = 1f;

	private float timeScaleVel;

	private float actionMeter;

	private float vel;

	public bool can_fire = true;
}