using System.Collections;
using UnityEngine;

//Stores informations about various speakers in the story.
[CreateAssetMenu(menuName = "Speaker Options")]
public class DialogueConfig : ScriptableObject
{
    public string speaker_name;
    public TMPro.TMP_FontAsset font;
    public float time_between_sentences;
    public float time_between_characters;
    public Color32 speaker_color;
    //Sounds while playing?
}
