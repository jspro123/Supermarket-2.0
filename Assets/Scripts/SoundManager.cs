using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private List<SoundInfo> soundsList;
    private Dictionary<string, SoundInfo> soundsDict;
    private Dictionary<string, AudioSource> soundsPlaying;
    public AudioSource sourcePrefab;
    public GameObject soundParent;

    private void Awake()
    {
        soundsDict = new Dictionary<string, SoundInfo>();
        soundsPlaying = new Dictionary<string, AudioSource>();

        for (int i = 0; i < soundsList.Count; i++)
        {
            soundsDict.Add(soundsList[i].name, soundsList[i]);
        }

        //soundsList.Clear();
    }

    IEnumerator FadeOutClip(AudioSource source, SoundInfo clip)
    {
        float startVolume = source.volume;
        source.volume = (clip.fade_out == 0) ? 0 : source.volume;

        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / clip.fade_out;
            yield return null;
        }

        if(source != null) { source.Stop(); Destroy(source.gameObject); }
        yield break;
    }

    private void KillSound(string name)
    {
        SoundInfo clip;
        AudioSource source;
        if (soundsDict.ContainsKey(name))
        {
            clip = soundsDict[name];
        }
        else
        {
            Debug.Log("Trying to kill something that doesn't exist.");
            return;
        }

        if (soundsPlaying.ContainsKey(name) && soundsPlaying[name] != null)
        {
            if (clip.playTillFinished)
            {
                Debug.Log("Play till finished set to true! Aborting...");
                return;
            }

            source = soundsPlaying[name];
            soundsPlaying.Remove(name);
            StartCoroutine(FadeOutClip(source, clip));
        }
    }

    private void PlaySound(string name)
    {
        SoundInfo clip;
        if (soundsDict.ContainsKey(name))
        {
            clip = soundsDict[name];
        } else
        {
            Debug.Log("Trying to play something that doesn't exist: " + name);
            return;
        }

        if (soundsPlaying.ContainsKey(name) && soundsPlaying[name] != null)
        {
            //What do
        } else
        {
            soundsPlaying[name] = Instantiate(sourcePrefab) as AudioSource;
            soundsPlaying[name].transform.SetParent(soundParent.transform);
            soundsPlaying[name].clip = clip.sound;
            if(clip.loop) { soundsPlaying[name].loop = true; }
            soundsPlaying[name].Play();
        }
    }

    public void HandleTags(List<string> tags)
    {
        CleanUp();
        for (int i = 0; i < tags.Count; i++)
        {
            string key = tags[i].Split('/')[0];
            switch (key)
            {
                case "Sound":
                    PlaySound(tags[i].Split('/')[1]);
                    break;

                case "Kill":
                    KillSound(tags[i].Split('/')[1]);
                    break;

                default:
                    break;
            }
        }
    }

    private void CleanUp()
    {
        foreach (KeyValuePair<string, AudioSource> entry in soundsPlaying)
        {
            if (!entry.Value.isPlaying)
            {
                KillSound(entry.Key);
            }
        }
    }

    public void FlippedPage()
    {
        foreach (KeyValuePair<string, AudioSource> entry in soundsPlaying)
        {
            if (soundsDict.ContainsKey(entry.Key))
            {
                SoundInfo info = soundsDict[entry.Key];
                if (!info.cutAtTrigger)
                {
                    KillSound(entry.Key);
                }
            } else
            {
                Debug.Log("Something is rotten here...");
            }
        }
    }
}
