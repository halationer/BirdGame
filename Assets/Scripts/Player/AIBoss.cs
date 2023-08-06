using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBoss : Player, IDestroySelf, IScoreObject
{
    public Animator missileAnim;
    public Animator batteryAnim;
    public Transform target;
    public float comingTime = -1f;
    public List<Transform> bulletSocketList;

    [SerializeField]
    private int score = 888;
    public int Score => score;

    bool batteryOn = false;
    ShootBattery batteryComponent;

    protected override void Start()
    {
        base.Start();

        gameObject.SetActive(false);
        batteryComponent = GetComponentInChildren<ShootBattery>();
        batteryComponent.onBatteryReady += delegate () { batteryOn = true; };
    }

    protected override void OnGameStart()
    {
        base.OnGameStart();

        if(comingTime > 0)
        {
            GenerateBoss();
        }
    }

    public void GenerateBoss()
    {
        // put boss outof screen and active
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1.5f, 0.5f));
        gameObject.SetActive(true);

        StartCoroutine(BossPlay());
    }

    protected override void OnGameEnd()
    {
        base.OnGameEnd();

        if (batteryOn)
        {
            batteryOn = false;
            batteryAnim.SetTrigger("Back");
        }
        StopAllCoroutines();
    }

    protected override void OnGameRestart()
    {
        base.OnGameRestart();

        gameObject.SetActive(false);
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
        yield return MoveTo(originalPosition);

        while(true)
        {
            yield return new WaitForSeconds(0.5f);

            yield return ShootByList(2);
            yield return new WaitForSeconds(0.5f);
            yield return ShootByList(3);
            yield return new WaitForSeconds(0.6f);
            yield return ShootByList(4);
            yield return new WaitForSeconds(0.7f);
            missileAnim.SetTrigger("Shoot");

            yield return new WaitForSeconds(3.0f);
            batteryAnim.SetTrigger("Expand");
           
            yield return WaitForBatteryReady();

            yield return BatteryShoot(Random.Range(20, 30));

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
        gameObject.SetActive(false);
        GameManager.Instance.EndGame();
    }
}
