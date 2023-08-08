using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnim : MonoBehaviour
{
    public void Die()
    {
        transform.parent.GetComponent<IDestroySelf>()?.DestroySelf_AnimEvent();
    }
}
