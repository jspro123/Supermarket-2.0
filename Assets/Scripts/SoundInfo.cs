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
