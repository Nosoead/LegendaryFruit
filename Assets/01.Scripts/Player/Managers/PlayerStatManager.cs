using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatManager : MonoBehaviour
{
    public UnityAction<string, float> OnSubscribeToStatUpdateEvent;
    public UnityAction<float, float, float> OnHealthDataToUIEvent;
    public event UnityAction<float, AttributeType> DamageTakenEvent;
    public event UnityAction<float> OnHealEvent;
    public UnityAction OnStopCoroutine;
    [SerializeField] private PlayerSO playerData;
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private PlayerAnimationController playerAnimationController;
    private List<WeaponSO> eatWeapons = new List<WeaponSO>();
    private PlayerStat stat;
    private StatHandler statHandler;

    [SerializeField] private MonoBehaviour damageableObject;
    private IDamageable damageable;
    private AttributeType currentTakeAttributeType;
    private PlayerInput input;

    private void Awake()
    {
        if (input == null)
        {
            input = GatherInputManager.Instance.input;
        }
        if (playerInteraction == null)
        {
            playerInteraction = GetComponent<PlayerInteraction>();
        }
        stat = new PlayerStat();
        statHandler = new StatHandler();
        playerData = Instantiate(playerData);
        damageable = damageableObject as IDamageable;
    }

    private void OnEnable()
    {
        playerInteraction.FruitWeaponEatAndStatUpEvent += OnIncreaseStat;
        playerInteraction.FruitWeaponEatAndStatUpEvent += OnRegisteConsumeItemData;
        stat.OnStatUpdatedEvent += OnStatUpdatedEvent;
        stat.OnHealthUpdateEvent += OnHealthUpdateEvent;
        stat.OnDie += OnDie;
        if(damageable is PlayerCondition condition)
        {
            condition.OnTakeHitType += OnAttributeTypeReceived;
        }
    }

    private void OnDisable()
    {
        playerInteraction.FruitWeaponEatAndStatUpEvent -= OnIncreaseStat;
        playerInteraction.FruitWeaponEatAndStatUpEvent -= OnRegisteConsumeItemData;
        stat.OnStatUpdatedEvent -= OnStatUpdatedEvent;
        stat.OnHealthUpdateEvent -= OnHealthUpdateEvent;
        stat.OnDie -= OnDie;
        if (damageable is PlayerCondition condition)
        {
            condition.OnTakeHitType -= OnAttributeTypeReceived;
        }
    }
    private void Start()
    {
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
        float result = statHandler.Add(stat.GetStatValue(statKey), eatValue);
        Debug.Log($"11key : {statKey}, currentValue : {eatValue}");
        stat.UpdateStat(statKey, result);
    }

    private void OnRegisteConsumeItemData(WeaponSO weaponData)
    {
        eatWeapons.Add(weaponData);
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
        input.Player.Disable();
        input.Changer.Disable();
    }

    private void OnAttributeTypeReceived(AttributeType type)
    {
        currentTakeAttributeType = type;
    }
    #endregion

    #region /ApplystatMethod
    public void ApplyInstantDamage(float damage)
    {
        float result = statHandler.Substract(stat.GetStatValue("CurrentHealth"), damage);
        DamageTakenEvent?.Invoke(damage, currentTakeAttributeType);
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
        ParticleManager.Instance.SetParticleTypeAndPlay(transform.position, ParticleType.Heal);
        //TODO 회복량 표시
        OnHealEvent?.Invoke(heal);  
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