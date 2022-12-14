using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class FoxControler : MonoBehaviour
{
    [Range(0.01f, 20)][SerializeField] private float moveSpeed = 0.1f;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool isWalking;
    private bool isFacingRight=true;
    private int score = 0;
    public AudioClip bsound;
    public AudioClip gsound;
    public AudioClip vsound;
    public AudioClip jump_sound;
    public AudioClip needKey_sound;
    public AudioClip key_sound;
    public AudioClip hit_sound;
    public AudioClip enemyHit_sound;
    public AudioClip hsound;
    public AudioClip lost_sound;
    public AudioClip maintheme;
    private AudioSource source;
    public bool fallthrough = false;
    private Vector2 startPosition;
    public float rayLength = 1.0f;
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
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    jump();
                }
                if ( Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    fallthrough = true;
                }else
                {
                    fallthrough = false;
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
        source = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        source.clip = maintheme;
        source.volume = 0.03f;
        source.Play();
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
            source.PlayOneShot(jump_sound, AudioListener.volume);
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
        if (other.CompareTag("bonus"))
        {
            score++;
            source.PlayOneShot(bsound, AudioListener.volume);
            ScoreMenager.instance.addPoint(1);
            Debug.Log("Score: " + score);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("gem"))
        {
            score += 5;
            source.PlayOneShot(gsound, AudioListener.volume);
            ScoreMenager.instance.addPoint(5);
            Debug.Log("Score: " + score);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Key"))
        {
            ScoreMenager.instance.addkey();
            other.gameObject.SetActive(false);
            source.PlayOneShot(key_sound, AudioListener.volume);
        }
        else if (other.CompareTag("Exit"))
        {
            bool s = ScoreMenager.instance.ReachExit();
            if (s)
            {
                source.clip = null;
                source.PlayOneShot(vsound, AudioListener.volume);
            }
            else
                source.PlayOneShot(needKey_sound, AudioListener.volume);

        }
        else if (other.CompareTag("Enemy"))
        {
            if (transform.position.y > other.gameObject.transform.position.y)
            {
                score += 3;
                ScoreMenager.instance.addPoint(3);
                Debug.Log("Killed an enemy");
                Debug.Log("Score: " + score);
                source.PlayOneShot(enemyHit_sound, AudioListener.volume);
            }
            else
            {
                transform.position = startPosition;
                ScoreMenager.instance.subhp();
                if (ScoreMenager.instance.life == 0)
                {
                    source.clip = null;
                    source.PlayOneShot(lost_sound, AudioListener.volume*2);
                    
                    Death();

                }
                else
                    source.PlayOneShot(hit_sound, AudioListener.volume);
            }
        }
        else if (other.CompareTag("Live"))
        {
            ScoreMenager.instance.addhp();
            other.gameObject.SetActive(false);
            source.PlayOneShot(hsound, AudioListener.volume);
        }
        else if (other.CompareTag("Death"))
        {
            source.clip = null;
           source.PlayOneShot(lost_sound, AudioListener.volume*2);
            Death();
        }

    }
    private void Death()
    {
        ScoreMenager.instance.death();
    }
}
