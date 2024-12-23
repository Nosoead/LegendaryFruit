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