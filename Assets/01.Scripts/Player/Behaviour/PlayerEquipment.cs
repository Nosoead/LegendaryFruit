using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEquipment : MonoBehaviour
{
    //TODO 최대 2개 보관 *리겜할때, 배열 청소 및 현재인덱스 0으로 초기화
    public UnityAction<WeaponSO> OnEquipWeaponChanged;
    [SerializeField] private WeaponSO weaponData;
    [SerializeField] private PooledFruitWeapon weaponPrefab; //TODO ObjectPool에서 최대 6개 생성 및 관리
    [SerializeField] private PlayerInteraction interaction;
    [SerializeField] private SpriteRenderer weaponSprite;
    [SerializeField] private PlayerController controller;
    private WeaponSO startingWeaponData;

    private List<WeaponSO> equipWeapons = new List<WeaponSO>();
    private int maxWeaponCapacity = 2;
    private int currentEquipWeaponIndex = 0;

    private void Awake()
    {
        EnsureComponents();
    }

    private void OnEnable()
    {
        controller.OnSwapWeaponEvent += OnSwapWeaponEvent;
        interaction.FruitWeapOnEquipEvent += OnWeaponEquipEvent;
    }

    private void OnDisable()
    {
        controller.OnSwapWeaponEvent -= OnSwapWeaponEvent;
        interaction.FruitWeapOnEquipEvent -= OnWeaponEquipEvent;
    }

    private void Start()
    {
        OnWeaponEquipEvent(weaponData);//Attack에서 OnEnable구독해서 이렇게 함.
    }

    private void EnsureComponents()
    {
        if (interaction == null)
        {
            interaction = GetComponentInParent<PlayerInteraction>();
        }
        if (weaponSprite == null)
        {
            weaponSprite = GetComponentInChildren<SpriteRenderer>();
        }
        if (controller == null)
        {
            controller = GetComponentInParent<PlayerController>();
        }
    }

    private void OnSwapWeaponEvent()
    {
        if (equipWeapons.Count < 2)
        {
            currentEquipWeaponIndex = 0;
            return;
        }
        currentEquipWeaponIndex = (currentEquipWeaponIndex + 1) % equipWeapons.Count;
        UpdateWeaponSprite();
    }

    private void OnWeaponEquipEvent(WeaponSO weaponData)
    {

        if (equipWeapons.Count < maxWeaponCapacity)
        {
            equipWeapons.Add(weaponData);
            currentEquipWeaponIndex = equipWeapons.Count-1;
        }
        else
        {
            ReplaceWeapon(weaponData);
        }
        UpdateWeaponSprite();
    }

    private void ReplaceWeapon(WeaponSO weaponData)
    {
        //TODO : 오브젝트풀 들고와서 SO랑 스프라이트 덮어씌우기
        //매번 여기서 참조풀려서 경고 뜰 것. -> stageManager에서 풀링 후 껍데기 들고와서 씌우기
        var discardedObject = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        discardedObject.weaponData = equipWeapons[currentEquipWeaponIndex];
        discardedObject.EnsureComponents();
        equipWeapons[currentEquipWeaponIndex] = weaponData;
    }

    private void UpdateWeaponSprite()
    {
        weaponSprite.sprite = equipWeapons[currentEquipWeaponIndex].weaponSprite;
        OnEquipWeaponChanged?.Invoke(equipWeapons[currentEquipWeaponIndex]);
    }

    public (List<WeaponSO>, int) SaveEquipmentData()
    {
        return (equipWeapons, currentEquipWeaponIndex);
    }

    public void LoadEquipmentData(List<WeaponSO> weaponDataList, int currentEquipWeaponIndex)
    {
        equipWeapons = weaponDataList;
        this.currentEquipWeaponIndex = currentEquipWeaponIndex;
        UpdateWeaponSprite();
    }

    public void DeleteEquipmentData()
    {
        equipWeapons.Clear();
        currentEquipWeaponIndex = 0;
    }
}
