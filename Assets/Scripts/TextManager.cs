using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using TMPro;

public class TextManager : MonoBehaviour
{
    public static event Action<Story> OnCreateStory;

    [SerializeField]
    private TextAsset inkJSONAsset;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject choicePrefab;
    [SerializeField]
    private TMPro.TextMeshProUGUI textPrefab;
    [SerializeField]
    private int textSize; //Font size
    [SerializeField]
    private int choiceSize;
    [SerializeField]
    private SoundManager soundManager;

    private int numSpaces = 8; //For some reason, tabbing produces strange results.
    public Story story;
    private GameObject textArea;
    private VerticalLayoutGroup textAreaVertical;

    public List<DialogueConfig> speakers; //My favorite hack/workaround to not having Odin :(
    private Dictionary<string, DialogueConfig> speakersDic;

    //For skipping through text if the user clicks early. 
    private bool mouseKeyPressed = false;
    private bool mouseKeyReleased = true;
    private bool pickingChoice = false;

    void Start()
    {
        textArea = canvas.transform.GetChild(0).gameObject;
        textAreaVertical = textArea.GetComponent<VerticalLayoutGroup>();
        speakersDic = new Dictionary<string, DialogueConfig>();

        for (int i = 0; i < speakers.Count; i++)
        {
            speakersDic.Add(speakers[i].name, speakers[i]);
        }

        StartStory();
    }

    void StartStory()
    {
        story = new Story(inkJSONAsset.text);
        if (OnCreateStory != null) OnCreateStory(story);
        RefreshView();
    }

    //Adds numSpaces spaces to a string.
    string AddSpaces(string text)
    {
        string spaces = "";
        for (int i = 0; i < numSpaces; i++)
        {
            spaces += " ";
        }
        return spaces + text;
    }

    //Slowly draws text on a screen. Skippable. 
    IEnumerator DrawText(string text, string speakerName)
    {
        TMPro.TextMeshProUGUI storyText = Instantiate(textPrefab) as TMPro.TextMeshProUGUI;
        storyText.transform.SetParent(textArea.transform, false);
        storyText.maxVisibleCharacters = 0;

        Assert.IsTrue(speakersDic.ContainsKey(speakerName));
        DialogueConfig speaker = speakersDic[speakerName];
        string addColor = TagsUtility.AddColorTag(text, speaker.speaker_color);
        storyText.text += addColor;
        storyText.fontSize = textSize;

        for (int i = 0; i < text.Length; i++)
        {
            if (mouseKeyPressed)
            {
                mouseKeyPressed = false;
                storyText.maxVisibleCharacters += (text.Length - i);
                break;
            }
            storyText.maxVisibleCharacters++;
            yield return new WaitForSeconds(speaker.time_between_characters);
        }

        yield return new WaitForSeconds(speaker.time_between_sentences);
        RefreshView();
    }

    //Displays the choices as text on a screen and adds listeners to the buttons. 
    IEnumerator DisplayChoices()
    {
        TMPro.TextMeshProUGUI blank = Instantiate(textPrefab) as TMPro.TextMeshProUGUI;
        blank.transform.SetParent(textArea.transform, false);
        blank.text = " ";

        DialogueConfig choices = speakersDic["Choice"];

        for (int i = 0; i < story.currentChoices.Count; i++)
        {
            Choice choice = story.currentChoices[i];
            GameObject button = Instantiate(choicePrefab);
            button.transform.SetParent(textArea.transform);

            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                story.ChooseChoiceIndex(choice.index);
                pickingChoice = false;
                RefreshView();
            });

            TMPro.TextMeshProUGUI buttonCurrentText = button.GetComponent<TMPro.TextMeshProUGUI>();
            buttonCurrentText.fontSize = choiceSize;
            buttonCurrentText.text = AddSpaces(choice.text);
            buttonCurrentText.maxVisibleCharacters = 0;
            button.transform.localScale = new Vector3(1, 1, 1);
            LayoutRebuilder.ForceRebuildLayoutImmediate(textAreaVertical.GetComponent<RectTransform>());

            for (int j = 0; j < choice.text.Length + numSpaces; j++)
            {
                buttonCurrentText.maxVisibleCharacters++;
                yield return new WaitForSeconds(choices.time_between_characters);
            }

            yield return new WaitForSeconds(choices.time_between_sentences);
        }
        yield break;
    }

    //Where we check if the story can advance.
    void RefreshView()
    {
        if (story.canContinue)
        {
            string text = story.Continue();
            if(!TagsUtility.TagExists("continue", story.currentTags)) { RemoveChildren(); soundManager.FlippedPage(); }
            text = text.Trim();
            string speakerName = TagsUtility.GetTagWithKey("speaker/", story.currentTags);
            speakerName = (speakerName == "speaker/") ? "Default" : speakerName;
            StartCoroutine(DrawText(text, speakerName));
            soundManager.HandleTags(story.currentTags);
        }

        else if (story.currentChoices.Count > 0)
        {
            pickingChoice = true;
            StartCoroutine(DisplayChoices());
        }

        else
        {
            //END reached. Do something definitive.
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && mouseKeyReleased && !pickingChoice)
        {
            mouseKeyPressed = true;
            mouseKeyReleased = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseKeyPressed = false;
            mouseKeyReleased = true;
        }
    }

    //Cleans up the text and choice objects.
    void RemoveChildren()
    {
        int childCount = textArea.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            GameObject.Destroy(textArea.transform.GetChild(i).gameObject);
        }
    }
}
