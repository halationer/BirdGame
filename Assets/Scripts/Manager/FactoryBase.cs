using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBase : MonoBehaviour
{
    public List<GameObject> objList;
    public int initialPoolSize = 5;
    public float rangeY = 0.1f;
    public bool debugRange = false;
    public bool useRandom = true;

    protected List<Queue<GameObject>> deactiveObj;

    public virtual GameObject ActiveObj(Transform initTransform, GameObject objType = null)
    {
        int typeIndex = 0;

        if (objType != null)
        {
            typeIndex = objList.IndexOf(objType);
        }

        if (deactiveObj[typeIndex].Count == 0)
        {
            MakeNewObj(typeIndex);
        }
        GameObject obj = deactiveObj[typeIndex].Dequeue();
        obj.transform.SetPositionAndRotation(initTransform.position, initTransform.rotation);
        if (useRandom) RandomY(obj);
        obj.SetActive(true);
        return obj;
    }

    public virtual void DestroyObj(GameObject obj)
    {
        int typeIndex = 0;

        if (obj != null)
        {
            typeIndex = obj.GetComponent<IFactoryObject>().TypeIndex;
        }

        // ∑¿÷π÷ÿ∏¥»Î∂”
        if (obj.activeInHierarchy)
        {
            obj.SetActive(false);
            deactiveObj[typeIndex].Enqueue(obj);
        }
    }

    protected virtual void Start()
    {
        InitObjs();

        GameManager.Instance.OnGameRestart += OnGameRestart;
    }

    protected virtual void OnGameRestart()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            DestroyObj(transform.GetChild(i).gameObject);
        }
    }

    protected virtual void InitObjs()
    {
        deactiveObj = new();

        for (int typeIndex = 0; typeIndex < objList.Count; ++typeIndex)
        {
            deactiveObj.Add(new Queue<GameObject>());

            for (int i = 0; i < initialPoolSize; ++i)
            {
                MakeNewObj(typeIndex);
            }
        }
    }

    protected virtual GameObject MakeNewObj(int typeIndex)
    {
        GameObject newObj = Instantiate(objList[typeIndex], transform);
        newObj.SetActive(false);
        newObj.GetComponent<IFactoryObject>().TypeIndex = typeIndex;
        deactiveObj[typeIndex].Enqueue(newObj);
        return newObj;
    }

    protected virtual void RandomY(GameObject obj)
    {
        Vector3 randomPos = Vector3.up * Random.Range(-rangeY, rangeY);
        if (debugRange) randomPos = Vector3.up * Random.Range(-1, 2) * rangeY;
        obj.transform.position += randomPos;
    }
}
