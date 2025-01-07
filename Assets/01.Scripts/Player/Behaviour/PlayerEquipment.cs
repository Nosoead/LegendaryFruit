using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class PlayerEquipment : MonoBehaviour
{
    //TODO 최대 2개 보관 *리겜할때, 배열 청소 및 현재인덱스 0으로 초기화
    public UnityAction<WeaponSO> OnEquipWeaponChanged;
    [SerializeField] private WeaponSO weaponData;
    //[SerializeField] private PooledFruitWeapon weaponPrefab; //TODO ObjectPool에서 최대 6개 생성 및 관리
    [SerializeField] private PlayerInteraction interaction;
    [SerializeField] private SpriteRenderer weaponSprite;
    [SerializeField] private PlayerController controller;
    private IObjectPool<PooledFruitWeapon> fruitWeapon;
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
        PoolManager.Instance.CreatePool<PooledFruitWeapon>(PoolType.PooledFruitWeapon, false, 5, 5);
        Init();
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

    private void Init()
    {
        CacheItem();
        if (!DataManager.Instance.GetCanLoad<SaveDataContainer>())
        {
            OnWeaponEquipEvent(weaponData);//Attack에서 OnEnable구독해서 이렇게 함.
        }
    }

    private void CacheItem()
    {
        fruitWeapon = PoolManager.Instance.GetObjectFromPool<PooledFruitWeapon>(PoolType.PooledFruitWeapon);
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
        //var discardedObject = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        PooledFruitWeapon discardedObject = fruitWeapon.Get();
        discardedObject.gameObject.transform.position = transform.parent.position; //dotween으로 플레이어 콜라이더 벗어나게하고 돌아오도록 세팅
        DropDotween(discardedObject.gameObject, transform.parent.position);
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

    #region /DropDotween
    private void DropDotween(GameObject go, Vector3 position)
    {
        RaycastHit2D blockHit = Physics2D.Raycast(go.transform.position, Vector2.down, 50, LayerMask.GetMask("Block"));
        RaycastHit2D groundHit = Physics2D.Raycast(go.transform.position, Vector2.down, 50, LayerMask.GetMask("Ground"));
        RaycastHit2D closestHit;

        //가까운 히트
        if (blockHit.collider != null && groundHit.collider != null)
        {
            closestHit = blockHit.distance < groundHit.distance ? blockHit : groundHit;
        }
        else if (blockHit.collider != null)
        {
            closestHit = blockHit;
        }
        else if (groundHit.collider != null)
        {
            closestHit = groundHit;
        }
        else
        {
            return;
        }
        float hitPosY = closestHit.point.y;
        float goalPos = hitPosY + 0.5f;
        if (closestHit.collider != null)
        {
            // 무기가 떨궈짐
            Sequence sequence = DOTween.Sequence();
            sequence.Append(go.transform.DOMoveY(position.y+3.5f, 0.5f, false))
                    .Append(go.transform.DOMoveY(goalPos, 0.5f, false));
            
        }
    }
    #endregion
}
