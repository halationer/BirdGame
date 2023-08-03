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

    public GameObject bulletType;
    public int initialPoolSize = 5;
    public float rangeY = 0.1f;
    public bool useRandom = true;

    List<GameObject> allBulletList = new List<GameObject>();
    Queue<GameObject> deactiveBullet = new Queue<GameObject>();

    public GameObject ActiveBullet(Transform initTransform)
    {
        if (deactiveBullet.Count == 0)
        {
            MakeNewBullet();
        }
        GameObject bullet = deactiveBullet.Dequeue();
        bullet.transform.SetPositionAndRotation(initTransform.position, initTransform.rotation);
        if (useRandom) RandomY(bullet);
        bullet.SetActive(true);
        return bullet;
    }

    public void DestroyBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        deactiveBullet.Enqueue(bullet);
    }

    private void Start()
    {
        InitBullets();

        GameManager.Instance.OnGameRestart += OnGameRestart;
    }

    void OnGameRestart()
    {
        foreach (GameObject bullet in allBulletList)
        {
            if (bullet.activeSelf)
            {
                DestroyBullet(bullet);
            }
        }
    }

    void InitBullets()
    {
        for (int i = 0; i < initialPoolSize; ++i)
        {
            MakeNewBullet();
        }
    }

    void MakeNewBullet()
    {
        GameObject newBullet = Instantiate(bulletType, transform);
        newBullet.SetActive(false);
        deactiveBullet.Enqueue(newBullet);
        allBulletList.Add(newBullet);
    }

    void RandomY(GameObject bullet)
    {
        bullet.transform.position += Vector3.up * Random.Range(-rangeY, rangeY);
    }
}
