using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public event Action<Collider2D> itemFunc;
    public float existTime = 5.0f;

    protected Coroutine buffCoroutine = null;

    private void OnEnable()
    {
        if(existTime > 0)
        {
            StartCoroutine(DestroySelf());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "player")
        {
            itemFunc(collision);
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(existTime);
        while (buffCoroutine != null)
            yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
