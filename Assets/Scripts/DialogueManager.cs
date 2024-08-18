using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float delayBetweenMessages = 1f;
    [SerializeField] private AudioClip[] typeClips;

    private Queue<string> sentences;
    public bool hasPlayedEndDialogue = false;

    void Start()
    {
        sentences = new Queue<string>();
        StartDialogue();
    }

    void StartDialogue()
    {
        sentences.Clear();
        sentences.Enqueue("[Home Base]: Come in, Astronaut. Do you read me? This is Mission Control.");
        sentences.Enqueue("[Astronaut]: I read you, Mission Control. What's the situation?");
        sentences.Enqueue("[Home Base]: We've lost contact with you for hours. Your suit's telemetry is... unusual. What's your status?");
        sentences.Enqueue("[Astronaut]: I'm... I'm not sure how to explain this. Something's happened to me. I can move objects with my mind now.");
        sentences.Enqueue("[Home Base]: ...Say again, Astronaut? Did you say you can move objects with your mind?");
        sentences.Enqueue("[Astronaut]: Affirmative. I know it sounds crazy, but it's true. I think it happened when I touched that strange artifact.");
        sentences.Enqueue("[Home Base]: This is unprecedented. We need to get you back immediately for analysis and debriefing. Can you see the rescue beacon?");
        sentences.Enqueue("[Astronaut]: Negative. My surroundings are unfamiliar. Lots of debris and strange structures.");
        sentences.Enqueue("[Home Base]: Listen carefully. We've activated the emergency rescue point. It should be emitting a strong signal. You need to navigate to it using your... new ability.");
        sentences.Enqueue("[Astronaut]: Understood. Any idea what I'm up against?");
        sentences.Enqueue("[Home Base]: Unknown. But be cautious. Your suit's integrity is compromised, and we're detecting unstable energy readings in your vicinity. Use your telekinesis wisely.");
        sentences.Enqueue("[Astronaut]: Copy that. I'll make my way to the rescue point. Any other advice?");
        sentences.Enqueue("[Home Base]: Remember your training. Stay calm, conserve your suit's power, and... well, we never trained for telekinesis, but don't overexert yourself. We'll maintain communication as long as possible. Good luck, Astronaut. Bring yourself home.");
        sentences.Enqueue("[Astronaut]: Roger that, Mission Control. I'm on my way.");

        StartCoroutine(TypeDialogue());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("End") && !hasPlayedEndDialogue)
        {
            StopAllCoroutines();
            sentences.Clear();
            sentences.Enqueue("[Home Base]: Thank you for playing brave Astronaut. We hope you enjoyed the demo!");

            hasPlayedEndDialogue = true;
            StartCoroutine(TypeDialogue());
        }
    }

    IEnumerator TypeDialogue()
    {
        dialogueText.gameObject.SetActive(true);
        
        while(sentences.Count > 0)
        {
            string currentSentence = sentences.Dequeue();
            yield return StartCoroutine(TypeSentence(currentSentence));
            yield return new WaitForSeconds(delayBetweenMessages);
        }

        yield return new WaitForSeconds(delayBetweenMessages);
        dialogueText.gameObject.SetActive(false);
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            SoundFXManager.instance.PlayRandomSoundFXClip(typeClips, transform, 1f);
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}