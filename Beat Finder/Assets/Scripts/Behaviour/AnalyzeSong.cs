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
    public AudioSource songSource;
    public AudioSource clipSource;

    public TextAsset jsonFileToAnalyze;

    [SerializeField]
    public Song currentSong;

    private SongAnalyzer songAnalyzer;
    private SongLoader songLoader;

    private void Awake()
    {
        songAnalyzer = GetComponentInChildren<SongAnalyzer>();
        songLoader = GetComponentInChildren<SongLoader>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            songAnalyzer.StartAnalyze(songSource);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            currentSong = LoadJsonToData();

            StartCoroutine(PlaySimplifiedSong());
        }
    }

    private Song LoadJsonToData()
    {
        Assert.IsNotNull(jsonFile);
        Song song = new Song();
        song = JsonUtility.FromJson<Song>(jsonFile.text);

        return song;
    }



    public IEnumerator PlaySimplifiedSong()
    {
        double timer = 0;
        int currentSongBeatIndex = 0;

        yield return new WaitForSeconds(0.5f);

        Debug.Log("Start Play");

        songSource.volume = 0.25f;
        songSource.Play();

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
        songSource.PlayOneShot(clip);
    }
}