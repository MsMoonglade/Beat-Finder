using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SongAnalyzerManager : MonoBehaviour
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            songLoader.Load(jsonFileToAnalyze);
            songLoader.StartPlaySongInJson(clipSource , songSource);
        }
    }
}
