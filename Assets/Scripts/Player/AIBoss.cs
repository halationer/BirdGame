using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBoss : Player, IScoreObject
{
    Animator missileAnim;

    [SerializeField]
    private int score = 888;
    public int Score => score;

    protected override void Start()
    {
        base.Start();

        missileAnim = GetComponentInChildren<Animator>();
    }

    protected override void OnGameStart()
    {
        base.OnGameStart();

        missileAnim.SetTrigger("Shoot");
    }

    protected override void Update()
    {
        if (GameManager.Instance.state == GameState.End) return;
    }

}
