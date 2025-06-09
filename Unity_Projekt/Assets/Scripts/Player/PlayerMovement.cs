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

    private bool isDragging = false;
    private Vector2 aimInput;
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
    public bool SkillDash = false;

    public Attack playerAttack;

    public Menu menu;
    private Camera mainCam;
    public float maxDragDistance = 5f;
    public UIManager uiManager;

    void Start()
    {
        mainCam = Camera.main;
        aimInput = Vector2.zero;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        switcher = GetComponent<ModeSwitcher>();
        attack = GetComponent<Attack>();
        controlls = new PlayerControlls();

        controlls.Gameplay.Map.performed += ctx => uiManager.OpenorClose();
        controlls.Gameplay.SwitchRight.performed += ctx => uiManager.SwitchMenu(1);
        controlls.Gameplay.SwitchLeft.performed += ctx => uiManager.SwitchMenu(-1);
        controlls.Gameplay.Jump.performed += ctx => Jump();
        controlls.Gameplay.Dash.performed += ctx => Dash();
        controlls.Gameplay.Attack.performed += ctx => attack.attack();
        controlls.Gameplay.Switch.performed += ctx => switcher.Switch();
        controlls.Gameplay.Options.performed += ctx => menu.OpenClose();
        controlls.Gameplay.Up.performed += ctx => menu.Up();
        controlls.Gameplay.Down.performed += ctx => menu.Down();
        controlls.Gameplay.Select.performed += ctx => menu.Select();
        controlls.Gameplay.Back.performed += ctx => menu.back();
    }

    void Jump()
    {
        jump = true;
    }

    void Dash()
    {
        if (SkillDash)
            dash = true;
    }

    void OnEnable()
    {
        controlls.Gameplay.Enable();
    }

    void OnDisable()
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
            aimInput = Vector2.zero;
        }

        if (Input.GetMouseButton(1) && isDragging)
        {
            // Relativer Mausinput (funktioniert auch bei gelocktem Cursor)
            float mouseX = Input.GetAxis("Mouse X") * aimSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * aimSensitivity;

            aimInput += new Vector2(mouseX, mouseY);

            if (aimInput.magnitude > maxAimRadius)
                aimInput = aimInput.normalized * maxAimRadius;

            Vector2 direction = aimInput.normalized;
            float power = aimInput.magnitude * 2f;

            trajectoryRenderer.ShowTrajectory(throwPoint.position, direction, power, teleportProjectilePrefab.GetComponent<Rigidbody2D>().gravityScale);
        }

        if (Input.GetMouseButtonUp(1) && isDragging)
        {
            isDragging = false;
            trajectoryRenderer.HideTrajectory();

            Vector2 direction = aimInput.normalized;
            float power = aimInput.magnitude * 2f;

            GameObject proj = Instantiate(teleportProjectilePrefab, throwPoint.position, Quaternion.identity);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * power, ForceMode2D.Impulse);

            proj.GetComponent<TeleportProjectile>().SetPlayer(gameObject);
            lastTeleportTime = Time.time;

            aimInput = Vector2.zero;
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

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
        jump = false;
        dash = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LearnDash"))
        {
            SkillDash = true;
            Destroy(collision.gameObject);
        }
    }
}
