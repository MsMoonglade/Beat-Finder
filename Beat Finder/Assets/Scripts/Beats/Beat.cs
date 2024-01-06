using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Song
{
    public string songName;
    public float songDuration;
    public List<Beat> beats = new List<Beat>();

    public float currentMaxSpectrum = 0;
    public float currentMinSpectrum = 0;


    public Song() 
    {
        songName = string.Empty;
        songDuration = 0;
        currentMaxSpectrum = 0;
        currentMinSpectrum = 10000;
    }

    public Song(string i_name , float i_duration , List<Beat> i_beats)
    {
        songName = i_name;
        songDuration = i_duration;
        beats = i_beats;
    }

    public void MinMaxSpectrum()
    {
        foreach (var beat in beats)
        {
            if(beat.spectrumValue > currentMaxSpectrum)
            {
                currentMaxSpectrum = beat.spectrumValue;
            }

            if (beat.spectrumValue < currentMinSpectrum)
            {
                currentMinSpectrum = beat.spectrumValue;
            }
        }
    }

    public float GetSpectrumValuePercentage(int i_index)
    {
        return beats[i_index].spectrumValue/currentMaxSpectrum;
    }
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