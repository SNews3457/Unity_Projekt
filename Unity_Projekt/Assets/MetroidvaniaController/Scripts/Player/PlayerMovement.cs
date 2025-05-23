using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
	private float teleportCooldown = 3f;
    private float lastTeleportTime = -Mathf.Infinity;

	public bool SkillTeleport = true; // optional: Fähigkeit erlernen

	public CharacterController2D controller;
	
	public Animator animator;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool dash = false;
	public bool SkillDash = false; //Dagobert bool zum erlernen des Dashs 
	public Attack playerAttack;
	//bool dashAxis = false;

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
		
		if (Input.GetKeyDown(KeyCode.T) && Time.time >= lastTeleportTime + teleportCooldown)
        {
            controller.TeleportForward();
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
