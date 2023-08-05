using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    public Transform target;
    public float startSpeed = 10.0f;
    public float moveSpeed = 3.0f;

    Rigidbody2D rigid;
    float currentSpeed;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        currentSpeed = startSpeed;
    }

    private void Update()
    {
        MoveTarget();
    }

    void MoveTarget()
    {
        if (target == null) return;

        currentSpeed -= Time.deltaTime * 3.0f;
        if(currentSpeed < moveSpeed) currentSpeed = moveSpeed;

        Vector3 moveDelta = target.position - transform.position;
        moveDelta.Normalize();
        moveDelta *= currentSpeed * Time.deltaTime;
        transform.position += moveDelta;
        transform.rotation = Quaternion.FromToRotation(Vector3.down, moveDelta);
    }
}
