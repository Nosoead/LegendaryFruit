public class NormalMonster : Monster
{  
    private MonsterSO monsterdata;
    private MonsterStateMachine stateMachine;
    private MonsterStat stat;
    private void Awake()
    {
        stat = new MonsterStat();
    }

    private void Start()
    {
        stat.InitStat(MonsterData);
        stateMachine.Initialize(stateMachine.patrollState);
        
    }
    private void Update()
    {
        stateMachine.Excute();
    }

}
