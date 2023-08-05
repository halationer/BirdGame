using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrigger : MonoBehaviour, IDamageMaker
{
    public int damage = 1;
    public bool debug = false;

    public int GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag == "enemy_bullet" && collision.tag == "player_bullet")
        {
            BulletPool.Instance.DestroyObj(collision.gameObject);
            BulletPool.Instance.DestroyObj(gameObject);
        }

        ILifeObject obj = collision.GetComponent<ILifeObject>();

        if (obj == null) return;
        

        if (gameObject.tag == "player_bullet" && collision.tag == "enemy")
        {
            obj.TakeDamage(damage);

            if (obj.isDie())
            {
                collision.GetComponent<IFactoryObject>()?.DestroySelf();
                int score = collision.GetComponent<IScoreObject>().Score;
                ScoreManager.Instance.AddScore(score);
            }

            BulletPool.Instance.DestroyObj(gameObject);
        }

        if (gameObject.tag == "enemy_bullet" && collision.tag == "player")
        {
            obj.TakeDamage(damage);

            if(debug) gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.blue;

            if (obj.isDie())
            {
                collision.GetComponent<BirdAttack>()?.Die();
            }

            BulletPool.Instance.DestroyObj(gameObject);
        }
    }
}
