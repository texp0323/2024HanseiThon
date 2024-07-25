using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float moveSpeed = 10f; // 캐릭터 이동 속도
    [SerializeField] private float jumpForce = 20f; // 점프 힘

    private Rigidbody2D rb;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private KeyCode moveLeft;
    [SerializeField] private KeyCode moveRight;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode downKey;

    [SerializeField] private KeyCode attackKey;
    [SerializeField] private KeyCode skillKey;
    [SerializeField] private KeyCode UltimateKey;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PlayerJump();
    }

    void PlayerJump()
    {
        if (isGrounded && Input.GetKey(jumpKey))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();

        GroundCheck();
    }

    void PlayerMove()
    {
        float moveInput = 0;
        if (Input.GetKey(moveLeft))
        {
            moveInput = -1f; // 왼쪽으로 이동
        }
        else if (Input.GetKey(moveRight))
        {
            moveInput = 1f; // 오른쪽으로 이동
        }
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }
    
    void GroundCheck()
    {
        if (rb.velocity.y < 0)
        {
            if (Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}
