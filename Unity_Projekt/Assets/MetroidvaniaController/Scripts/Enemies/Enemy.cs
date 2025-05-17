using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float life = 10;
    protected bool isPlat;
    protected bool isObstacle;
    protected Transform fallCheck;
    protected Transform wallCheck;
	public LayerMask turnLayerMask;
    protected Rigidbody2D rb;

	public GameObject EXP;
	public GameObject Orbs;
    protected bool facingRight = true;
	public AchievementManager achievementManager;
	public float speed = 5f;
	public Attack Attack;
    protected bool isBurning = false;
    public bool isInvincible = false;
    protected bool isHitted = false;
    protected bool die = false;
    protected SpriteRenderer spriteRenderer;


    void Awake()
    {
        fallCheck = transform.Find("FallCheck");
        wallCheck = transform.Find("WallCheck");
        rb = GetComponent<Rigidbody2D>();
		Attack = FindAnyObjectByType<Attack>();
		achievementManager = FindAnyObjectByType<AchievementManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void FixedUpdate () 
    {
        Cursor.visible = true;

		if (life <= 0 && !die)
		{
			die = true;
			transform.GetComponent<Animator>().SetBool("IsDead", true);
			
			StartCoroutine(DestroyEnemy());
		}

		isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Default"));
		isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);

		if (!isHitted && life > 0 && Mathf.Abs(rb.linearVelocity.y) < 0.5f)
		{
			if (isPlat && !isObstacle && !isHitted)
			{
				if (facingRight)
				{
					rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
				}
				else
				{
					rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
				}
			}
			else
			{
				Flip();
			}
		}
	}


	void Flip (){
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void ApplyDamage(float damage) {
		if (!isInvincible) 
		{
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			transform.GetComponent<Animator>().SetBool("Hit", true);
			life -= damage;

			if(Attack.fireEffect) //Dagobert Wenn das Schwert einen FlammenEffekt hat, dann Schaden �ber Zeit
			{
				StartCoroutine(Burn());
			}

			rb.linearVelocity = Vector2.zero;
			rb.AddForce(new Vector2(direction * 500f, 100f));
			StartCoroutine(HitTime());
		}
	}


    IEnumerator Burn()
    {
        if (isBurning) yield break;
        isBurning = true;

        int ticks = 7;
        float burnDamage = 0.3f;
        float interval = 0.3f;

        Color originalColor = spriteRenderer.color;
        Color burnColor = new Color(1f, 0.4f, 0.1f); // rötlich-orange

        for (int i = 0; i < ticks; i++)
        {
            // Visueller Flackereffekt
            spriteRenderer.color = burnColor;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = originalColor;

            // Schaden zufügen
            if (life <= 0) break;
            life -= burnDamage;

            yield return new WaitForSeconds(interval - 0.15f);
        }

        spriteRenderer.color = originalColor;
        isBurning = false;
    }
    void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(2f, transform.position);
		}
	}

	IEnumerator HitTime()
	{
		isHitted = true;
		isInvincible = true;
		yield return new WaitForSeconds(0.1f);
		isHitted = false;
		isInvincible = false;
	}

    IEnumerator DestroyEnemy()
    {
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        capsule.size = new Vector2(1f, 0.25f);
        capsule.offset = new Vector2(0f, -0.8f);
        capsule.direction = CapsuleDirection2D.Horizontal;

        yield return new WaitForSeconds(0.25f);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // Zuf�llige Anzahl an EXP (1�5)
        int expCount = Random.Range(1, 6);  // Zuf�llige Zahl zwischen 1 und 5
        for (int i = 0; i < expCount; i++)
        {
            GameObject exp = Instantiate(EXP, transform.position, Quaternion.identity);  // EXP Instanz erstellen
            Rigidbody2D expRb = exp.GetComponent<Rigidbody2D>();  // Rigidbody2D des EXP-Objekts holen
            if (expRb != null)
            {
                // Kraft auf das EXP-Objekt anwenden
                Vector2 force = new Vector2(Random.Range(-3f, 3f), Random.Range(2f, 7f));
                expRb.AddForce(force, ForceMode2D.Impulse);  // Force anwenden
            }
        }

        // Zuf�llige Anzahl an Orbs (1�5)
        int orbCount = Random.Range(1, 6);  // Zuf�llige Zahl zwischen 1 und 5
        for (int i = 0; i < orbCount; i++)
        {
            GameObject orb = Instantiate(Orbs, transform.position, Quaternion.identity);  // Orb Instanz erstellen
            Rigidbody2D orbRb = orb.GetComponent<Rigidbody2D>();  // Rigidbody2D des Orbs holen
            if (orbRb != null)
            {
                // Kraft auf das Orb-Objekt anwenden
                Vector2 force = new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 5f));
                orbRb.AddForce(force, ForceMode2D.Impulse);  // Force anwenden
            }
        }

        yield return new WaitForSeconds(3f);  // Warten bevor der Feind zerst�rt wird
        achievementManager.enemiesKilled++;  // Feindeskill-Tracker erh�hen
        Destroy(gameObject);  // Feind zerst�ren
    }



}
