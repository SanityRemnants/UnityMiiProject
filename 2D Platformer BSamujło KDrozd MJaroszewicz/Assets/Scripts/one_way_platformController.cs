using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class one_way_platformController : MonoBehaviour
{
    FoxControler foxControler;
    PlatformEffector2D PlatformEffector2D;
    // Start is called before the first frame update
    void Start()
    {
        PlatformEffector2D = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            foxControler = collision.gameObject.GetComponent<FoxControler>();
            if(foxControler.fallthrough)
            {
                foxControler = null;
                PlatformEffector2D.rotationalOffset = 180;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(foxControler==null)
        {
            return;
        }
        if(foxControler.fallthrough)
        {
            PlatformEffector2D.rotationalOffset = 180;
            foxControler = null;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        PlatformEffector2D.rotationalOffset = 0;
        foxControler = null;
    }
}
