using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour, IFactoryObject
{
    public float existTime = 3.0f;

    [HideInInspector]
    public int typeIndex = 0;

    public int TypeIndex { get => typeIndex; set => typeIndex = value; }

    void Start()
    {
        GameManager.Instance.OnGameEnd += delegate () { StopAllCoroutines(); };
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyByTime());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(existTime);
        DestroySelf();
    }

    public void DestroySelf()
    {
        BulletPool.Instance.DestroyBullet(gameObject);
    }
}
