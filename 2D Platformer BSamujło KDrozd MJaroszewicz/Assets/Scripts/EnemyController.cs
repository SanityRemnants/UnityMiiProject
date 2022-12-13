using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [Range(0.01f, 20)][SerializeField] private float moveSpeed = 0.1f;
    private Animator animator;
    private bool isFacingRight = false;
    private float startPositionX;
    public float moveRange = 1.0f;
    private bool isMovingRight = false;


    private void Start()
    {

    }

    private void Update()
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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        startPositionX = this.transform.position.x;
    }

    private void flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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
        if(collision.CompareTag("Player"))
        {
            if (collision.gameObject.transform.position.y > transform.position.y)
            {
                animator.SetBool("isDead",true);
                StartCoroutine(KillOnAnimationEnd());
            }
        }
    }

    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
