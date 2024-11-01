
using System.Collections;
using System.Collections.Generic;
//using Unity.Android.Gradle.Manifest;
using UnityEngine;

//This script requires you to have setup your animator with 3 parameters, "InputMagnitude", "InputX", "InputZ"
//With a blend tree to control the inputmagnitude and allow blending between animations.
[RequireComponent(typeof(CharacterController))]
public class MovementInput : MonoBehaviour {

    public float Velocity;
    [Space]

	public float InputX;
	public float InputZ;
	public Vector3 desiredMoveDirection;
	public bool blockRotationPlayer;
	public float desiredRotationSpeed = 0.1f;
	public Animator anim;
	public float Speed;
	public float allowPlayerRotation = 0.1f;
	public Camera cam;
	public CharacterController controller;
	public bool isGrounded = true;

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    public float VerticalAnimTime = 0.2f;
    [Range(0,1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;


    private Vector3 moveVector;





    [Header("跳跃")]
    [Tooltip("角色跳跃的高度")] public float jumpHeight = 1.5f;
    [Tooltip("判断是否在跳跃")] private bool isJumping;
    [Header("重力")]public float gravity = 9.8f;
    private float _verticalVelocity;

    [Header("地面检测")]
    [Tooltip("地面检测位置")] public Transform groundCheck;
    [Tooltip("地面检测半径")] public float sphereRadius = 0.5f;
    bool isLanded = true;

    [Header("背包控制")]
    public GameObject bagUI;
    private bool isOpenBag = false;


    // Use this for initialization
    void Start () 
    {

		anim = this.GetComponent<Animator> ();
        anim.SetBool("isGround", true);
		cam = Camera.main;
		controller = this.GetComponent<CharacterController> ();

        if(groundCheck == null)
        {
            groundCheck = transform.Find("GroundCheck");
        }

	}
	
	// Update is called once per frame
	void Update () 
    {


		InputMagnitude ();

        OpenOrCloseBagLogic();

        isGrounded = IsGrounded();

        SetJump();

        controller.Move( new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);


    }


    void OpenOrCloseBagLogic()
    {
        isOpenBag = bagUI.activeSelf;
        if (Input.GetKeyDown(KeyCode.E))
        {
            isOpenBag = !isOpenBag;
            bagUI.SetActive(isOpenBag);
        }
    }


    void SetJump()
    {
        bool jump = Input.GetKey(KeyCode.Space);
        if (isGrounded)
        {
            // 在着地时阻止垂直速度无限下降
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -10f;
            }

            if (jump)
            {
                Debug.Log("Jump");
                _verticalVelocity = jumpHeight;
            }
        }
        else
        {
            //随时间施加重力
            //_verticalVelocity += -gravity * Time.deltaTime;
        }
       _verticalVelocity += -gravity * Time.deltaTime;
    }

    bool IsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(groundCheck.position, sphereRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && !IsChildOf(collider.transform, transform)) // 忽略角色自身和所有子集碰撞体
            {
                return true;
            }
        }
        return false;
    }

    //判断child是否是parent的子集
    bool IsChildOf(Transform child, Transform parent)
    {
        while (child != null)
        {
            if (child == parent)
            {
                return true;
            }
            child = child.parent;
        }
        return false;
    }

    //在场景视图显示检测，方便调试
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //地面检测可视化
        Gizmos.DrawWireSphere(groundCheck.position, sphereRadius);
    }


    void PlayerMoveAndRotation() {
		InputX = Input.GetAxis ("Horizontal");
		InputZ = Input.GetAxis ("Vertical");

		var camera = Camera.main;
		var forward = cam.transform.forward;
		var right = cam.transform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize ();
		right.Normalize ();

		desiredMoveDirection = forward * InputZ + right * InputX;

		if (blockRotationPlayer == false) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), desiredRotationSpeed);
            controller.Move(desiredMoveDirection * Time.deltaTime * Velocity);
		}
	}

    public void LookAt(Vector3 pos)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), desiredRotationSpeed);
    }

    public void RotateToCamera(Transform t)
    {

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        desiredMoveDirection = forward;

        t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
    }

    void IsLandTrue()
    {
        isLanded = true;
    }

	void InputMagnitude() 
	{
		InputX = Input.GetAxis ("Horizontal");
		InputZ = Input.GetAxis ("Vertical");

		Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        anim.SetBool("isGround", isGrounded);

        //if (!isGrounded)
        //{
        //    anim.SetFloat("Blend", Vector3.Distance(controller.velocity, Vector3.zero));
        //}

        if (Speed > allowPlayerRotation ) 
        {
			anim.SetFloat ("Blend", Speed, StartAnimTime, Time.deltaTime);
			PlayerMoveAndRotation ();
		} 
        else if (Speed < allowPlayerRotation ) 
        {
			anim.SetFloat ("Blend", Speed, StopAnimTime, Time.deltaTime);
		}
	}
}
