using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBySpeed : MonoBehaviour
{
    public float rotateFix = 20.0f;

    Rigidbody2D rigid;
    bool rotateLock = true;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        if(rigid == null) rigid = gameObject.AddComponent<Rigidbody2D>();

        GameManager.Instance.OnGameStart += delegate () { rotateLock = false; };
        GameManager.Instance.OnGameSimulateEnd += delegate () { rotateLock = true; };
        GameManager.Instance.OnGameRestart += delegate () { rotateLock = true; transform.eulerAngles = Vector3.zero; };
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (rotateLock) return;

        if (rigid != null)
        {
            float speed = rigid.velocity.y;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(speed * rotateFix, -90f, 20f));
        }
    }
}
