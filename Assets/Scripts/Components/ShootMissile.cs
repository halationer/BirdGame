using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMissile : MonoBehaviour
{
    public Transform missileSocket;
    public GameObject missileType;

    [HideInInspector]
    public Transform target;

    public void ShootOneMissile()
    {
        GameObject missile = BulletPool.Instance.ActiveObj(missileSocket, missileType);
        missile.GetComponent<MoveToTarget>().target = target;
        missileSocket.gameObject.SetActive(false);
    }

    public void ReMakeSocket()
    {
        missileSocket.gameObject.SetActive(true);
    }
    
}
