using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueList : UIBase
{
    [SerializeField] private UIDialogue uiDialogue;
    [SerializeField] private ScrollRect scr;
    [SerializeField] private Image viewport;
    [SerializeField] private Image root;
    [SerializeField] private Button btnBase;

    private bool isInit = false;
    
    private void Update()
    {
        if (!isInit &&  DialogueManager.Instance.IsLoad)
        {
            isInit = true;
            LoadDialogueList();
        }
    }

    private void LoadDialogueList()
    {
        var eDialogueList = DialogueManager.Instance.GetDialogueList();
        while (eDialogueList.MoveNext())
        {
            var dList = eDialogueList.Current.Value;
            
            var btn = Instantiate(btnBase, root.transform);
            btn.GetComponentInChildren<TMP_Text>().text = dList.Title;
            
            btn.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                var dialogue =  DialogueManager.Instance.GetDialogueData(dList.StartDialogue);
                uiDialogue.SetDialogue(dialogue);
            });

            btn.gameObject.SetActive(true);
        }
    }
}