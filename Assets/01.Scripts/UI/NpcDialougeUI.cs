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
            uiDialogue.gameObject.SetActive(true);
            SetDialogue();
            Invoke(nameof(SetDialogue), 0.1f);
            isFirstOpen = true;
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
        if (dialogue != null)
        {
            uiDialogue.SetDialogue(dialogue); 
        }
    }
   
    public void GetDialougeIndex(int index)
    {
        dialogueIndex = index;
    }
}