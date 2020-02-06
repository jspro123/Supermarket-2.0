using System;
using System.Collections.Generic;
using UnityEngine;

public class TagsUtility : MonoBehaviour
{
    //Searches through the current list of tags and returns the value if the "key" is found.
    //Tags with keys and values are written in the form key/value
    public static string GetTagWithKey(string key, List<string> tags)
    {
        string found_tag = tags.Find(x => x.StartsWith(key, StringComparison.Ordinal));
        return (found_tag == null) ? key : found_tag.Split('/')[1];
    }

    //Checks if a tag exists.
    public static bool TagExists(string name, List<string> tags)
    {
        return (tags.Find(x => x.Equals(name, StringComparison.Ordinal)) != null);
    }

    //Adds the <color> tag to a string.
    public static string AddColorTag(string text, Color32 color)
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGBA((color)) + ">" + text + "</color>";
    }

    public static string CheckStringWithKey(string key, string comp)
    {
        bool found_tag = comp.StartsWith(key, StringComparison.Ordinal);
        return (!found_tag) ? key : comp.Split('/')[1];
    }

}
