using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Song
{
    public string songName;
    public float songDuration;
    public List<Beat> beats = new List<Beat>();

    public Song() 
    {
        songName = string.Empty;
        songDuration = 0;
    }   

    public Song(string i_name , float i_duration , List<Beat> i_beats)
    {
        songName = i_name;
        songDuration = i_duration;
        beats = i_beats;
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