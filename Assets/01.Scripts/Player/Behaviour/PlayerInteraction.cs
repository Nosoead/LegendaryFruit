using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    //[SerializeField] private TextMeshProUGUI promptText;
    private LayerMask tapAndHoldMask;
    private LayerMask tapMask;
    private IInteractable currentInteractable;
    private bool canTapInteractWithObject;
    private bool CanHoldInteractWithObject;
    private bool isTapPressed;
    private bool isHoldPressed;
    private Coroutine coCheckHoldPressed;
    private float tapCompleteTime = 1f;
    private float holdCompleteTime = 2f;

    private void Awake()
    {
        EnsureComponents();
        tapMask = LayerMask.GetMask("NPC") | LayerMask.GetMask("Reward");
        tapAndHoldMask = LayerMask.GetMask("Item");
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        controller.OnTapInteractEvent += OnTapInteractEvent;
        controller.OnHoldInteractEvent += OnHoldInteractEvent;
    }

    private void OnDisable()
    {
        controller.OnTapInteractEvent -= OnTapInteractEvent;
        controller.OnHoldInteractEvent -= OnHoldInteractEvent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == tapMask)
        {
            canTapInteractWithObject = true;
            currentInteractable = collision.gameObject.GetComponent<IInteractable>();
        }
        else if (collision.gameObject.layer == tapAndHoldMask)
        {
            CanHoldInteractWithObject = true;
            ShowTapAndHoldPrompt();
            currentInteractable = collision.gameObject.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ResetInteractionChecker();
    }

    private void EnsureComponents()
    {
        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }
    }

    private void OnTapInteractEvent(bool isTapPressedSignal)
    {
        if (!canTapInteractWithObject)
        {
            return;
        }
        isTapPressed = isTapPressedSignal;
        if (canTapInteractWithObject && isTapPressed)
        {
            currentInteractable.Interact(isHoldPressed, isTapPressed);
            ResetInteractionChecker();
        }
    }

    private void OnHoldInteractEvent(bool isHoldPressedSignal)
    {
        if (!CanHoldInteractWithObject)
        {
            return; 
        }
        if (coCheckHoldPressed != null)
        {
            StopCoroutine(coCheckHoldPressed);
        }
        coCheckHoldPressed = StartCoroutine(CheckHoldPress(isHoldPressedSignal));

    }

    private IEnumerator CheckHoldPress(bool isHoldPressedSignal)
    {
        float pressedTime = 0f;
        
        while (isHoldPressedSignal)
        {
            pressedTime += Time.deltaTime;
            if (pressedTime >= 2f)
            {
                isHoldPressedSignal = false;
            }
            yield return null;
        }

        if (pressedTime > holdCompleteTime)
        {
            isHoldPressed = true;
            if (CanHoldInteractWithObject && isHoldPressed)
            {
                currentInteractable.Interact(isHoldPressed, isTapPressed);
                ResetInteractionChecker();
            }
        }

        if (pressedTime < tapCompleteTime)
        {
            isTapPressed = true;
            if (CanHoldInteractWithObject && isTapPressed)
            {
                currentInteractable.Interact(isHoldPressed, isTapPressed);
                ResetInteractionChecker();
            }
        }
    }

    private void ResetInteractionChecker()
    {
        isTapPressed = false;
        isHoldPressed = false;
        canTapInteractWithObject = false;
        CanHoldInteractWithObject = false;
        currentInteractable = null;
    }

    private void ShowTapAndHoldPrompt()
    {
        //TODO : 아이템 정보 및 이미지 내용 띄워주기
    }
}
