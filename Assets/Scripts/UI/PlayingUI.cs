using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingUI : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);

        GameManager.Instance.OnGameStart += delegate () { gameObject.SetActive(true); };
        GameManager.Instance.OnGameEnd += delegate () { gameObject.SetActive(false); };
    }
}
