using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Sound Options")]
public class SoundInfo : ScriptableObject
{
    public string sound_name;
    public AudioClip sound;
    [Tooltip("How many seconds to fade out over. Set to 0 to suddenly cut. Non-negative. ")]
    public float fade_out;
    [Tooltip("If true, will play until the sound effect is finished playing.")]
    public bool playTillFinished;
    [Tooltip("If true, will only cut when requested. Otherwise, cuts on the next 'page' of text. ")]
    public bool cutAtTrigger;
    [Tooltip("If true, will loop the sound effect. ")]
    public bool loop;

    //Fade in?
    //Force kill? (If a player goes through the text too quickly)
}

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

    IEnumerator FadeOutClip(string name)
    {
        //Both guaranteed to exist
        SoundInfo clip = soundsDict[name];
        AudioSource source = soundsPlaying[name];
        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / clip.fade_out;
            yield return null;
        }

        source.Stop();
        if(source != null) { Destroy(source); }
        soundsPlaying.Remove(name);
        yield break;
    }

    private void KillSound(string name)
    {
        SoundInfo clip;
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

            if(clip.fade_out == 0) {
                soundsPlaying[name].Stop();
                if (soundsPlaying[name] != null) { Destroy(soundsPlaying[name]); }
                soundsPlaying.Remove(name);
            }
            else { StartCoroutine(FadeOutClip(name)); }
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
            Debug.Log("Trying to play something that doesn't exits.");
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
                entry.Value.Stop();
                if (entry.Value != null) { Destroy(entry.Value); }
                soundsPlaying[entry.Key] = null;
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
