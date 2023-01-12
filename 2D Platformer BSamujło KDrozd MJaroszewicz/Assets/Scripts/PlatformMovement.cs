using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;
    // Start is called before the first frame update
    private int currentPoint;
    private bool movingForward = true;
    void Start()
    {
        currentPoint = startingPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position,points[currentPoint].position) < 0.02f)
        {
            if(currentPoint==points.Length-1)
            {
                movingForward = false;
            }else if(currentPoint==0)
            {
                movingForward = true;
            }
           currentPoint = movingForward ? currentPoint+1 : currentPoint-1;          
        }
        transform.position = Vector2.MoveTowards(transform.position, points[currentPoint].position, speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(transform.position.y<collision.transform.position.y- 0.245f) //gracz sie nie przylepia do platformy od bokow i dolu
            collision.transform.SetParent(transform);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
