using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private RaycastHit2D groundHit;
    [SerializeField] private float extraHeigth = 0.25f;
    private Collider2D coll;
    [SerializeField] private LayerMask whatIsGround;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    public bool IsGrounded()
    {
        groundHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, extraHeigth, whatIsGround);

        if(groundHit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
