using UnityEngine.Pool;

public class TestHowToUseObjectPoolManager : Singleton<TestHowToUseObjectPoolManager>
{
    private IObjectPool<PooledMonster> monster;
    private IObjectPool<PooledReward> Reward;
    // private PooledObject<PooledMonster> monsterObject;
    private void Start()
    {
        PoolManager.Instance.CreatePool<PooledMonster>(PoolType.PooledMonster, false, 7, 12);
        PoolManager.Instance.CreatePool<PooledReward>(PoolType.PooledReward, false, 5, 10);
        monster = PoolManager.Instance.poolDictionary[PoolType.PooledMonster] as IObjectPool<PooledMonster>;
        Reward = PoolManager.Instance.poolDictionary[PoolType.PooledReward] as IObjectPool<PooledReward>;
    }

    public void GetMon()
    {
        if (monster.Get() is PooledMonster mon)
        {
           // mon.Release3sec();
        }
    }

    public void GetReward()
    {
        if (Reward.Get() is PooledReward re)
        {
            re.Release3sec();
        }
    }
}