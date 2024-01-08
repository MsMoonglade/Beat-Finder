using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongAnalyzer : MonoBehaviour
{
    public int partPerBpm;

    private int currentSongBPM;

    private AudioSource currentAudiosource;
    private Coroutine songAnalyzerCoroutine;

    private void Awake()
    {
        songAnalyzerCoroutine = null;  
    }

    public void StartAnalyze(AudioSource i_source)
    {
        currentAudiosource = i_source;
        currentSongBPM = UniBpmAnalyzer.AnalyzeBpm(currentAudiosource.clip);

        if (currentSongBPM < 0)
        {
            Debug.LogError("AudioClip is null.");
            return;
        }

        Debug.LogWarning("Current Song BPM is : " + currentSongBPM);

        if (songAnalyzerCoroutine == null)
            songAnalyzerCoroutine = StartCoroutine(SongAnalyzerCoroutine());
    }

    public void StartPlaySource(AudioSource i_source)
    {

    }

    private IEnumerator SongAnalyzerCoroutine()
    {
        currentAudiosource.Play();

        float beatInterval = 60f / currentSongBPM;
        float time = 0;
        float songtime = 0;

        List<Beat> thisSongBeat = new List<Beat>();

        while (currentAudiosource.isPlaying)
        {
            float[] spectrum = new float[256];

            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

            time += Time.deltaTime;

            if (time >= (beatInterval / partPerBpm))
            {
                float averageBeatValue = ReturnAverageSpectrum(spectrum);

                Beat beat = new Beat(currentAudiosource.time, averageBeatValue);
                thisSongBeat.Add(beat);
                time = 0;
            }

            yield return null;
        }

        Song o_analyzedSong = new Song(currentAudiosource.clip.name, currentAudiosource.clip.length, thisSongBeat);


        //Detect min and max Spectrum Value in the Song
        float currentMinSpectrum = 100;
        float currentMaxSpectrum = 0;

        foreach (Beat b in o_analyzedSong.beats)
        {
            if (b.spectrumValue != 0 && b.spectrumValue < currentMinSpectrum)
                currentMinSpectrum = b.spectrumValue;

            if (b.spectrumValue > currentMaxSpectrum)
                currentMaxSpectrum = b.spectrumValue;
        }

        o_analyzedSong.currentMinSpectrum = currentMinSpectrum;
        o_analyzedSong.currentMaxSpectrum = currentMaxSpectrum;

        SaveDataToJson(o_analyzedSong);
    }

    private float ReturnAverageSpectrum(float[] i_spectrumSegment)
    {
        float o_average = 0;

        foreach (float i in i_spectrumSegment)
        {
            o_average += i;
        }

        return (o_average / i_spectrumSegment.Length) * 100;
    }

    private void SaveDataToJson(Song i_songToSave)
    {
        Debug.Log("Saved");

        string data = JsonUtility.ToJson(i_songToSave);
        string path = "Assets/Jsons/";
        path += currentAudiosource.clip.name;
        path += ".json";

        System.IO.File.WriteAllText(path, data);

        UnityEditor.AssetDatabase.Refresh();
    }
}
