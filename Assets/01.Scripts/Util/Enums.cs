public enum AttributeType
{
    Burn,
    SlowDown,
    Knockback,
    Poison,
    Blindness,
    Normal
}

public enum StatType
{
    MaxHealth,
    MoveSpeed,
    AttackPower,
    AttackSpeed,
    Defense,
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
    Stage11 = 11021,
    Stage21 = 11031,
    StageShelter = 11041,
    StageBoss = 11051,
    Stage12 = 21021,
    Stage22 = 21031
}

public enum PoolType
{
    PooledReward,
    PooledMonster,
    PooledSound,
    PooledEffect,
    PooledFruitWeapon,
    PooledPotion,
    PooledCurrency,
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

public enum ItemType
{
    Weapon,
    Potion,
    Currency
}

public enum SkillType
{
    Count = 1,
    Grade
}