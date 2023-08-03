using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag == "player_bullet" && collision.tag == "enemy")
        {
            collision.GetComponent<IFactoryObject>()?.DestroySelf();
            ScoreManager.Instance.AddScore();
            BulletPool.Instance.DestroyBullet(gameObject);
        }

        if(gameObject.tag == "enemy_bullet" && collision.tag == "player")
        {
            collision.GetComponent<BirdAttack>()?.CollisionDie();
            BulletPool.Instance.DestroyBullet(gameObject);
        }
    }
}
