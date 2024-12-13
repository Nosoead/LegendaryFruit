using System.ComponentModel;

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

public interface IMonster
{
    void Enter();
    void Excute();
    void Exit();
}
