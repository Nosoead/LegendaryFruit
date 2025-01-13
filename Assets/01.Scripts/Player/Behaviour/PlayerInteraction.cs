using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteraction : MonoBehaviour
{
    public UnityAction<WeaponSO> FruitWeapOnEquipEvent;
    public UnityAction<WeaponSO> FruitWeaponEatAndStatUpEvent;
    public UnityAction<WeaponSO, Vector3, bool> ShowPromptEvent;
    public UnityAction<float> ShowFillamountInPromptEvent;
    [SerializeField] private PlayerController controller;
    private IInteractable currentInteractable;
    private WeaponSO weaponData;

    //Layer
    private int TalkNpcLayer;
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
        TalkNpcLayer = LayerMask.NameToLayer("TalkableNPC");
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
        if (collision.gameObject.layer == NPCLayer || collision.gameObject.layer == TalkNpcLayer || 
            collision.gameObject.layer == rewardLayer)
        {
            canTapInteractWithObject = true;
            currentInteractable = collision.gameObject.GetComponent<IInteractable>();
        }
        else if (collision.gameObject.layer == itemLayer)
        {
            CanHoldInteractWithObject = true;
            currentInteractable = collision.gameObject.GetComponent<IInteractable>();
            if (collision.gameObject.TryGetComponent(out PooledFruitWeapon fruitWeapon))
            {
                weaponData = fruitWeapon.GetWeaponSO();
                ShowTapAndHoldPrompt(isOpen: true);
                GiveFillamountData(holdCompleteTime);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ShowTapAndHoldPrompt(isOpen: false);
        GiveFillamountData(holdCompleteTime);
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
            GiveFillamountData(pressedTime);
            yield return null;
        }

        if (isHoldPressed)
        {
            if (CanHoldInteractWithObject && isHoldPressed)
            {
                FruitWeaponEatAndStatUpEvent?.Invoke(weaponData);
                currentInteractable.Interact(isHoldPressed, isTapPressed);
                ShowTapAndHoldPrompt(isOpen: false);
                ResetInteractionChecker();
            }
        }
        else if (isTapPressed)
        {
            if (CanHoldInteractWithObject && isTapPressed)
            {
                FruitWeapOnEquipEvent?.Invoke(weaponData);
                currentInteractable.Interact(isHoldPressed, isTapPressed);
                ShowTapAndHoldPrompt(isOpen: false);
                ResetInteractionChecker();
            }
        }
        else
        {
            Debug.Log("1초 이상 2초 미만에 취소함");
            isTapPressed = false;
            isHoldPressed = false;
        }
        GiveFillamountData(holdCompleteTime);
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
        if (weaponData != null)
        {
            Vector3 promptPosition = SetPromptPosition();
            ShowPromptEvent?.Invoke(weaponData, promptPosition, isOpen);
        }
        else
        {
            return;
        }
    }

    private Vector3 SetPromptPosition()
    {
        Vector3 inGamePosition = new Vector3(transform.position.x+2.5f,transform.position.y+4f,transform.position.z);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(inGamePosition);
        return screenPosition;
    }

    private void GiveFillamountData(float progressValue)
    {
        float result = progressValue / holdCompleteTime;
        ShowFillamountInPromptEvent?.Invoke(result);
    }
}