using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BeatDetector : MonoBehaviour
{
    public int partPerBpm;

    public AudioSource songSource;

    private Coroutine beatCoroutine;


    void Start()
    {
        int bpm = UniBpmAnalyzer.AnalyzeBpm(songSource.clip);

        if (bpm < 0)
        {
            Debug.LogError("AudioClip is null.");
            return;
        }

        Debug.Log("BPM is " + bpm);

        beatCoroutine = StartCoroutine(BeatCoroutine(bpm));
    }

    private IEnumerator BeatCoroutine(int i_bpm)
    {
        songSource.Play();

        float beatInterval = 60f / i_bpm;
        float time = 0;

        while (true)
        {
            float[] spectrum = new float[128];

            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

            time += Time.deltaTime;

            if (time >= (beatInterval / partPerBpm ))
            {
                //clipSource.Play();

                float value = ReturnAverage(spectrum);

                //if(value>= 0.5f)
                  //  clipSource.Play();

                Debug.Log(value);

                time =0 ;
            }

            yield return null;
        }
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