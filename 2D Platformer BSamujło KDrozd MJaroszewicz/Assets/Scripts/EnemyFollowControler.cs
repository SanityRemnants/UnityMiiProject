using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyFollowControler : MonoBehaviour
{
    public Animator animator;
    private Vector3 startPosition;
    private AIDestinationSetter destinationSetter;
    private AIPath aIPath;
    private CapsuleCollider2D Enemycollider;
    private AudioSource source;
    public AudioClip enemyHit_sound;
    private float speed;


    private void Start()
    {
        Enemycollider = GetComponent<CapsuleCollider2D>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aIPath = GetComponent<AIPath>();
        speed = aIPath.maxSpeed;
    }

    private void Awake()
    {
        
        source = GetComponent<AudioSource>();
        startPosition = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (!ScoreMenager.instance.CheckIfPlayerAlive())
        {
            destinationSetter.enabled = false;
            aIPath.destination = startPosition;
        }
        else
        {
            aIPath.maxSpeed = speed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("attack"))
        {
            aIPath.enabled = false;
            ScoreMenager.instance.addPoint(6);
            source.PlayOneShot(enemyHit_sound, AudioListener.volume);
            Enemycollider.enabled = false;
            destinationSetter.enabled = false;
            animator.SetBool("isDead", true);
            StartCoroutine(KillOnAnimationEnd());

        }else if(collision.CompareTag("Player"))
        {
            aIPath.maxSpeed = 20;
            destinationSetter.enabled = false;
            aIPath.destination = startPosition;
        }
    }

    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.75f);
        gameObject.SetActive(false);
    }
}
