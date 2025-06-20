using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_WallCheck;

    public AudioClip teleportSound;
    private AudioSource audioSource;
    public float teleportDistance = 3f;
    public float teleportDuration = 0.2f;
    public SpriteRenderer spriteRenderer;
    public Camera mainCamera;
    private bool isTeleporting = false;

    private Transform lastCheckpoint;
    private SaveGroundSaver savegroundsaver;
    public int lives = 5;
    public Transform checkpoint;
    public bool CheckpointActive = false;

    const float k_GroundedRadius = .2f;
    private bool m_Grounded;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private Vector3 velocity = Vector3.zero;
    private float limitFallSpeed = 25f;

    public bool SkillDoubkeJump;
    public bool canDoubleJump = true;
    public float m_DashForce = 25f;
    private bool canDash = true;
    private bool isDashing = false;
    private bool m_IsWall = false;
    private bool isWallSliding = false;
    private bool oldWallSlidding = false;
    private float prevVelocityX = 0f;
    private bool canCheck = false;

    public float life = 10f;
    public bool invincible = false;
    private bool canMove = true;

    private Animator animator;
    public ParticleSystem particleJumpUp;
    public ParticleSystem particleJumpDown;
    public ParticleSystem particleJumpWall;

    public CurrencyManager currencyManager;
    private float jumpWallStartX = 0;
    private float jumpWallDistX = 0;
    private bool limitVelOnWallJump = false;

    [Header("Events")]
    [Space]
    public Sprite UnactiveCheckpoint;
    public Sprite ActivCheckpoint;

    public UnityEvent OnFallEvent;
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    // === Coyote Time ===
    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private void Awake()
    {
        savegroundsaver = GetComponent<SaveGroundSaver>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (OnFallEvent == null)
            OnFallEvent = new UnityEvent();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        if (!SkillDoubkeJump)
            canDoubleJump = false;

        life = 1;
        bool wasGrounded = m_Grounded;
        m_Grounded = false;
        coyoteTimeCounter -= Time.fixedDeltaTime;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                coyoteTimeCounter = coyoteTime;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                    if (!m_IsWall && !isDashing)
                        particleJumpDown.Play();
                    canDoubleJump = true;
                    if (m_Rigidbody2D.linearVelocity.y < 0f)
                        limitVelOnWallJump = false;
                }
            }
        }

        m_IsWall = false;

        if (!m_Grounded)
        {
            OnFallEvent.Invoke();
            Collider2D[] collidersWall = Physics2D.OverlapCircleAll(m_WallCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < collidersWall.Length; i++)
            {
                if (collidersWall[i].gameObject != null)
                {
                    isDashing = false;
                    m_IsWall = true;
                }
            }
            prevVelocityX = m_Rigidbody2D.linearVelocity.x;
        }

        if (limitVelOnWallJump)
        {
            if (m_Rigidbody2D.linearVelocity.y < -0.5f)
                limitVelOnWallJump = false;

            jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;
            if (jumpWallDistX < -0.5f && jumpWallDistX > -1f)
            {
                canMove = true;
            }
            else if (jumpWallDistX < -1f && jumpWallDistX >= -2f)
            {
                canMove = true;
                m_Rigidbody2D.linearVelocity = new Vector2(10f * transform.localScale.x, m_Rigidbody2D.linearVelocity.y);
            }
            else if (jumpWallDistX < -2f || jumpWallDistX > 0)
            {
                limitVelOnWallJump = false;
                m_Rigidbody2D.linearVelocity = new Vector2(0, m_Rigidbody2D.linearVelocity.y);
            }
        }
    }

    public void Move(float move, bool jump, bool dash)
    {
        if (canMove)
        {
            if (dash && canDash && !isWallSliding)
                StartCoroutine(DashCooldown());

            if (isDashing)
            {
                m_Rigidbody2D.linearVelocity = new Vector2(transform.localScale.x * m_DashForce, 0);
            }
            else if (m_Grounded || m_AirControl)
            {
                if (m_Rigidbody2D.linearVelocity.y < -limitFallSpeed)
                    m_Rigidbody2D.linearVelocity = new Vector2(m_Rigidbody2D.linearVelocity.x, -limitFallSpeed);

                Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.linearVelocity.y);
                m_Rigidbody2D.linearVelocity = Vector3.SmoothDamp(m_Rigidbody2D.linearVelocity, targetVelocity, ref velocity, m_MovementSmoothing);

                if (move > 0 && !m_FacingRight && !isWallSliding)
                    Flip();
                else if (move < 0 && m_FacingRight && !isWallSliding)
                    Flip();
            }

            // === Coyote Time Jump ===
            if ((m_Grounded || coyoteTimeCounter > 0f) && jump)
            {
                coyoteTimeCounter = 0f;
                animator.SetBool("IsJumping", true);
                animator.SetBool("JumpUp", true);
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                canDoubleJump = true;
                particleJumpDown.Play();
                particleJumpUp.Play();
            }
            else if (!m_Grounded && jump && canDoubleJump && !isWallSliding)
            {
                canDoubleJump = false;
                m_Rigidbody2D.linearVelocity = new Vector2(m_Rigidbody2D.linearVelocity.x, 0);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce / 1.2f));
                animator.SetBool("IsDoubleJumping", true);
            }

            // Wall Sliding & Wall Jumping
            // (Hier bleibt dein bestehender Code unverändert)
            // ...
            // Code für wall sliding und jumping wie zuvor
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void ApplyDamage(float damage, Vector3 position)
    {
        if (!invincible)
        {
            animator.SetBool("Hit", true);
            life -= damage;
            Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f;
            m_Rigidbody2D.linearVelocity = Vector2.zero;
            m_Rigidbody2D.AddForce(damageDir * 10);
            if (life <= 0)
            {
                StartCoroutine(WaitToDead());
            }
            else
            {
                StartCoroutine(Stun(0.25f));
                StartCoroutine(MakeInvincible(1f));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            SpriteRenderer newCheckpointSprite = other.GetComponent<SpriteRenderer>();
            if (lastCheckpoint != null)
            {
                SpriteRenderer lastCheckpointSprite = lastCheckpoint.GetComponent<SpriteRenderer>();
                if (lastCheckpointSprite != null)
                    lastCheckpointSprite.sprite = UnactiveCheckpoint;
            }
            newCheckpointSprite.sprite = ActivCheckpoint;
            checkpoint = other.transform;
            lastCheckpoint = other.transform;
            CheckpointActive = true;
        }

        if (other.CompareTag("Orb"))
        {
            currencyManager.AddOrbs(1);
            Destroy(other.gameObject);
        }
    }

    IEnumerator DashCooldown()
    {
        animator.SetBool("IsDashing", true);
        isDashing = true;
        canDash = false;
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
        yield return new WaitForSeconds(0.5f);
        canDash = true;
    }

    IEnumerator Stun(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    IEnumerator MakeInvincible(float time)
    {
        invincible = true;
        yield return new WaitForSeconds(time);
        invincible = false;
    }

    IEnumerator WaitToDead()
    {
        invincible = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        yield return new WaitUntil(() => m_Grounded);
        collider.enabled = false;
        rb.simulated = false;
        lives--;
        animator.SetBool("IsDead", true);
        canMove = false;
        GetComponent<Attack>().enabled = false;
        yield return new WaitForSeconds(0.4f);
        m_Rigidbody2D.linearVelocity = new Vector2(0, m_Rigidbody2D.linearVelocity.y);
        yield return new WaitForSeconds(1.1f);
        savegroundsaver.WarpPlayerToSaveGround();
        animator.SetBool("IsDead", false);
        canMove = true;
        GetComponent<Attack>().enabled = true;
        collider.enabled = true;
        rb.simulated = true;

        if (CheckpointActive && checkpoint != null && lives == 0)
        {
            transform.position = checkpoint.position;
            animator.SetBool("IsDead", false);
            life = 10f;
            canMove = true;
            lives = 5;
            invincible = false;
            GetComponent<Attack>().enabled = true;
        }
        else if (lives == 0)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        yield return new WaitForSeconds(1);
        invincible = false;
    }

    public void TeleportForward()
    {
        if (isTeleporting || !canMove) return;
        StartCoroutine(TeleportCoroutine());
    }

    private IEnumerator TeleportCoroutine()
    {
        isTeleporting = true;
        canMove = false;

        if (audioSource != null && teleportSound != null)
            audioSource.PlayOneShot(teleportSound);

        spriteRenderer.enabled = false;

        if (mainCamera != null)
            mainCamera.GetComponent<CameraFollow>().enabled = false;

        float direction = m_FacingRight ? 1f : -1f;
        Vector3 teleportTarget = transform.position + new Vector3(direction * teleportDistance, 0f, 0f);

        yield return new WaitForSeconds(teleportDuration);
        transform.position = teleportTarget;

        yield return new WaitForSeconds(0.05f);
        if (mainCamera != null)
            mainCamera.GetComponent<CameraFollow>().enabled = true;

        spriteRenderer.enabled = true;
        isTeleporting = false;
        canMove = true;
    }
}
