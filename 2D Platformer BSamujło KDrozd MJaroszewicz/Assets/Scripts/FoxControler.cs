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
    public AudioClip bsound;
    public AudioClip gsound;
    public AudioClip vsound;
    public AudioClip attack_sound;
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
    public LayerMask platformLayer;
    public GameObject attack;
    public CapsuleCollider2D mainCollider;
    private float cooldown;
    public float AttackCooldown;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            if (cooldown < 0)
                cooldown = 0;
           
        }
        ScoreMenager.instance.setCooldown(cooldown);
        isWalking = false;
        if (ScoreMenager.instance.currentGameState == GameState.GS_GAME)
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                if (!ScoreMenager.instance.CheckIfPlayerAlive())
                {
                    ScoreMenager.instance.SetPlayerAlive(true);
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
                isWalking = true;
                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                if (!isFacingRight)
                {
                    flip();
                }
            }
            else
            {
                mainCollider.enabled = true;
            }
                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                {
                if ((cooldown<=0)&&(ScoreMenager.instance.CheckIfPlayerAlive()))
                {
                    attack.SetActive(true);
                    StartCoroutine(attackoff());
                    source.PlayOneShot(attack_sound, AudioListener.volume*2);
                }
                
                 }
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                if (!ScoreMenager.instance.CheckIfPlayerAlive())
                {
                    ScoreMenager.instance.SetPlayerAlive(true);
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
                isWalking = true;
                    transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                    if (isFacingRight)
                    {
                        flip();
                    }
                }
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    jump();
                if (!ScoreMenager.instance.CheckIfPlayerAlive())
                {
                    ScoreMenager.instance.SetPlayerAlive(true);
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
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
        source.volume = 0.1f;
        source.Play();
    }
    private IEnumerator attackoff()
    {
        yield return new WaitForSeconds(0.209f);
        attack.SetActive(false);
        cooldown = AttackCooldown;
    }
    private bool isGrounded()
    {
        bool is_grounded = Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
        if(!is_grounded)
        {
            is_grounded = Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, platformLayer.value)&&rigidBody.velocity.y==0;
        }
        return is_grounded;


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
            source.PlayOneShot(bsound, AudioListener.volume);
            ScoreMenager.instance.addPoint(1);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("gem"))
        {
            source.PlayOneShot(gsound, AudioListener.volume);
            ScoreMenager.instance.addPoint(5);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Key"))
        {
           // startPosition = other.transform.position;
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
        else if (other.CompareTag("Respawn"))
        {
            if ((startPosition.x != other.gameObject.transform.position.x) && (startPosition.y != other.gameObject.transform.position.y))
            {
                ScoreMenager.instance.ReachRespawn();
                startPosition = other.gameObject.transform.position;

                source.PlayOneShot(key_sound, AudioListener.volume);
            }

        }
        else if (other.CompareTag("Enemy"))
        {
            if (ScoreMenager.instance.CheckIfPlayerAlive())
            {
                if (mainCollider.IsTouching(other))
                {
                    ScoreMenager.instance.subhp();
                    gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                    if (ScoreMenager.instance.life == 0)
                    {
                        source.clip = null;
                        source.PlayOneShot(lost_sound, AudioListener.volume * 2);

                        Death();


                    }
                    else
                    {
                        source.PlayOneShot(hit_sound, AudioListener.volume);
                        transform.position = startPosition;
                    }
                }
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
            
            ScoreMenager.instance.subhp();
            gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
            if (ScoreMenager.instance.life == 0)
            {
                source.clip = null;
                source.PlayOneShot(lost_sound, AudioListener.volume * 2);

                Death();

            }
            else
            {
                source.PlayOneShot(hit_sound, AudioListener.volume);
                transform.position = startPosition;
            }
        }

    }
    private void Death()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        ScoreMenager.instance.death();
    }
}
