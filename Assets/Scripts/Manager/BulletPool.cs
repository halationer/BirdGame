using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : FactoryBase
{
    public static BulletPool Instance;

    private void Awake()
    {
        Instance = this;
    }
}
