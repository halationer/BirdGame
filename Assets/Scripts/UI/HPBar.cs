using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public GameObject obj;
    public float changeSpeed = 0.1f;

    Slider slider;
    float targetValue;

    private void Start()
    {
        slider = GetComponent<Slider>();
        if ( obj == null ) gameObject.SetActive(false);
    }

    private void Update()
    {
        if (obj == null) return;

        ILifeObject lifeObject = obj.GetComponent<ILifeObject>();
        targetValue = lifeObject.GetHP() / (float)lifeObject.GetMaxHP();
        slider.value = Mathf.Lerp(slider.value, targetValue, changeSpeed);
    }
}
