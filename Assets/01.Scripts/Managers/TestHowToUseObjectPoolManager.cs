using UnityEngine.Pool;

public class TestHowToUseObjectPoolManager : Singleton<TestHowToUseObjectPoolManager>
{
    private IObjectPool<PooledMonster> monster;
    private IObjectPool<testPooledReward> Reward;
    // private PooledObject<PooledMonster> monsterObject;
    private void Start()
    {
        PoolManager.Instance.CreatePool<PooledMonster>(PoolType.PooledMonster, false, 7, 12);
        PoolManager.Instance.CreatePool<testPooledReward>(PoolType.PooledReward, false, 5, 10);
        monster = PoolManager.Instance.poolDictionary[PoolType.PooledMonster] as IObjectPool<PooledMonster>;
        Reward = PoolManager.Instance.poolDictionary[PoolType.PooledReward] as IObjectPool<testPooledReward>;
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
        if (Reward.Get() is testPooledReward re)
        {
            re.Release3sec();
        }
    }
}