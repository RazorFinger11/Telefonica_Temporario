using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Questions
{
    public Sentences[] sentences;

    [HideInInspector]
    public int index;
}

[System.Serializable]
public class Sentences
{
    public string sentence;
    public Sprite sprite;

    // Things that should go in this class:  Sounds, Effects, background changes, music changes, anything that is triggered on dialogue 
}

public class ClientSentences : MonoBehaviour
{
    public GameObject blackScreen;

    public TextMeshProUGUI textDisplay;
    public Questions[] questionArray;

    public SpriteRenderer sprRender;

    [HideInInspector]
    public int questionNumber = 0;

    public float typeSpeed = 0.02f;

    public AnswerManager answerScript;

    private bool finishedReaction;

    [HideInInspector]
    public int reactionIndex = 0;
    string[] reactions;
    Sprite[] sprites;
    Sentences[] sentencesInReactionArray;

    private void Start()
    {
        // Start question
        StartCoroutine(typeQuestion());

        Debug.Log(reactions);
        
    }

    private void Update()
    {
        // If the text is done showing, you can call the next one
        if (textDisplay.text == questionArray[questionNumber].sentences[questionArray[questionNumber].index].sentence)
        {
            // When player press button, if there is still sentences left in the question, keep writing them.
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (questionArray[questionNumber].index < questionArray[questionNumber].sentences.Length - 1)
                {
                    // stop writing current sentence and move to next one
                    StopCoroutine(typeQuestion());

                    textDisplay.text = "";
                    
                    questionArray[questionNumber].index++;
                    StartCoroutine(typeQuestion());
                }
                // If all sentences in current questions have been said
                else
                {
                    StopCoroutine(typeQuestion());
                    answerScript.DisplayAnswers();
                }
            }
        }

        // Moving forward with reactions
        if(textDisplay.text == sentencesInReactionArray[reactionIndex].sentence)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // If we still have a reaction sentence, say it
                if (reactionIndex < sentencesInReactionArray.Length - 1)
                {
                    reactionIndex++;
                    StartCoroutine(typeReaction(sentencesInReactionArray));
                }
                // If we are done with reactions, move to next question (Or end game)
                else
                {
                    // If we still have both questions and answers, continue game
                    if (questionNumber < questionArray.Length -1 && answerScript.answerNumber < answerScript.answerArray.Length -1)
                    {
                        Debug.Log("we do get here right");
                        // Stop reactions
                        StopCoroutine(typeReaction(sentencesInReactionArray));
                        // Reset current text
                        textDisplay.text = "";
                        // Go to next question
                        questionNumber++;
                        // Go to next Answer
                        answerScript.answerNumber++;
                        // get to the first sentence of the current question
                        questionArray[questionNumber].index = 0;

                        // Weird ass button logic
                        answerScript.OnDisable();
                        answerScript.OnEnable();

                        // Start typing
                        StartCoroutine(typeQuestion());
                    }
                    // If we're done, end it
                    else
                    {
                        blackScreen.SetActive(true);
                    }
                }

            }
        }

    }

    // Type the client question slowly
    IEnumerator typeQuestion()
    {
        // change sprite
        sprRender.sprite = questionArray[questionNumber].sentences[questionArray[questionNumber].index].sprite;

        // write sentence letter by letter
        foreach (char letter in questionArray[questionNumber].sentences[questionArray[questionNumber].index].sentence.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    // type the reaction the client will have to your answer
    public IEnumerator typeReaction(Sentences[] _reaction)
    {
        textDisplay.text = "";

        /*for (int i = 0; i <= _reaction.Length; i++)
        {
            reactions[i] = _reaction[i].sentence;
            sprites[i] = _reaction[i].sprite; 
        }*/

        sentencesInReactionArray = _reaction;

        sprRender.sprite = sentencesInReactionArray[reactionIndex].sprite;

        // type every letter
        foreach (char letter in sentencesInReactionArray[reactionIndex].sentence)
        {
            Debug.Log(_reaction[reactionIndex].sentence);
            textDisplay.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}
