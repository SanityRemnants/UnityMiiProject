using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoxControler : MonoBehaviour
{
    [Range(0.01f, 20)][SerializeField] private float moveSpeed = 0.1f;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool isWalking;
    private bool isFacingRight=true;
    private int score = 0;

    private Vector2 startPosition;
    public const float rayLength = 0.4f;
    public float jumpforce = 5.0f;
    public LayerMask groundLayer;


    private void Start()
    {
        
    }

    private void Update()
    {
        isWalking = false;
        if (ScoreMenager.instance.currentGameState == GameState.GS_GAME)
        {       
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    isWalking = true;
                    transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                    if (!isFacingRight)
                    {
                        flip();
                    }
                }
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                    isWalking = true;
                    transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                    if (isFacingRight)
                    {
                        flip();
                    }
                }
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    jump();
                }
                animator.SetBool("isWalking", isWalking);
                if (!isGrounded())
                {
                    animator.SetBool("isGrounded", false);
                    animator.SetBool("isFalling", isFalling());
                }
                else
                {
                    animator.SetBool("isGrounded", true);
                }
                //Debug.DrawRay(transform.position, rayLength*Vector3.down, Color.white, 1, false);
        }
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
    }

    private bool isGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }
    private bool isFalling()
    {
        return rigidBody.velocity.y < -0.2f;
    }

    private void jump()
    {
        if (isGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        }
    }

    private void flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("bonus"))
        {
            score++;
            ScoreMenager.instance.addPoint(1);
            Debug.Log("Score: " + score);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("gem"))
        {
            score+=5;
            ScoreMenager.instance.addPoint(5);
            Debug.Log("Score: " + score);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Key"))
        {
            ScoreMenager.instance.addkey();
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Exit"))
        {
            ScoreMenager.instance.ReachExit();
  
        } else if (other.CompareTag("Enemy"))
        {
            if (transform.position.y > other.gameObject.transform.position.y)
            {
                score += 3;
                ScoreMenager.instance.addPoint(3);
                Debug.Log("Killed an enemy");
                Debug.Log("Score: " + score);
            } else
            {
                transform.position = startPosition;
                ScoreMenager.instance.subhp();
                if(ScoreMenager.instance.life == 0)
                {
                    Death();
                }
            }
        } else if (other.CompareTag("Live"))
        {
            ScoreMenager.instance.addhp();
            other.gameObject.SetActive(false);
        } else if (other.CompareTag("Death"))
                Death();

    }
    private void Death()
    {
        ScoreMenager.instance.death();
    }
}
