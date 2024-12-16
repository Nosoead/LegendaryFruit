using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteraction : MonoBehaviour
{
    public UnityAction<WeaponSO> FruitWeapOnEquipEvent;
    public UnityAction<string, float> FruitWeaponEatAndStatUpEvent; //먹은 리스트도 저장가능~
    [SerializeField] private PlayerController controller;
    //[SerializeField] private TextMeshProUGUI promptText;
    private IInteractable currentInteractable;
    private WeaponSO weaponData;

    //Layer
    private int NPCLayer;
    private int rewardLayer;
    private int itemLayer;
    
    //Tap or Hold Check
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
        NPCLayer = LayerMask.NameToLayer("NPC");
        rewardLayer = LayerMask.NameToLayer("Reward");
        itemLayer = LayerMask.NameToLayer("Item");
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
        if (collision.gameObject.layer == NPCLayer || collision.gameObject.layer == rewardLayer)
        {
            canTapInteractWithObject = true;
            currentInteractable = collision.gameObject.GetComponent<IInteractable>();
        }
        else if (collision.gameObject.layer == itemLayer)
        {
            CanHoldInteractWithObject = true;
            ShowTapAndHoldPrompt(isOpen : true);
            currentInteractable = collision.gameObject.GetComponent<IInteractable>();
            if (collision.gameObject.TryGetComponent(out FruitWeapon fruitWeapon))
            {
                weaponData = fruitWeapon.weaponData;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ResetInteractionChecker();
        ShowTapAndHoldPrompt(isOpen: false);
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
        Debug.Log(isTapPressed);
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
            if (pressedTime >= holdCompleteTime)
            {
                isHoldPressed = true;
                break;
            }
            else if (pressedTime > tapCompleteTime)
            {
                isTapPressed = false;
            }
            else if (pressedTime == tapCompleteTime) { Debug.Log("1초 경과"); }
            else
            {
                isTapPressed = true;
            }
            yield return null;
        }

        if (isHoldPressed)
        {
            if (CanHoldInteractWithObject && isHoldPressed)
            {
                FruitWeaponEatAndStatUpEvent?.Invoke(((StatType)((int)weaponData.type)).ToString(), weaponData.eatValue);
                currentInteractable.Interact(isHoldPressed, isTapPressed);
                ResetInteractionChecker();
                ShowTapAndHoldPrompt(isOpen : false);
            }
        }
        else if (isTapPressed)
        {
            if (CanHoldInteractWithObject && isTapPressed)
            {
                FruitWeapOnEquipEvent?.Invoke(weaponData);
                currentInteractable.Interact(isHoldPressed, isTapPressed);
                ResetInteractionChecker();
                ShowTapAndHoldPrompt(isOpen: false);
            }
        }
        else
        {
            Debug.Log("1초 이상 2초 미만에 취소함");
            isTapPressed = false;
            isHoldPressed = false;
        }
    }

    private void ResetInteractionChecker()
    {
        isTapPressed = false;
        isHoldPressed = false;
        canTapInteractWithObject = false;
        CanHoldInteractWithObject = false;
        currentInteractable = null;
        weaponData = null;
    }

    private void ShowTapAndHoldPrompt(bool isOpen)
    {
        //TODO : 아이템 정보 SO받아서 이미지 내용 띄워주기
    }
}