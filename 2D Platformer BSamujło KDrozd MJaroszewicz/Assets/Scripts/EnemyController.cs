using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [Range(0.01f, 20)][SerializeField] private float moveSpeed = 0.1f;
    private Animator animator;
    private bool isFacingRight = true;
    private float startPositionX;
    private bool isAlive = true;
    public float moveRange = 1.0f;
    private bool isMovingRight = false;
    private CapsuleCollider2D Enemycollider;
    public GameObject kill;


    private void Start()
    {
        Enemycollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (isAlive)
        {
            if (isMovingRight)
            {
                if (this.transform.position.x <= startPositionX + moveRange)
                {
                    MoveRight();
                }
                else
                {
                    isMovingRight = false;
                    MoveLeft();
                }
            }
            else
            {
                if (this.transform.position.x >= startPositionX - moveRange)
                {
                    MoveLeft();
                }
                else
                {
                    isMovingRight = true;
                    MoveRight();
                }
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        startPositionX = this.transform.position.x;
    }

    private void flip()
    {
        if (isAlive)
        {
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        if (!isFacingRight)
        {
            flip();
        }
    }

    private void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        if (isFacingRight)
        {
            flip();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.CompareTag("Player"))
            {
                if (collision.gameObject.transform.position.y > kill.transform.position.y)
                {
                    isAlive = false;
                    Enemycollider.enabled = false;
                    animator.SetBool("isDead", true);
                    StartCoroutine(KillOnAnimationEnd());
                }
            }
    }

    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
