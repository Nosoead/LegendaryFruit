using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShelterNPC : MonoBehaviour, IInteractable
{
    private int playerLayer;
    [SerializeField] private Canvas pressFCanvas;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        pressFCanvas = GetComponentInChildren<Canvas>();
    }

    private void Start()
    {
        pressFCanvas.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            TogglePrompt(isOpen: true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            TogglePrompt(isOpen: false);
        }
    }

    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed || isPressed)
        {
            UIManager.Instance.ToggleUI<ShelterNPCUI>(isPreviousWindowActive:false);
        }
    }

    private void TogglePrompt(bool isOpen)
    {
        if (isOpen)
        {
            pressFCanvas.gameObject.SetActive(isOpen);
        }
        else
        {
            pressFCanvas.gameObject.SetActive(isOpen);
        }
    }
}
