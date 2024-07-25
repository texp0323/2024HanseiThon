using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private int hp = 100;
    private bool isDead = false;

    Animator anim;
    SpriteRenderer spr;

    [SerializeField] private KeyCode attackKey;
    [SerializeField] private KeyCode skillKey;
    [SerializeField] private KeyCode ultimateKey;

    //이동 관련 변수들
    #region move
    [SerializeField] private float moveSpeed = 10f; // 캐릭터 이동 속도
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
    #endregion

    void Start()
    {
        TryGetComponent<Rigidbody2D>(out rb);
        TryGetComponent<Animator>(out anim);
        TryGetComponent<SpriteRenderer>(out spr);
    }

    void Update()
    {
        PlayerJump();
        if (Input.GetKeyDown(attackKey))
            anim.SetTrigger("Attack");
        if (Input.GetKeyDown(skillKey))
            anim.SetTrigger("Skill");
        if (Input.GetKeyDown(ultimateKey))
            anim.SetTrigger("Ultimate");
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
        int moveInput = 0;
        if (Input.GetKey(moveLeft))
        {
            moveInput = -1; // 왼쪽으로 이동
            spr.flipX = false;
        }
        else if (Input.GetKey(moveRight))
        {
            moveInput = 1; // 오른쪽으로 이동
            spr.flipX = true;
        }
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        anim.SetBool("Run", moveInput != 0);
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

    public void TakeHit(int damage)
    {
        hp -= damage;
        if(damage <= 0)
        {
            hp = 0;
            isDead = true;
        }
        else
        {

        }
    }

    void Attack()
    {
        
    }

    void Skill()
    {
        Debug.Log("skill");
    }

    void Ultimate()
    {
        
    }

}
