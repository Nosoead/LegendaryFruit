using UnityEngine;

public class MonsterStateMachine
{
    protected IMonster currentState { get; private set; }
    public MonsterController monster{ get; private set; }
    public IdleState idleState { get; private set; }
    public PatrollState patrollState{ get; private set; }
    public AttackState attackState{ get; private set; }
    
    public MonsterStateMachine(MonsterController monster)
    {
        this.monster = monster;
        this.idleState = new IdleState(monster);
        this.patrollState = new PatrollState(monster);
        this.attackState = new AttackState(monster);
    }



    public void Initialize(IMonster monsterState) // 초기화
    {
        currentState = monsterState;
    }

    public void TransitionToState(IMonster nextState)
    {
        currentState?.Exit();
        currentState = nextState; // 다음 스테이트로
        currentState?.Enter();
    }

    public void MoveTowardsTarget()
    {
        var data = monster.Monster.Data;
        Transform monsterTransform = monster.transform;
        Transform targetTransform = data.target.transform;//monster.Data.target.transform;
        
        Vector2 direction = (targetTransform.position - monsterTransform.position).normalized;
        monsterTransform.position += (Vector3)(direction * (data.moveSpeed * Time.deltaTime));
    }

    public void Move()
    {
        Transform monsterTransfrom = monster.transform;
        float direction = Mathf.Sign(monsterTransfrom.localScale.x);
        Vector2 moveDirection = new Vector2(direction, 0);
            
        monsterTransfrom.position += (Vector3)(moveDirection * (monster.Monster.Data.moveSpeed * Time.deltaTime));
    }
    public void Excute()
    {
        if (currentState != null)
        {
            currentState?.Excute();
        }
    }
    public void ReverseDirection()
    {
        // 몬스터의 x축 방향을 반전
        Vector3 scale = monster.transform.localScale;
        scale.x *= -1; 
        monster.transform.localScale = scale;
    }
}
