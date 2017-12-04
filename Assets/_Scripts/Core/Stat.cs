using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField]
    private BarController bar;

    [SerializeField]
    private float fatMax;

    [SerializeField]
    private float currentFat;

    [SerializeField]
    private float critMax;

    [SerializeField]
    private float currentCrit;

    public float CurrentFat
    {
        get
        {
            return currentFat;
        }

        set
        {
            this.currentFat = Mathf.Clamp(value,0,FatMax);
            bar.FatValue = currentFat;
        }
    }

    public float FatMax
    {
        get
        {
            return fatMax;
        }

        set
        {
            this.fatMax = value;
            bar.fatMax = fatMax;
        }
    }

    public float CurrentCrit
    {
        get
        {
            return currentCrit;
        }

        set
        {
            this.currentCrit = Mathf.Clamp(value, 0, CritMax);
            bar.CritValue = currentCrit;
        }
    }

    public float CritMax
    {
        get
        {
            return critMax;
        }

        set
        {
            this.critMax = value;
            bar.critMax = critMax;
        }
    }

    public void Initialize()
    {
        this.FatMax = fatMax;
        this.CurrentFat = currentFat;

        this.CritMax = critMax;
        this.CurrentCrit = currentCrit;
    }
}
