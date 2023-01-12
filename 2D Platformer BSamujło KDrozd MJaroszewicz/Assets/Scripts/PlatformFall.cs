using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFall : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public float timeToFall = 0.5f;
    public float cooldown = 4;

    private bool touched;
    private Vector3 initPosition;
    private BoxCollider2D Bcollider;

    private float actualCD; //zmienne aktualizowane 
    private float actualtimeToFall;
    //  Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
        Bcollider = GetComponent<BoxCollider2D>();
        actualCD = cooldown;
        actualtimeToFall = timeToFall;
    }

    // Update is called once per frame
    void Update()
    {
        if(touched)
        {
            if(actualtimeToFall > 0)
            {
                actualtimeToFall -= Time.deltaTime;
            }else if(actualCD > 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 1);
                rigidBody.constraints = RigidbodyConstraints2D.None;
                rigidBody.gravityScale = 2;
                actualCD -= Time.deltaTime;
                Bcollider.enabled = false;
                
                
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                touched = false;
                actualCD = cooldown;
                actualtimeToFall = timeToFall;
                rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
                rigidBody.gravityScale = 0;
                transform.position = initPosition;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                Bcollider.enabled = true;

            }
            
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        touched = true;
    }
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

}
