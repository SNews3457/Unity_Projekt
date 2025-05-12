using UnityEngine;
using System.Collections;

public class EnemyF : MonoBehaviour
{
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

    // == Fernkampf ==
    public Transform firePoint; // Abschusspunkt für Projektil
    public GameObject projectilePrefab; // Projektilprefab
    public float attackRange = 10f;
    public float shootCooldown = 2f;
    private float shootTimer = 0f;
    private Transform player;

    void Awake()
    {
        fallCheck = transform.Find("FallCheck");
        wallCheck = transform.Find("WallCheck");
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
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
            if (isPlat && !isObstacle)
            {
                if (facingRight)
                    rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
                else
                    rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            }
            else
            {
                Flip();
            }
        }

        // == Fernkampfverhalten ==
        if (player != null && life > 0)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= attackRange)
            {
                shootTimer -= Time.fixedDeltaTime;
                if (shootTimer <= 0f)
                {
                    ShootAtPlayer();
                    shootTimer = shootCooldown;
                }

                // Gegner richtet sich zum Spieler aus
                if ((player.position.x < transform.position.x && facingRight) ||
                    (player.position.x > transform.position.x && !facingRight))
                {
                    Flip();
                }
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void ApplyDamage(float damage)
    {
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

    void ShootAtPlayer()
{
    if (projectilePrefab == null || firePoint == null || player == null)
    {
        Debug.LogWarning("Fehlende Referenz beim Schießen (Projectile, FirePoint oder Player)");
        return;
    }

    Vector2 direction = (player.position - firePoint.position).normalized;
    GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
    ThrowableProjectile tp = projectile.GetComponent<ThrowableProjectile>();
    tp.direction = direction;
    tp.owner = this.gameObject;
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

        int expCount = Random.Range(1, 6);
        for (int i = 0; i < expCount; i++)
        {
            GameObject exp = Instantiate(EXP, transform.position, Quaternion.identity);
            Rigidbody2D expRb = exp.GetComponent<Rigidbody2D>();
            if (expRb != null)
            {
                Vector2 force = new Vector2(Random.Range(-3f, 3f), Random.Range(2f, 7f));
                expRb.AddForce(force, ForceMode2D.Impulse);
            }
        }

        int orbCount = Random.Range(1, 6);
        for (int i = 0; i < orbCount; i++)
        {
            GameObject orb = Instantiate(Orbs, transform.position, Quaternion.identity);
            Rigidbody2D orbRb = orb.GetComponent<Rigidbody2D>();
            if (orbRb != null)
            {
                Vector2 force = new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 5f));
                orbRb.AddForce(force, ForceMode2D.Impulse);
            }
        }

        yield return new WaitForSeconds(3f);
        achievementManager.enemiesKilled++;
        Destroy(gameObject);
    }
}
