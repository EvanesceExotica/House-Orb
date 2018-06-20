using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
public class DialogueDisplayer : MonoBehaviour
{
    Transform dialogueSpace;

    Transform lineStart;
    CanvasGroup ourCanvasGroup;
    Image image;
    GameObject speaker;
    LineRenderer ourLineRenderer;
    // Use this for initialization
    Speech hintGivenComment;
    Speech previousSconceTeleportGivenComment;

    Speech autoRepelComment;

    TextMeshProUGUI ourText;

    Color currentColor;
    enum MemoryReaction
    {
        Hint,
        SconceTeleport,
        AutoReflect
    }

    public static event Action DialogueDisplayed;

    public void DialogueDisplayedWrapper()
    {
        if (DialogueDisplayed != null)
        {
            DialogueDisplayed();
        }
    }
    MemoryReaction reactionToMemory;
    void ReactToMemoryHover()
    {

    }
    void Awake()
    {
        RectTransform rectTransform = (RectTransform)transform;
        lineStart = rectTransform.Find("LineStartPosition");
        ourText = GetComponentInChildren<TextMeshProUGUI>();
        dialogueSpace = GameObject.Find("DialogueSpace").transform;
        ourLineRenderer = GetComponentInChildren<LineRenderer>();
        image = GetComponentInChildren<Image>();
        ourCanvasGroup = GetComponent<CanvasGroup>();
        Memory.HoveringOverMemoryObject += ReactToMemoryHover;
        SpeechTrigger.SpeechTriggered += SetSpeech;
        ourCanvasGroup.alpha = 0;
        ourLineRenderer.material.DOFade(0, 0);
        //ourLineRenderer.material.color = new Color(0, 0, 0, 0);
    }

    void SetSpeech(Speech speech)
    {
        Debug.Log("Speech triggered");
        StartCoroutine(React(1.0f, speech));
    }

    IEnumerator React(float delay, Speech speech)
    {
        SetSpeechBubble(speech);
        yield return new WaitForSeconds(delay);
        FadeIn();
        yield return StartCoroutine(TypeText(speech));
        yield return new WaitForSeconds(delay);
        FadeOut();
    }

    IEnumerator TypeText(Speech speech)
    {
        string speechText = speech.GrabRandomTextChoice();
        float delayTime = 0.1f;
        foreach (char letter in speechText)
        {
            if (letter == '^')
            {
                //we put this character before ellipses to add pause
                delayTime = 0.3f;
            }
            else
            {
                delayTime = 0.1f;
                ourText.text += letter;
            }
            yield return new WaitForSeconds(delayTime/*speech.textSpeed*/);
        }
    }

    void SetSpeechBubble(Speech speech)
    {
        ourText.text = " ";
        transform.position = dialogueSpace.transform.position;
        speech.SetTextColor();
        currentColor = speech.bubbleColor;
        image.color = currentColor;
        ourLineRenderer.startColor = currentColor;
        GameObject ourSpeaker = ReturnSpeaker(speech);
        Debug.Log("This is our speaker " + ourSpeaker.name);
        if (ourSpeaker != null)
        {
            Debug.Log(ourSpeaker.transform.position);
            Vector2 speechBubbleLineStartPosition = GameHandler.bubbleLineStartTransform.position;
            ourLineRenderer.SetPosition(0, speechBubbleLineStartPosition);
            ourLineRenderer.SetPosition(1, ourSpeaker.transform.position);
        }
    }

    GameObject ReturnSpeaker(Speech speech)
    {
        GameObject ourSpeaker = null;
        if (speech.speaker == Speech.OurSpeaker.player)
        {
            ourSpeaker = GameHandler.playerGO;
        }
        else if (speech.speaker == Speech.OurSpeaker.orb)
        {
            ourSpeaker = GameHandler.fatherOrbGO;
        }
        return ourSpeaker;
    }

    void FadeIn()
    {

        ourCanvasGroup.DOFade(1, 0.5f);
        ourLineRenderer.DOColor(new Color2(Color.clear, Color.clear), new Color2(currentColor, Color.white), 0.5f);
        ourLineRenderer.material.DOFade(1, 5.0f);
    }

    void FadeOut()
    {
        ourCanvasGroup.DOFade(0, 0.5f);
        ourLineRenderer.DOColor(new Color2(ourLineRenderer.startColor, ourLineRenderer.endColor), new Color2(Color.clear, Color.clear), 0.5f);
        ourLineRenderer.material.DOFade(0, 0.5f);
    }


   
}
