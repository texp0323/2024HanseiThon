using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private int playerIndex;


    private float hp = 100;
    private float mp = 0;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private SpriteRenderer arrowSpr;

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
    #endregion

    void Start()
    {
        TryGetComponent<Rigidbody2D>(out rb);
        TryGetComponent<Animator>(out anim);
        TryGetComponent<SpriteRenderer>(out spr);
        hp = 100;

        if (playerIndex == 1)
            transform.localScale = new Vector3(1, 1, 1);
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            arrowSpr.flipX = true;
        }
    }

    void Update()
    {
        if (!GameManager.Instance.getIsGaming()) return;

        PlayerJump();
        if (Input.GetKeyDown(attackKey))
            anim.SetTrigger("Attack");
        if (Input.GetKeyDown(skillKey) && mp >= 10)
        {
            anim.SetTrigger("Skill");
            gainMp(-10);
        }
            if (Input.GetKeyDown(ultimateKey) && mp >= 50)
        {
            anim.SetTrigger("Ultimate");
            gainMp(-50);
        }
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
        GroundCheck();
        if (!GameManager.Instance.getIsGaming()) return;

        PlayerMove();
    }

    void PlayerMove()
    {
        int moveInput = 0;
        if (Input.GetKey(moveLeft))
        {
            moveInput = -1; // 왼쪽으로 이동
            transform.localScale = new Vector3(-1, 1, 1);
            arrowSpr.flipX = true;
        }
        else if (Input.GetKey(moveRight))
        {
            moveInput = 1; // 오른쪽으로 이동
            transform.localScale = new Vector3(1, 1, 1);
            arrowSpr.flipX = false;
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
        rb.velocity = Vector2.zero;
        gainMp(5);
        if(hp <= 0)
        {
            hp = 0;
            if (playerIndex == 1)
                GameManager.Instance.Win("Player2");
            else
                GameManager.Instance.Win("Player1");

            rb.velocity = Vector2.up * 10;
            anim.SetTrigger("Death");
        }
        GameManager.Instance.hpUpdate(playerIndex, hp);
        StopCoroutine("HitColor");
        StartCoroutine("HitColor");
    }

    void gainMp(int value)
    {
        mp += value;
        if (mp > 100)
            mp = 100;
        GameManager.Instance.mpUpdate(playerIndex, mp);
    }

    IEnumerator HitColor()
    {
        spr.color = Color.red;
        yield return new WaitForSeconds(0.05f);
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
                gainMp(2);
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
                gainMp(2);
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
                gainMp(2);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos.position, attackRadius);
    }
}
