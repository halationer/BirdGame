using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StartUI : MonoBehaviour, IPointerClickHandler
{
    private void Start()
    {
        GameManager.Instance.OnGameRestart += delegate () { gameObject.SetActive(true); };
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.StartGame();
        gameObject.SetActive(false);
    }
}
