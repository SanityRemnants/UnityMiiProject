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


    private void Start()
    {
        Enemycollider = GetComponent<CapsuleCollider2D>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aIPath = GetComponent<AIPath>();
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        startPosition = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (ScoreMenager.instance.CheckIfPlayerAlive())
        {

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("attack"))
        {
            aIPath.destination = transform.position;
            ScoreMenager.instance.addPoint(3);
            source.PlayOneShot(enemyHit_sound, AudioListener.volume);
            Enemycollider.enabled = false;
            destinationSetter.enabled = false;
            animator.SetBool("isDead", true);
            StartCoroutine(KillOnAnimationEnd());

        }else if(collision.CompareTag("Player"))
        {
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
