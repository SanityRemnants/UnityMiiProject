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
    private bool hasKey = false;
    private bool victory = false;
    
    public const float rayLength = 0.4f;
    public float jumpforce = 5.0f;
    public LayerMask groundLayer;


    private void Start()
    {
        
    }

    private void Update()
    {
        isWalking = false;
        if (victory == false)
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
            animator.SetBool("isGrounded", isGrounded());
            //Debug.DrawRay(transform.position, rayLength*Vector3.down, Color.white, 1, false);
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("esc");
            Application.Quit();
        }
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private bool isGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    private void jump()
    {
        if (isGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);

            Debug.Log("Jumping");
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
            hasKey = true;
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Exit"))
        {
            if(hasKey)
            {
                victory = true;
                ScoreMenager.instance.victory();
                Time.timeScale = 0;
            }
            else
            {
                ScoreMenager.instance.youNeedKey();
            }
        }

    }
}
