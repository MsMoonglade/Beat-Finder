using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class AnalyzeSong : MonoBehaviour
{
    public int partPerBpm;

    public AudioSource sourceToAnalyze;
    public AudioSource playClipsource;
    public TextAsset jsonFile;

    private int bpm;
    private Coroutine beatCoroutine;

    [SerializeField]
    public Song currentSong;

    void Start()
    {
        bpm = UniBpmAnalyzer.AnalyzeBpm(sourceToAnalyze.clip);

        if (bpm < 0)
        {
            Debug.LogError("AudioClip is null.");
            return;
        }

        Debug.Log("BPM is " + bpm);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            beatCoroutine = StartCoroutine(SongAnalyzerCoroutine(bpm));
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            currentSong = LoadJsonToData();

            StartCoroutine(PlaySimplifiedSong());
        }
    }

    private IEnumerator SongAnalyzerCoroutine(int i_bpm)
    {
        sourceToAnalyze.Play();

        float beatInterval = 60f / i_bpm;
        float time = 0;
        float songtime = 0;

        List<Beat> thisSongBeat = new List<Beat>();

        while (sourceToAnalyze.isPlaying)
        {
            float[] spectrum = new float[128];

            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

            time += Time.deltaTime;

            if (time >= (beatInterval / partPerBpm))
            {
                float averageBeatValue = ReturnAverage(spectrum);

                Beat beat = new Beat(sourceToAnalyze.time , averageBeatValue);

                thisSongBeat.Add(beat);

                time = 0;
            }

            yield return null;
        }

        Song o_analyzedSong = new Song(sourceToAnalyze.clip.name , sourceToAnalyze.clip.length , thisSongBeat);

        SaveDataToJson(o_analyzedSong);
    }

    private void SaveDataToJson(Song i_songToSave)
    {
        Debug.Log("Saved");

        string data = JsonUtility.ToJson(i_songToSave);
        string path = "Assets/Jsons/";
        path += sourceToAnalyze.clip.name;
        path += ".json";

        System.IO.File.WriteAllText(path , data);

        UnityEditor.AssetDatabase.Refresh();
    }

    private Song LoadJsonToData()
    {
        Debug.Log("Loaded");

        Assert.IsNotNull(jsonFile);
        Song song = new Song();
        song = JsonUtility.FromJson<Song>(jsonFile.text);

        return song;
    }

    private float ReturnAverage(float[] i_initial)
    {
        float o_average = 0;

        foreach (float i in i_initial)
        {
            o_average += i;
        }

        return (o_average / i_initial.Length) * 100;
    }

    public IEnumerator PlaySimplifiedSong()
    {
        double timer = 0;
        int currentSongBeatIndex = 0;

        currentSong.MaxSpectrum();

        yield return new WaitForSeconds(0.5f);

        Debug.Log("Start Play");

        sourceToAnalyze.volume = 0.25f;
        sourceToAnalyze.Play();

        while (timer < currentSong.songDuration)
        {
            timer += Time.unscaledDeltaTime;

            if (currentSong.beats.Count > currentSongBeatIndex)
            {
                if (timer >= currentSong.beats[currentSongBeatIndex].time)
                {
                    float percentValue = currentSong.GetSpectrumValuePercentage(currentSongBeatIndex);
                    PlayClip(percentValue);
                    currentSongBeatIndex++;
                }
            }

            yield return null;
        }

        Debug.Log("End Play");
    }

    private void PlayClip(float i_percentValue)
    {        
        AudioClip clip = ReferenceByValue.instance.ReturnValue<AudioClip>(ReferenceByValue.instance.possibleClip, i_percentValue);
        playClipsource.PlayOneShot(clip);
    }
}