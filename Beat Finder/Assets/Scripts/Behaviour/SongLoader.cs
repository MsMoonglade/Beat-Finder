using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SongLoader : MonoBehaviour
{
    Song savedSong ;

    private AudioSource clipAudioSource;
    private AudioSource originalSongAudioSource;
    private Coroutine playSongCoroutine;

    private void Awake()
    {
        playSongCoroutine = null;
    }

    public void Load(TextAsset i_jsonFile)
    {
        savedSong = LoadJsonToData(i_jsonFile);
    }
    private Song LoadJsonToData(TextAsset i_jsonFile)
    {
        Assert.IsNotNull(i_jsonFile);
        Song o_song = new Song();
        o_song = JsonUtility.FromJson<Song>(i_jsonFile.text);

        return o_song;
    }

    public void StartPlaySongInJson(AudioSource i_clipAudioSource , AudioSource i_originalSongAudioSource)
    {
        clipAudioSource = i_clipAudioSource;
        originalSongAudioSource = i_originalSongAudioSource;

        if (playSongCoroutine == null)
            playSongCoroutine = StartCoroutine(StartSavedSong());
    }

    private void PlayClip(float i_percentValue)
    {
        AudioClip clip = ReferenceByValue.instance.ReturnValue<AudioClip>(ReferenceByValue.instance.possibleClip, i_percentValue);
        clipAudioSource.PlayOneShot(clip);
    }   

    private IEnumerator StartSavedSong()
    {
        double timer = 0;
        int currentSongBeatIndex = 0;

        yield return new WaitForSeconds(0.5f);

        Debug.Log("Start Play");

        originalSongAudioSource.volume = 0.15f;
        originalSongAudioSource.Play();

        while (timer < savedSong.songDuration)
        {
            timer += Time.unscaledDeltaTime;

            if (savedSong.beats.Count > currentSongBeatIndex)
            {
                if (timer >= savedSong.beats[currentSongBeatIndex].time)
                {
                    float percentValue = savedSong.GetSpectrumValuePercentage(currentSongBeatIndex);
                    PlayClip(percentValue);
                    currentSongBeatIndex++;
                }
            }

            yield return null;
        }

        Debug.Log("End Play");
    }
}
