using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFall : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public float timeToFall = 1.5f;
    public float cooldown = 9;
    private bool touched;
    private Vector3 initPosition;
    public BoxCollider2D collider;
   //  Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(touched)
        {
            if(timeToFall > 0)
            {
                timeToFall -= Time.deltaTime;
            }else if(cooldown>0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 1);
                rigidBody.constraints = RigidbodyConstraints2D.None;
                rigidBody.gravityScale = 2;
                cooldown -= Time.deltaTime;
                collider.enabled = false;
                
                
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                touched = false;
                cooldown = 9;
                timeToFall = 1.5f;
                rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
                rigidBody.gravityScale = 0;
                transform.position = initPosition;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                collider.enabled = true;

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
