using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
  [SerializeField] private UIDialogue uiDialogue;
  private int dialogueIndex = 10001;

  private bool istrigger;
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player") && !istrigger)
    {
      if (uiDialogue == null) uiDialogue = FindObjectOfType<UIDialogue>(true);
      uiDialogue.gameObject.SetActive(true);
      var dialogue = DialogueManager.Instance.GetDialogueData(dialogueIndex);
      uiDialogue.SetDialogue(dialogue);
      istrigger = true;
    }
  }
}


