using UnityEngine;
using UnityEngine.UI;

public class MonsterHPUI : UIBase
{
  [SerializeField] private GameObject monsterHPUI;
  [SerializeField] private MonsterStatManager monsterStatManager;
  [SerializeField] private Image healthBar;

  private void Awake()
  {
    if (monsterStatManager == null)
    {
      monsterStatManager = GetComponentInParent<MonsterStatManager>();
    }
    if (monsterHPUI == null)
    {
      monsterHPUI = GetComponentInChildren<GameObject>();
    }
    monsterHPUI.SetActive(false);
  }

  private void OnEnable()
  {
    monsterStatManager.OnShowHealthBarEvent += OnShowHealthbar;
  }

  private void OnDisable()
  {
    monsterStatManager.OnShowHealthBarEvent -= OnShowHealthbar;
  }
  private void OnShowHealthbar(float healthData)
  {
    if (!monsterHPUI.activeSelf)
    {
      monsterHPUI.SetActive(true);
    }
    healthBar.fillAmount = healthData;
  }
}
