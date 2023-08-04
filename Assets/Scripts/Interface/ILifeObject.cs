using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILifeObject
{
    int GetHP();

    int GetMaxHP();

    void TakeDamage(int damage);

    bool isDie();
}