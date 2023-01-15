using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAiEnabler : MonoBehaviour
{
    public AIDestinationSetter destinationSetter;
    private BoxCollider2D trigger;
    private void Awake()
    {
        trigger = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && trigger.IsTouching(collision))
        {
            Debug.Log(destinationSetter.enabled);
            destinationSetter.enabled = true;
        }
    }
}
