public enum AttributeType
{
    Burn,
    SlowDown,
    KnockBack,
    Poison,
    Blindness,
    Normal
}

public enum StatType
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
    Unique,
    Legendary
}

public enum StageType
{
    StageLobby = 11001,
    Stage0 = 11011,
    Stage1 = 11021,
    Stage2 = 11031,
    StageShelter = 11041,
    StageBoss = 11051,
}

public enum PoolType
{
    PooledReward,
    PooledMonster,
    PooledSound,
    PooledEffect,
    PooledFruitWeapon,
    PooledBossMonster,
    PooledParticle,
    PooledProjectile,
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

    MonsterAttack= 20,
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
    InfiniteDoors
}

public enum ProjectileType
{
    Player,
    Monster
}

public enum ParticleType
{
    Heal,
    BurnDamage,
    NormalDamage,
    SlowDownDamage,
    KnockBackDamage
}