using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI NPCNameText;
	[SerializeField] private TextMeshProUGUI NPCDialogueText;
	[SerializeField]         TextMeshProUGUI skipText;

	[SerializeField] private float typeSpeed   = 5f;
	[SerializeField] private float MaxTypeTime = 0.1f;

	[SerializeField] private float fadeSpeed;


	private Queue<string> paragraphs = new Queue<string>();

	private                  bool conversationEnded;
	private                  bool isTyping;
	[SerializeField] private bool isInDialogue;

	private string    paragraph;
	private Coroutine typewriterCoroutine;

	[SerializeField] private CanvasGroup canvasGroup;

	public DialogueController(TextMeshProUGUI skipText, bool conversationEnded) {
		this.skipText          = skipText;
		this.conversationEnded = conversationEnded;
	}

	private void Start() {
		canvasGroup.alpha = 0;
	}

	public void DisplayNextParagraph(DialogueText dialogueText) {
		isInDialogue = true;
		if (paragraphs.Count == 0) {
			switch (conversationEnded) {
				case false:
					StartConversation(dialogueText);
					break;
				case true when !isTyping:
					EndConversation();
					return;
			}
		}

		if (!isTyping) {
			paragraph = paragraphs.Dequeue();

			typewriterCoroutine = StartCoroutine(Typewriter(paragraph));
		} else {
			FinishParagraphEarly();
		}

		if (paragraphs.Count == 0) {
			conversationEnded = true;
		}
	}

	private void StartConversation(DialogueText dialogueText) {
		if (!gameObject.activeSelf) {
			gameObject.SetActive(true);
			LeanTween.alphaCanvas(canvasGroup, 1, fadeSpeed).setEaseInOutSine();
		}

		NPCNameText.text = dialogueText.npcName;


		foreach (var t in dialogueText.paragraphs) {
			paragraphs.Enqueue(t);
		}
	}

	private void EndConversation() {
		isInDialogue      = false;
		conversationEnded = false;

		if (!gameObject.activeSelf) return;
		canvasGroup.alpha = 0;
		gameObject.SetActive(false);
	}

	private IEnumerator Typewriter(string paragraph) {
		isTyping = true;

		var maxVisibleChars = 0;

		NPCDialogueText.text                 = paragraph;
		NPCDialogueText.maxVisibleCharacters = maxVisibleChars;

		foreach (var c in paragraph.ToCharArray()) {
			maxVisibleChars++;
			NPCDialogueText.maxVisibleCharacters = maxVisibleChars;

			yield return new WaitForSeconds(MaxTypeTime / typeSpeed);
		}

		isTyping = false;
	}

	private void FinishParagraphEarly() {
		StopCoroutine(typewriterCoroutine);

		NPCDialogueText.maxVisibleCharacters = paragraph.Length;

		isTyping = false;
	}

	public bool GetIsInDialogue() {
		return isInDialogue;
	}
}