using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private int playerIndex;

    private float hp = 100;
    private bool isDead = false;
    int dir;

    [SerializeField] private LayerMask playerLayer;

    Animator anim;
    SpriteRenderer spr;

    [SerializeField] private KeyCode attackKey;
    [SerializeField] private KeyCode skillKey;
    [SerializeField] private KeyCode ultimateKey;

    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRadius;
    [SerializeField] private int attackDamage;
    [SerializeField] private Transform skillPos;
    [SerializeField] private float skillRadius;
    [SerializeField] private int skillDamage;
    [SerializeField] private Transform ultimatePos;
    [SerializeField] private float ultimateRadius;
    [SerializeField] private int ultimateDamage;

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
        hp = 100;
        isDead = false;
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
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetKey(moveRight))
        {
            moveInput = 1; // 오른쪽으로 이동
            transform.localScale = new Vector3(1, 1, 1);
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

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            hp = 0;
            isDead = true;
        }
        GameManager.Instance.hpUpdate(playerIndex, hp);
        StopCoroutine("HitColor");
        spr.color = Color.white;
        StartCoroutine("HitColor");
    }

    IEnumerator HitColor()
    {
        spr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spr.color = Color.white;
    }
    void Attack()
    {
        Collider2D[] playersInRange = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, playerLayer);

        foreach (Collider2D player in playersInRange)
        {
            if (player.gameObject != gameObject) // 자기 자신을 제외
            {
                player.gameObject.GetComponent<PlayerController>().TakeDamage(attackDamage);
            }
        }
    }

    void Skill()
    {
        Collider2D[] playersInRange = Physics2D.OverlapCircleAll(skillPos.position, skillRadius, playerLayer);

        foreach (Collider2D player in playersInRange)
        {
            if (player.gameObject != gameObject) // 자기 자신을 제외
            {
                player.gameObject.GetComponent<PlayerController>().TakeDamage(skillDamage);
            }
        }
    }

    void Ultimate()
    {
        Collider2D[] playersInRange = Physics2D.OverlapCircleAll(ultimatePos.position, ultimateRadius, playerLayer);

        foreach (Collider2D player in playersInRange)
        {
            if (player.gameObject != gameObject) // 자기 자신을 제외
            {
                player.gameObject.GetComponent<PlayerController>().TakeDamage(ultimateDamage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos.position, attackRadius);
    }
}
