using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float life = 10;
	private bool isPlat;
	private bool isObstacle;
	private Transform fallCheck;
	private Transform wallCheck;
	public LayerMask turnLayerMask;
	private Rigidbody2D rb;

	public GameObject EXP;
	public GameObject Orbs;
	private bool facingRight = true;
	public AchievementManager achievementManager;
	public float speed = 5f;

	public bool isInvincible = false;
	private bool isHitted = false;
	bool die = false;

	void Awake () {
		fallCheck = transform.Find("FallCheck");
		wallCheck = transform.Find("WallCheck");
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

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
			rb.linearVelocity = Vector2.zero;
			rb.AddForce(new Vector2(direction * 500f, 100f));
			StartCoroutine(HitTime());
		}
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

        // Zufällige Anzahl an EXP (1–5)
        int expCount = Random.Range(1, 6);  // Zufällige Zahl zwischen 1 und 5
        for (int i = 0; i < expCount; i++)
        {
            GameObject exp = Instantiate(EXP, transform.position, Quaternion.identity);  // EXP Instanz erstellen
            Rigidbody2D expRb = exp.GetComponent<Rigidbody2D>();  // Rigidbody2D des EXP-Objekts holen
            if (expRb != null)
            {
                // Kraft auf das EXP-Objekt anwenden
                Vector2 force = new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 5f));
                expRb.AddForce(force, ForceMode2D.Impulse);  // Force anwenden
            }
        }

        // Zufällige Anzahl an Orbs (1–5)
        int orbCount = Random.Range(1, 6);  // Zufällige Zahl zwischen 1 und 5
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

        yield return new WaitForSeconds(3f);  // Warten bevor der Feind zerstört wird
        achievementManager.enemiesKilled++;  // Feindeskill-Tracker erhöhen
        Destroy(gameObject);  // Feind zerstören
    }



}
