using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour {
    
    private float fatPoints;
    private float critPoints;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Image fatnessContent;

    [SerializeField]
    private Image criticContent;

    public float fatMax;
    public float critMax;

    public void setFatValue(float value)
    {
        fatPoints = Map(value, 0, fatMax, 0, 1);
    }

    public void setCritValue(float value)
    {
        critPoints = Map(value, 0, critMax, 0, 1);

    }

    void Update ()
    {
        HandleBar();
	}

    private void HandleBar()
    {
        if (fatPoints != fatnessContent.fillAmount)
        {
            fatnessContent.fillAmount = Mathf.Lerp(fatnessContent.fillAmount, fatPoints, Time.deltaTime * lerpSpeed);
        }

        if (critPoints != criticContent.fillAmount)
        {
            criticContent.fillAmount = Mathf.Lerp(criticContent.fillAmount, critPoints, Time.deltaTime * lerpSpeed);
        }

    }

    private float Map(float val, float inMin, float inMax, float outMin, float outMax)
    {
        return (val - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
