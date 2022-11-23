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
    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
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
                rigidBody.constraints = RigidbodyConstraints2D.None;
                rigidBody.gravityScale = 1;
                cooldown -= Time.deltaTime;
            }else
            {
                touched = false;
                cooldown = 9;
                timeToFall = 1.5f;
                rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
                rigidBody.gravityScale = 0;
                transform.position = initPosition;
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
