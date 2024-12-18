using UnityEngine.Pool;

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

//public interface ISetPooledObject<T> where T : Component
//{
//    void SetPooledObject(IObjectPool<T> pool);
//}