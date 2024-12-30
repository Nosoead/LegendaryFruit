public enum AttributeType
{
    Burn,
    SlowDown,
    KnockBack,
    Poison,
    Blindness,
    Normal
}

enum StatType
{
    MaxHealth,
    MoveSpeed,
    Defense,
    AttackSpeed,
    AttackPower,
    Heal
}

public enum GradeType
{
    Normal,
    Rare,
    Epic,
    Legendary
}

public enum StageType
{
    Stage0 = 11001,
    Stage1 = 11011,
    Stage2 = 11021,
    StageShelter = 11031,
    StageBoss = 11041,
}

public enum PoolType
{
    PooledReward,
    PooledMonster,
    PooledSound,
    PooledEffect,
    PooledFruitWeapon,
    Count
}

public enum SceneType
{
    TitleScene = 0,
    OneCycleScene = 1
}
public enum SfxType
{
    UIButton = 0,

    PlayerAttack1 = 10,
    PlayerAttack2,
    PlayerJump,
    PlayerDash,
    PlayerMove,
    PlayerDamaged,
    PlayerDeath,

    MonsterAttack = 20,
    MonsterDamaged1,
    MonsterDamaged2,
    MonsterDeath,
    End
}
public enum BgmType
{
    Title = 0,      //타이틀
    InGame,         //인게임
    Boss,

}
enum Table
{
    Rule,
    Character,
    Stage,
    Monster
}