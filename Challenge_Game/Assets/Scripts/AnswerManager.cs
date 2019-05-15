using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Answers
{
    // Answers written down in your buttons
    public string[] answerText;

    // Value of each option
    public int[] values;

    // The reaction of the client after you click a button
    public Reactions[] reactionSentences;

    [HideInInspector]
    public int index;
}

[System.Serializable]
public class Reactions
{
    public Sentences[] sentences;

    [HideInInspector]
    public int index;
}

public class AnswerManager : MonoBehaviour
{
    public Button[] answerButtons;

    public Answers[] answerArray;

    [HideInInspector]
    public int answerNumber;

    public ClientSentences clientScript;
    public PointManager pointScript;

    // weird ass button logic
    public void OnEnable()
    {
        // when clicked it will call the "CallClientReaction" function, and pass on the appropriate reaction and add the appropriate amount of points
        for(int i = 0; i < answerButtons.Length; i++)
        {
            int closureIndex = i;
            answerButtons[closureIndex].onClick.AddListener(() => callClientReaction(answerArray[answerNumber].reactionSentences[closureIndex].sentences));
            answerButtons[closureIndex].onClick.AddListener(() => addPoints(answerArray[answerNumber].values[closureIndex]));
        }
    }

    // call the client writing script to make them write a reaction
    public void callClientReaction(Sentences[] reaction)
    {
        clientScript.reactionIndex = 0;
        clientScript.StartCoroutine(clientScript.typeReaction(reaction));
    }

    public void OnDisable()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int closureIndex = i;
            answerButtons[closureIndex].onClick.RemoveAllListeners();
        }
    }

    // Add Points
    public void addPoints(int value)
    {
        pointScript.AddPoints(value);
    }

    // Display Buttons
    public void DisplayAnswers()
    {
        for(int i = 0; i < answerArray[answerNumber].answerText.Length; i++)
        {
            // Enable the buttons
            answerButtons[i].gameObject.SetActive(true);
            // Attribute your text to them
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answerArray[answerNumber].answerText[i];
        }

    }

    // Hide Buttons
    public void DisableAnswers()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            // Disable the buttons
            answerButtons[i].gameObject.SetActive(false);
        }
    }
}
