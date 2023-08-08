using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedIt : Item
{
    public float keepTime = 3.0f;
    public float speedUp = 2.0f;

    public static AttackSpeedIt buffCurrent = null;

    void Start()
    {
        itemFunc += DoItem;
    }

    void Lock(BirdAttack bird)
    {
        if (buffCurrent == this) return;

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        buffCurrent = this;
        buffCoroutine = StartCoroutine(WaitTimeUp(bird));

        bird.attackSpeed *= speedUp;
    }

    void UnLock(BirdAttack bird)
    {
        if (buffCurrent != this) return;

        bird.attackSpeed /= speedUp;
        buffCurrent = null;
        buffCoroutine = null;
    }

    void DoItem(Collider2D collider)
    {
        BirdAttack bird = collider.GetComponent<BirdAttack>();
        if(bird != null) 
        { 
            buffCurrent?.UnLock(bird);
            Lock(bird);
        }
    }

    IEnumerator WaitTimeUp(BirdAttack bird)
    {
        yield return new WaitForSeconds(keepTime);
        UnLock(bird);
    }
}
