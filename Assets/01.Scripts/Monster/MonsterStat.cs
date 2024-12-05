
public class MonsterStat : Stat
{

    public float CurrentHealth {get; set; }
    public float MoveSpeed{get; set; }
    public override void InitStat(GameSO gameData)
    {
        if(gameData is MonsterSO monsterData)
        {
            CurrentHealth = monsterData.maxHealth;
            MoveSpeed = monsterData.moveSpeed;
        }
    }
}
