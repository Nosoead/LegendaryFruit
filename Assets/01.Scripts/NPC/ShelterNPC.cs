using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShelterNPC : NPC, IInteractable
{
    //F키 누르세요
    [SerializeField] private Canvas pressFCanvas;
    private int playerLayer;
    private int invincibleLayer;

    //private void Awake()
    //{
    //    playerLayer = LayerMask.NameToLayer("Player");
    //    invincibleLayer = LayerMask.NameToLayer("Invincible");
    //    pressFCanvas = GetComponentInChildren<Canvas>();
    //}

    //private void Start()
    //{
    //    pressFCanvas.gameObject.SetActive(false);
    //}

    public override void InitNPC()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        invincibleLayer = LayerMask.NameToLayer("Invincible");
        if (pressFCanvas == null)
        {
            pressFCanvas=GetComponentInChildren<Canvas>();
        }
        pressFCanvas.gameObject.SetActive(false);
    }

    #region /TogglePrompt
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer || collision.gameObject.layer == invincibleLayer)
        {
            TogglePrompt(isOpen: true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer || collision.gameObject.layer == invincibleLayer)
        {
            TogglePrompt(isOpen: false);
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
    #endregion

    public void Interact(bool isDeepPressed, bool isPressed)
    {
        if (isDeepPressed || isPressed)
        {
            UIManager.Instance.ToggleUI<ShelterNPCUI>(isPreviousWindowActive:false);
        }
    }

}
