using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Beats
{
    public List<Beat> beats = new List<Beat>();
}


[System.Serializable]
public class Beat
{
    public double time;
    public float spectrumValue;

    public Beat(double i_time , float i_spectrumValue)
    {
        time = i_time;
        spectrumValue = i_spectrumValue;
    }
}