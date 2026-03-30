using UnityEngine;

public class Mannekin : NPC, ITalkable
{
    [SerializeField] DialogueText dialogueText;
    [SerializeField] DialogueController dialogueController;
    public override void Interact()
    {
        Talk(dialogueText);
    }

    public void Talk(DialogueText dialogueText)
    {
        dialogueController.DisplayNextParagraph(dialogueText);
    }
}
