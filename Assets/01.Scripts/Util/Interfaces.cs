
public interface IInteractable
{
    void Interact(bool isDeepPressed, bool isPressed);
}

public interface IDamageable
{
    void TakeDamage(float damage);
    void BurnDamage(float damage, float attributeValue, float attributeRateTime, float attributeStack);
    void SlowDown(float damage, float attributeValue, float attributeRateTime);
}

public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}

public interface IResetPooledObject
{
    void Reset();
}

public interface IPersistentUI
{

}

public interface IProjectTileShooter
{
    void Shoot(PooledProjectTile projectTile);
}