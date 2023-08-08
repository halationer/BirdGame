using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBoss : Player, IDestroySelf, IScoreObject
{
    public Animator missileAnim;
    public ShootMissile missile;
    public Animator batteryAnim;
    public float comingTime = -1f;
    public List<Transform> bulletSocketList;

    [HideInInspector]
    public Transform bossStayPos;

    [SerializeField]
    private int score = 888;
    public int Score => score;

    Transform target;
    bool batteryOn = false;
    ShootBattery batteryComponent;

    public event Action onDie;

    protected override void Start()
    {
        base.Start();

        batteryComponent = GetComponentInChildren<ShootBattery>();
        batteryComponent.onBatteryReady += delegate () { batteryOn = true; };

        target = FindAnyObjectByType<BirdAttack>().transform;
        missile.target = target;
        LockHp(2.0f);
    }

    private void OnEnable()
    {
        OnGameStart();
    }

    protected override void OnGameStart()
    {
        base.OnGameStart();

        if(comingTime > 0)
        {
            RunBoss();
        }
    }

    public void RunBoss()
    {
        // put boss outof screen and active
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1.5f, 0.5f));
        StartCoroutine(BossPlay());
    }

    void BossStop()
    {
        if (batteryOn)
        {
            batteryOn = false;
            batteryAnim.SetTrigger("Back");
        }
        StopAllCoroutines();
    }

    protected override void OnGameEnd()
    {
        base.OnGameEnd();

        BossStop();
    }

    protected override void OnGameRestart()
    {
        base.OnGameRestart();

        DestroySelf();
    }

    protected override void Update()
    {
        if (GameManager.Instance.state == GameState.End) return;

        if(batteryOn)
        {
            batteryComponent.TurnToTarget(target);
        }
    }

    IEnumerator BossPlay()
    {
        // wait for comming
        yield return new WaitForSeconds(comingTime);

        // move to the original position
        yield return MoveTo(bossStayPos.position);

        while(true)
        {
            yield return new WaitForSeconds(0.5f);

            bulletSocketList[0].parent.Rotate(0, 0, 10, Space.Self);
            yield return ShootByList(2);
            yield return new WaitForSeconds(0.5f);
            bulletSocketList[0].parent.Rotate(0, 0, -5, Space.Self);
            yield return ShootByList(3);
            yield return new WaitForSeconds(0.6f);
            bulletSocketList[0].parent.Rotate(0, 0, -5, Space.Self);
            yield return ShootByList(4);
            yield return new WaitForSeconds(0.7f);
            missileAnim.SetTrigger("Shoot");

            yield return new WaitForSeconds(3.0f);
            batteryAnim.SetTrigger("Expand");
           
            yield return WaitForBatteryReady();

            yield return BatteryShoot(UnityEngine.Random.Range(20, 30));

            batteryOn = false;
            batteryAnim.SetTrigger("Back");
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator MoveTo(Vector3 position)
    {
        while(true)
        {
            Vector3 moveDirection = position - transform.position;
            if(moveDirection.magnitude < 0.1)
            {
                transform.position = position;
                break;
            }
            Vector3 moveDelta = moveDirection.normalized * moveSpeed * Time.deltaTime;
            transform.position += moveDelta;
            yield return null;
        }
    }

    IEnumerator WaitForBatteryReady()
    {
        while (!batteryOn)
            yield return new WaitForSeconds(0.2f);
    }

    IEnumerator BatteryShoot(int times)
    {
        while(times-- > 0)
        {
            if (times % 5 == 0) 
                yield return new WaitForSeconds(0.4f);

            batteryAnim.SetTrigger("Shoot");
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator ShootByList(int times)
    {
        while(times-- > 0) 
        {
            foreach (var socket in bulletSocketList)
            {
                BulletPool.Instance.ActiveObj(socket, bulletType);
            }
            yield return new WaitForSeconds(1 / attackSpeed);
        }
    }

    public void DestroySelf()
    {
        animator.SetTrigger("Die");

        BossStop();
    }


    public void DestroySelf_AnimEvent()
    {
        onDie?.Invoke();
        Destroy(gameObject);
    }
}
