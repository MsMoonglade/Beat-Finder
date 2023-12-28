using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BeatDetector : MonoBehaviour
{
    public AudioSource songSource;
    public AudioSource clipSource;

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
        clipSource.Play();

        float beatInterval = 60f / i_bpm;
        float time = 0;

        while (true)
        {
            time += Time.deltaTime;

            if (time >= beatInterval)
            {
                clipSource.Play();
                time -= beatInterval;
            }

            yield return null;
        }
    }
}