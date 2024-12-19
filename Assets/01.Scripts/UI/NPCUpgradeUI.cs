using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCUpgradeUI : UIBase
{
    [SerializeField] private UIDialogue uiDialogue;
    private int dialogueIndex;
    private bool isDialogueOpen;
    public override void Open()
    {
        dialogueIndex = Random.Range(10007, 10010);
        if (uiDialogue == null) uiDialogue = FindObjectOfType<UIDialogue>(true);
        uiDialogue.gameObject.SetActive(true);
        var dialogue = DialogueManager.Instance.GetDialogueData(dialogueIndex);
        uiDialogue.SetDialogue(dialogue);
        //gameObject.SetActive(false);
    }
}
