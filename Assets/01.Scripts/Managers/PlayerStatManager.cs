using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatManager : MonoBehaviour
{
    public UnityAction<string, float> OnSubscribeToStatUpdateEvent;
    public UnityAction<float, float, float> OnHealthDataToUIEvent;
    public UnityAction OnStopCoroutine;
    [SerializeField] private PlayerSO playerData;
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private PlayerAnimationController playerAnimationController;
    private List<WeaponSO> eatWeapons = new List<WeaponSO>();
    private PlayerStat stat;
    private StatHandler statHandler;

    private void Awake()
    {
        if (playerInteraction == null)
        {
            playerInteraction = GetComponent<PlayerInteraction>();
        }
        stat = new PlayerStat();
        statHandler = new StatHandler();
        playerData = Instantiate(playerData);
    }

    private void OnEnable()
    {
        playerInteraction.FruitWeaponEatAndStatUpEvent += OnIncreaseStat;
        playerInteraction.FruitWeaponEatAndStatUpEvent += OnRegisteConsumeItemData;
        stat.OnStatUpdatedEvent += OnStatUpdatedEvent;
        stat.OnHealthUpdateEvent += OnHealthUpdateEvent;
        stat.OnDie += OnDie;
    }

    private void OnDisable()
    {
        playerInteraction.FruitWeaponEatAndStatUpEvent -= OnIncreaseStat;
        playerInteraction.FruitWeaponEatAndStatUpEvent -= OnRegisteConsumeItemData;
        stat.OnStatUpdatedEvent -= OnStatUpdatedEvent;
        stat.OnHealthUpdateEvent -= OnHealthUpdateEvent;
        stat.OnDie -= OnDie;
    }
    private void Start()
    {
        // TODO: SaveManager를 통해 LoadData로 데이터 로드 시,
        //       Load 결과가 null인 경우 초기화 처리 추가
        Init();
    }

    public void Init()
    {
        stat.InitStat(playerData);
    }

    #region /subscribeMethod
    private void OnIncreaseStat(WeaponSO weaponData)
    {
        string statKey = ((StatType)((int)weaponData.type)).ToString();
        float eatValue = weaponData.eatValue;
        //Debug.Log("벨류 업 : " + eatValue);
        float result = statHandler.Add(stat.GetStatValue(statKey), eatValue);
        stat.UpdateStat(statKey, result);
        //Debug.Log("먹고난 후 : " + result);
    }

    private void OnRegisteConsumeItemData(WeaponSO weaponData)
    {
        eatWeapons.Add(weaponData);
        Debug.Log(eatWeapons.Count);
    }

    private void OnStatUpdatedEvent(string key, float value)
    {
        OnSubscribeToStatUpdateEvent?.Invoke(key, value);
    }

    private void OnHealthUpdateEvent(float currentHealth, float maxHealth)
    {
        float healthFillAmount = currentHealth/maxHealth;
        OnHealthDataToUIEvent?.Invoke(healthFillAmount, currentHealth, maxHealth);
    }

    private void OnDie()
    {
        playerAnimationController.OnDie();
        GameManager.Instance.GameEnd();
        //OnStopCoroutine?.Invoke();
    }

    #endregion

    #region /ApplystatMethod
    public void ApplyInstantDamage(float damage)
    {
        float result = statHandler.Substract(stat.GetStatValue("CurrentHealth"), damage);
        playerAnimationController.OnHit();
        stat.UpdateCurrentHealth(result);
    }

    public void ApplyTemporaryStatReduction(float attributeValue, string statKey)
    {
        float result = statHandler.Substract(stat.GetStatValue(statKey), attributeValue);
        stat.UpdateStat(statKey, result);
    }

    public void ApplyRestoreStat(float attributeValue, string statKey)
    {
        float result = statHandler.Add(stat.GetStatValue(statKey), attributeValue);
        stat.UpdateStat(statKey, result);
    }

    public void Heal(float heal)
    {
        float result = statHandler.Add(stat.GetStatValue("CurrentHealth"), heal, stat.GetStatValue("MaxHealth"));
        stat.UpdateCurrentHealth(result);
    }
    #endregion

    #region /Data Method
    public PlayerStatData SaveStatManagerData()
    {
        return stat.SavePlayerData(playerData);
    }

    public void LoadStatManagerData(PlayerStatData playerStatData)
    {
        stat.LoadPlayerData(playerData, playerStatData);
    }

    public void DeleteStatManagerData()
    {
        stat.DeletePlayerData();
    }

    public List<WeaponSO> SaveConsumeData()
    {
        return eatWeapons;
    }

    public void LoadConsumeData(List<WeaponSO> weaponDataList)
    {
        eatWeapons = weaponDataList;
    }

    public void DeleteConsumeData()
    {
        eatWeapons.Clear();
    }
    #endregion
}