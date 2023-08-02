using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour
{
    Rigidbody rigid;
    int jumpLock;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        if(rigid == null ) rigid = gameObject.AddComponent<Rigidbody>();
        jumpLock = jumpMax;
    }

    void Update()
    {
        Jump();
    }

    public int jumpMax = 1;
    public float jumpStrength = 5.0f;

    void Jump()
    {
        if (jumpLock <= 0) return;

        if (Input.GetMouseButtonDown(0))
        {
            --jumpLock;
            rigid.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        jumpLock = Mathf.Min(jumpLock + 1, jumpMax);
    }
}
