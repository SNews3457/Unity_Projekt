﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public float dmgValue = 4;
	public GameObject throwableObject;
	public Transform attackCheck;
	private Rigidbody2D m_Rigidbody2D;
	public Animator animator;
	public bool canAttack = true;
    public bool SkillShoot = false; //Dagobert bool zum erlernen des Schießens 
    public bool isTimeToCheck = false;
	public bool fireEffect = false; //Dagobert Feuereffekt 
	public GameObject cam;
	public float AttackCooldownM;
	ModeSwitcher switcher;
	public float darkcooldown;
	
	private void Awake()
	{
		switcher = GetComponent<ModeSwitcher>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	public void attack()
	{
		if(canAttack)
		{
            canAttack = false;
            animator.SetBool("IsAttacking", true);
            StartCoroutine(AttackCooldown());
        }
	}
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			attack();
		}

		if (Input.GetMouseButtonDown(1) && SkillShoot)
		{
            switch (switcher.currentMode)
            {
                case ModeSwitcher.PlayerMode.Light:
					return;
                case ModeSwitcher.PlayerMode.Dark:
                    GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f, -0.2f), Quaternion.identity) as GameObject;
                    Vector2 direction = new Vector2(transform.localScale.x, 0);
                    throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction;
                    throwableWeapon.name = "ThrowableWeapon";
                    break;

            }
		}
	}

	IEnumerator AttackCooldown()
	{
        switch (switcher.currentMode)
        {
            case ModeSwitcher.PlayerMode.Light:
                yield return new WaitForSeconds(AttackCooldownM);
				break;
            case ModeSwitcher.PlayerMode.Dark:
                yield return new WaitForSeconds(darkcooldown);
                break;

        }

		canAttack = true;
	}

    IEnumerator HitStop()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.14f);
        Time.timeScale = 1;
    }


    public void DoDashDamage()
	{
		dmgValue = Mathf.Abs(dmgValue);
		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 1.25f);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy")
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				StartCoroutine(HitStop());
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
				cam.GetComponent<CameraFollow>().ShakeCamera();
			}
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LearnShoot")) //Dagobert schießen erlernen wenn berührt
        {
            SkillShoot = true;
            Destroy(collision.gameObject);
        }
    }
}
