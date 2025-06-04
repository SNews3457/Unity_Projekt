using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class PlayerMovement : MonoBehaviour 
{
    PlayerControlls controlls;

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
    Attack attack;
	public Animator animator;

	public float runSpeed = 40f;
    ModeSwitcher switcher;
	float horizontalMove = 0f;
	bool jump = false;
	bool dash = false;
	public bool SkillDash = false; //Dagobert bool zum erlernen des Dashs 
	public Attack playerAttack;
    //bool dashAxis = false;

    Options options;
    private bool isDragging = false;
    private Camera mainCam;
    public float maxDragDistance = 5f;

    void Start()
    {
        mainCam = Camera.main;
        aimPosition = transform.position;
    }

    private void Awake()
    {
        switcher = GetComponent<ModeSwitcher>();
        attack = GetComponent<Attack>();
        controlls = new PlayerControlls();
        options = GetComponent<Options>();

        controlls.Gameplay.Jump.performed += ctx => Jump(); //Dagobert Gamepad Steuerung ctx = lehre Funktion da die Werte nicht benötigt werden
        controlls.Gameplay.Dash.performed += ctx => Dash();
        controlls.Gameplay.Attack.performed += ctx => attack.attack();
        controlls.Gameplay.Switch.performed += ctx => switcher.Switch();
        controlls.Gameplay.Options.performed += ctx => options.OpenClose();
    }

    void Jump()
    {
        jump = true; //Dagobert die Inputs müssen aufgrund der Gamepad Steuerung über Funktionen laufen
    }

    void Dash()
    {
        if(SkillDash)
            dash = true;
    }

    //Dagobert Enable und Dissable aus Unity, damit die Gamepadeingaben korrekt erkannt werden
    void OnEnable()
    {
        controlls.Gameplay.Enable();
    }

    void OnDissable()
    {
        controlls.Gameplay.Disable();
    }

    void Update()
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetKeyDown(KeyCode.Space))
		{
            Jump();
		}

		if (Input.GetKeyDown(KeyCode.LeftShift) && SkillDash)
		{
			Dash();
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
