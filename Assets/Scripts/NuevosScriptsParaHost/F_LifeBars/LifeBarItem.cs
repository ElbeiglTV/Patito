using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarItem : MonoBehaviour
{
    private Transform _owner;

    private const float Y_OFFSET = 1.5f;

    [SerializeField] private Image _myImage;

    public LifeBarItem Initialize(Transform owner)
    {
        _owner = owner;

        return this;
    }

    public void UpdateFillAmount(float newValue)
    {
        //_myImage.fillAmount = newValue;

        StopAllCoroutines();

        StartCoroutine(ReduceFillAmountInTime(newValue));
    }

    IEnumerator ReduceFillAmountInTime(float newValue)
    {
        var startValue = _myImage.fillAmount;

        var ticks = 0f;

        while (ticks < 1)
        {
            ticks += Time.deltaTime * 2;

            _myImage.fillAmount = Mathf.Lerp(startValue, newValue, ticks);

            yield return null;
        }
        
    }

    public void UpdatePosition()
    {
        transform.position = _owner.position + Vector3.up * Y_OFFSET;
    }
}
