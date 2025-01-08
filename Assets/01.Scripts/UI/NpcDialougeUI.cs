using System.Collections;
using UnityEngine;

public class NpcDialougeUI : UIBase
{
    [SerializeField] private UIDialogue uiDialoguePrefab;
    private UIDialogue uiDialogue;
    private int dialogueIndex;
    private bool isFirstOpen;

    public override void Open()
    {
        if (!isFirstOpen)
        {

            Init();
            isFirstOpen = true;
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            StartCoroutine(LoadDialogueData(dialogueIndex));
            
        }
        else
        {
            uiDialogue.gameObject.SetActive(true);
            SetDialogue();
        }
    }
    private void Init()
    {
        if (!uiDialogue)
        {
            GameObject dialogueObject = Instantiate(uiDialoguePrefab.gameObject);
            uiDialogue = dialogueObject.GetComponent<UIDialogue>();

            //DialogueManager.Instance.SetUIDialogue(uiDialogue);
        }

    }

    private void SetDialogue()
    {
        var dialogue = DialogueManager.Instance.GetDialogueData(dialogueIndex);
        if (dialogue == null) return;
        uiDialogue.SetDialogue(dialogue);
    }
    private IEnumerator LoadDialogueData(int index)
    {
        var dialogue = DialogueManager.Instance.GetDialogueData(index);
        
        while (dialogue == null)
        {
            yield return null; 
            dialogue = DialogueManager.Instance.GetDialogueData(index);
        }

        uiDialogue.SetDialogue(dialogue);
        gameObject.SetActive(false);
    }
    public void GetDialougeIndex(int index)
    {
        dialogueIndex = index;
    }
}