using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class AnalyzeSong : MonoBehaviour
{
    public int partPerBpm;

    public AudioSource sourceToAnalyze;
    public TextAsset jsonFile;

    private int bpm;
    private Coroutine beatCoroutine;

    [SerializeField]
    public Beats songData;

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
            songData = LoadJsonToData();
        }
    }

    private IEnumerator SongAnalyzerCoroutine(int i_bpm)
    {
        sourceToAnalyze.Play();

        float beatInterval = 60f / i_bpm;
        float time = 0;
        float songtime = 0;

        while (sourceToAnalyze.isPlaying)
        {
            float[] spectrum = new float[128];

            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

            time += Time.deltaTime;

            if (time >= (beatInterval / partPerBpm))
            {
                float averageBeatValue = ReturnAverage(spectrum);

                Beat beat = new Beat(sourceToAnalyze.time , averageBeatValue);

                songData.beats.Add(beat);

                time = 0;
            }

            yield return null;
        }

        SaveDataToJson();
    }

    private void SaveDataToJson()
    {
        Debug.Log("Saved");

        string data = JsonUtility.ToJson( songData );
        string path = "Assets/Jsons/";
        path += sourceToAnalyze.clip.name;
        path += ".json";

        System.IO.File.WriteAllText(path , data);

        UnityEditor.AssetDatabase.Refresh();
    }

    private Beats LoadJsonToData()
    {
        Debug.Log("Loaded");

        Assert.IsNotNull(jsonFile);
        Beats beats = new Beats();
        beats = JsonUtility.FromJson<Beats>(jsonFile.text);

        return beats;
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
}