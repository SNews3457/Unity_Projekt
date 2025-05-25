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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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

        if (Input.GetKeyDown(KeyCode.T) && SkillTeleport)
        {
            isAiming = true;
            aimPosition = transform.position;
        }

        if (Input.GetKey(KeyCode.T) && isAiming)
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            aimPosition += mouseDelta * aimSensitivity * Time.deltaTime;

            // Begrenzung des Zielbereichs um den Spieler herum
            Vector2 center = (Vector2)throwPoint.position;
            Vector2 dir = aimPosition - center;
            if (dir.magnitude > maxAimRadius)
                aimPosition = center + dir.normalized * maxAimRadius;

            // Zielrichtung berechnen und Trajectory anzeigen
            Vector2 direction = (aimPosition - (Vector2)throwPoint.position).normalized;
            trajectoryRenderer.ShowTrajectory(throwPoint.position, direction, force, teleportProjectilePrefab.GetComponent<Rigidbody2D>().gravityScale);

        }

        if (Input.GetKeyUp(KeyCode.T) && isAiming && Time.time >= lastTeleportTime + teleportCooldown)
        {
            isAiming = false;
            trajectoryRenderer.HideTrajectory();

            Vector2 direction = (aimPosition - (Vector2)throwPoint.position).normalized;

            GameObject proj = Instantiate(teleportProjectilePrefab, throwPoint.position, Quaternion.identity);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * force, ForceMode2D.Impulse);


            proj.GetComponent<TeleportProjectile>().SetPlayer(gameObject);

            lastTeleportTime = Time.time;
        }






        /*if (Input.GetAxisRaw("Dash") == 1 || Input.GetAxisRaw("Dash") == -1) //RT in Unity 2017 = -1, RT in Unity 2019 = 1
		{
			if (dashAxis == false)
			{
				dashAxis = true;
				dash = true;
			}
		}
		else
		{
			dashAxis = false;
		}
		*/

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
