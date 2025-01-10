
public interface IInteractable
{
    void Interact(bool isDeepPressed, bool isPressed);
}

public interface IDamageable
{
    void TakeDamage(float damage);
    void BurnDamage(float damage, float attributeValue, float attributeRateTime, float attributeStack);
    void SlowDown(float damage, float attributeValue, float attributeRateTime);
    void Knockback(float damage, float attributeValue, float lookDirection);
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