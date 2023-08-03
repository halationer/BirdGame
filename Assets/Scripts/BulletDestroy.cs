using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    public float existTime = 3.0f;

    void Start()
    {
        GameManager.Instance.OnGameEnd += delegate () { StopAllCoroutines(); };
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyByTime());
    }

    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(existTime);
        if (BulletPool.Instance != null)
            BulletPool.Instance.DestroyBullet(gameObject);
        else Destroy(gameObject, existTime);
    }
}
