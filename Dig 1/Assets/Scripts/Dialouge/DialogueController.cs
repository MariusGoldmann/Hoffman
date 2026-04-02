using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NPCNameText;
    [SerializeField] TextMeshProUGUI NPCDialogueText;

    [SerializeField] float typeSpeed = 5f;
    [SerializeField] float MaxTypeTime = 0.1f;
    

    Queue<string> paragraphs = new Queue<string>();

    bool conversationEnded;
    bool isTyping;
    [SerializeField] bool isInDialogue;

    string paragraph;
    Coroutine typewriterCoroutine;



    public void DisplayNextParagraph(DialogueText dialogueText)
    {
        isInDialogue = true;
        if (paragraphs.Count == 0)
        {
            if (!conversationEnded)
            {
                StartConversation(dialogueText);
            }
            else if (conversationEnded && !isTyping)
            {
                EndConversation();
                return;
            }
        }

        if (!isTyping)
        {
            paragraph = paragraphs.Dequeue();

            typewriterCoroutine = StartCoroutine(Typewriter(paragraph));
        }
        else
        {
            FinishParagraphEarly();
        }

        if (paragraphs.Count == 0)
        {
            conversationEnded = true;
        }
    }

    void StartConversation(DialogueText dialogueText)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        NPCNameText.text = dialogueText.npcName;

        for (int i = 0; i < dialogueText.paragraphs.Length; i++)
        {
            paragraphs.Enqueue(dialogueText.paragraphs[i]);
        }
    }
    public void EndConversation()
    {
        isInDialogue = false;
        conversationEnded = false;

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator Typewriter(string paragraph)
    {
        isTyping = true;

        int maxVisibleChars = 0;

        NPCDialogueText.text = paragraph;
        NPCDialogueText.maxVisibleCharacters = maxVisibleChars;

        foreach (char c in paragraph.ToCharArray())
        {

            maxVisibleChars++;
            NPCDialogueText.maxVisibleCharacters = maxVisibleChars;

            yield return new WaitForSeconds(MaxTypeTime / typeSpeed);
        }

        isTyping = false;
    }

    void FinishParagraphEarly()
    {
        StopCoroutine(typewriterCoroutine);

        NPCDialogueText.maxVisibleCharacters = paragraph.Length;

        isTyping = false;
    }

    public bool GetIsInDialogue()
    {
        return isInDialogue;
    }
}
