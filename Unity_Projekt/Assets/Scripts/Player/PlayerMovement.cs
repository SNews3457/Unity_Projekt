using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    public GameObject teleportProjectilePrefab;
    public Transform throwPoint;
    public TrajectoryRenderer trajectoryRenderer;

    private bool isAiming = false;
    private Vector2 aimPosition;
    public float aimSensitivity = 5f;
    public float maxAimRadius = 5f;
    float force = 10f; 
    private float teleportCooldown = 3f;
    private float lastTeleportTime = -Mathf.Infinity;

	public bool SkillTeleport = false;

	public CharacterController2D controller;
	
	public Animator animator;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool dash = false;
	public bool SkillDash = false; //Dagobert bool zum erlernen des Dashs 
	public Attack playerAttack;
    //bool dashAxis = false;


    private bool isDragging = false;
    private Camera mainCam;
    public float maxDragDistance = 5f;

    void Start()
    {
        mainCam = Camera.main;
        aimPosition = transform.position;
    }




    // Update is called once per frame
    void Update()
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetKeyDown(KeyCode.Space))
		{
			jump = true;
		}

		if (Input.GetKeyDown(KeyCode.LeftShift) && SkillDash)
		{
			dash = true;
		}

        if (Input.GetMouseButtonDown(1) && SkillTeleport && Time.time >= lastTeleportTime + teleportCooldown)
        {
            isDragging = true;
            aimPosition = transform.position;
        }

        if (Input.GetMouseButton(1) && isDragging)
        {
            Vector3 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;

            Vector2 center = throwPoint.position;
            Vector2 dragVector = center - (Vector2)mouseWorld;

            if (dragVector.magnitude > maxDragDistance)
                dragVector = dragVector.normalized * maxDragDistance;

            aimPosition = center - dragVector;

            Vector2 direction = dragVector.normalized;
            float power = dragVector.magnitude * 2f;

            trajectoryRenderer.ShowTrajectory(throwPoint.position, direction, power, teleportProjectilePrefab.GetComponent<Rigidbody2D>().gravityScale);
        }

        if (Input.GetMouseButtonUp(1) && isDragging)
        {
            isDragging = false;
            trajectoryRenderer.HideTrajectory();

            Vector2 center = throwPoint.position;
            Vector2 dragVector = center - (Vector2)aimPosition;
            Vector2 direction = dragVector.normalized;
            float power = dragVector.magnitude * 2f;

            GameObject proj = Instantiate(teleportProjectilePrefab, throwPoint.position, Quaternion.identity);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * power, ForceMode2D.Impulse);

            proj.GetComponent<TeleportProjectile>().SetPlayer(gameObject);
            lastTeleportTime = Time.time;
        }

    }

    public void OnFall()
	{
		animator.SetBool("IsJumping", true);
	}

	public void OnLanding()
	{
		animator.SetBool("IsJumping", false);
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
		jump = false;
		dash = false;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("LearnDash")) //Dagobert dashen erlernen wenn ber�hrt
		{
			SkillDash = true;
			
			Destroy(collision.gameObject);
		}

    }
}
