using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxControler : MonoBehaviour
{
    [Range(0.01f, 20)][SerializeField] private float moveSpeed = 0.1f;
    private Rigidbody2D rigidBody;

    public const float rayLength = 0.4f;
    public float jumpforce = 5.0f;
    public LayerMask groundLayer;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        } else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        } else if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }

        //Debug.DrawRay(transform.position, rayLength*Vector3.down, Color.white, 1, false);
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    bool isGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    void jump()
    {
        if (isGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);

            Debug.Log("Jumping");
        }
    }
}
