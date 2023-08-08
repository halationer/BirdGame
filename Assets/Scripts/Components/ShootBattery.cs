using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBattery : MonoBehaviour
{
    public Action onBatteryReady;
    public GameObject bulletType;
    public Transform bulletPipe;
    public Transform bulletSocket;
    public float rotateSpeed = 5.0f;

    public void BatteryReady()
    {
        if(onBatteryReady != null) onBatteryReady();
        Debug.Log("battery ready!");
    }

    public void Shoot()
    {
        GameObject bullet = BulletPool.Instance.ActiveObj(bulletSocket, bulletType);
    }

    public void TurnToTarget(Transform target)
    {
        Vector3 targetDir = target.position - bulletPipe.position;
        float angle = Vector3.Angle(bulletPipe.right, targetDir);
        if (angle < 0.1f) return;

        angle = Mathf.Min(angle, rotateSpeed * Time.deltaTime);
        angle *= Mathf.Sign(Vector3.Cross(bulletPipe.right, targetDir).z);
        bulletPipe.Rotate(bulletPipe.forward, angle);
    }
}
