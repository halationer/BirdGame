using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDebugger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogFormat("Trigger: this - {0} | other - {1} ", gameObject.name, collision.name);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogFormat("Collision: this - {0} | other - {1} ", gameObject.name, collision.gameObject.name);
    }
}
