using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
  [SerializeField] private UIDialogue uidialogue;
  [SerializeField] private int dialogueIndex;

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      var dialogue = DialogueManager.Instance.GetDialogueData(dialogueIndex);

      Debug.Log("트리거작동");
      if (dialogue != null)
      {
        uidialogue.gameObject.SetActive(true);
        uidialogue.SetDialogue(dialogue);
      }
    }
  }
}
