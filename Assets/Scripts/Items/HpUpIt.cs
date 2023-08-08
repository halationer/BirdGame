using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpUpIt : Item
{
    public float keepTime = 3.0f;
    public int hpUp = 1;

    public static AttackSpeedIt buffCurrent = null;

    void Start()
    {
        itemFunc += DoItem;
    }

    void DoItem(Collider2D collider)
    {
        BirdAttack bird = collider.GetComponent<BirdAttack>();
        if(bird != null) 
        {
            bird.TakeDamage(-hpUp);
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
}
