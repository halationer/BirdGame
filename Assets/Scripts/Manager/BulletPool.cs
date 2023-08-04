using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<GameObject> bulletList;
    public int initialPoolSize = 5;
    public float rangeY = 0.1f;
    public bool useRandom = true;

    Queue<GameObject>[] deactiveBullet;

    public GameObject ActiveBullet(Transform initTransform, GameObject bulletType = null)
    {
        int typeIndex = 0;

        if (bulletType != null)
        {
            typeIndex = bulletList.IndexOf(bulletType);
        }

        if (deactiveBullet[typeIndex].Count == 0)
        {
            MakeNewBullet(typeIndex);
        }
        GameObject bullet = deactiveBullet[typeIndex].Dequeue();
        bullet.transform.SetPositionAndRotation(initTransform.position, initTransform.rotation);
        if (useRandom) RandomY(bullet);
        bullet.SetActive(true);
        return bullet;
    }

    public void DestroyBullet(GameObject bullet)
    {
        int typeIndex = 0;

        if (bullet != null)
        {
            typeIndex = bullet.GetComponent<IFactoryObject>().TypeIndex;
        }

        // ∑¿÷π÷ÿ∏¥»Î∂”
        if(bullet.activeInHierarchy)
        {
            bullet.SetActive(false);
            deactiveBullet[typeIndex].Enqueue(bullet);
        }
    }

    private void Start()
    {
        InitBullets();

        GameManager.Instance.OnGameRestart += OnGameRestart;
    }

    void OnGameRestart()
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            DestroyBullet(transform.GetChild(i).gameObject);
        }
    }

    void InitBullets()
    {
        deactiveBullet = new Queue<GameObject>[bulletList.Count];

        for(int typeIndex = 0; typeIndex < deactiveBullet.Length; ++typeIndex)
        {
            deactiveBullet[typeIndex] = new Queue<GameObject>();

            for (int i = 0; i < initialPoolSize; ++i)
            {
                MakeNewBullet(typeIndex);
            }
        }
        
    }

    void MakeNewBullet(int typeIndex)
    {
        GameObject newBullet = Instantiate(bulletList[typeIndex], transform);
        newBullet.SetActive(false);
        newBullet.GetComponent<IFactoryObject>().TypeIndex = typeIndex;
        deactiveBullet[typeIndex].Enqueue(newBullet);
    }

    void RandomY(GameObject bullet)
    {
        bullet.transform.position += Vector3.up * Random.Range(-rangeY, rangeY);
    }
}
