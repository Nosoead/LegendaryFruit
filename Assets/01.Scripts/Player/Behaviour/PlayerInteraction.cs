using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteraction : MonoBehaviour
{
    public UnityAction<WeaponSO> FruitWeapOnEquipEvent;
    public UnityAction<WeaponSO> FruitWeaponEatAndStatUpEvent;
    public UnityAction<ItemSO, Vector3, bool> ShowPromptEvent;
    public UnityAction<float> ShowFillamountInPromptEvent;
    [SerializeField] private PlayerController controller;
    private IInteractable currentInteractable;
    private WeaponSO weaponData;
    private PotionSO potionData;
    private CurrencySO currencyData;

    //Layer
    private int consumableItemLayer;
    private int talkNpcLayer;
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
        consumableItemLayer = LayerMask.NameToLayer("ConsumableItem");
        talkNpcLayer = LayerMask.NameToLayer("TalkableNPC");
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
        if (collision.gameObject.layer == NPCLayer || collision.gameObject.layer == talkNpcLayer ||
            collision.gameObject.layer == rewardLayer || collision.gameObject.layer == consumableItemLayer)
        {
            canTapInteractWithObject = true;
            currentInteractable = collision.gameObject.GetComponent<IInteractable>();
            if (collision.gameObject.layer == consumableItemLayer)
            {
                if (collision.gameObject.TryGetComponent(out PooledPotion potion))
                {
                    potionData = potion.GetPotionSO();
                }
                else if (collision.gameObject.TryGetComponent(out PooledCurrency currency))
                {
                    currencyData = currency.GetCurrencySO();
                }
                ShowTapAndHoldPrompt(isOpen: true);
            }
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
            else if (pressedTime == tapCompleteTime)
            {
            }
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
        potionData = null;
        currencyData = null;
    }

    private void ShowTapAndHoldPrompt(bool isOpen)
    {
        Vector3 promptPosition;
        if (isOpen)
        {
            promptPosition = SetPromptPosition();
        }
        else
        {
            promptPosition = Vector3.zero;
        }
        if (weaponData != null)
        {
            ShowPromptEvent?.Invoke(weaponData, promptPosition, isOpen);
        }
        else if (potionData != null)
        {
            ShowPromptEvent?.Invoke(potionData, promptPosition, isOpen);
        }
        else if (currencyData != null)
        {
            ShowPromptEvent?.Invoke(currencyData, promptPosition, isOpen);
        }
        else
        {
            return;
        }
    }

    private Vector3 SetPromptPosition()
    {
        Vector3 inGamePosition =
            new Vector3(transform.position.x + 2.5f, transform.position.y + 4f, transform.position.z);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(inGamePosition);
        return screenPosition;
    }

    private void GiveFillamountData(float progressValue)
    {
        float result = progressValue / holdCompleteTime;
        ShowFillamountInPromptEvent?.Invoke(result);
    }
}