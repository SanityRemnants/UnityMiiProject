using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGraphic : MonoBehaviour
{
    private bool isFacingRight = true;
    public AIPath aIPath;

    // Update is called once per frame
    void Update()
    {
            if (isFacingRight && aIPath.desiredVelocity.x <= -0.01f)
            {
                flip();
            }
            else if (!isFacingRight && aIPath.desiredVelocity.x >= 0.01f)
            {
                flip();
            }
    }
    private void flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
