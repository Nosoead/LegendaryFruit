using UnityEngine;

public class MonsterStateMachine
{
    protected IMonster currentState { get; private set; }

    //public GameObject target { get; set; }

    public Monster monster{ get; private set; }
    public PatrollState patrollState{ get; private set; }
    public AttackState attackState{ get; private set; }
    public MonsterStateMachine(MonsterController monster)
    {
        this.patrollState = new PatrollState(this);
        this.attackState = new AttackState(this);
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
        Transform monsterTransform = monster.transform;
        Transform targetTransform = monster.Data.target.transform;
        var data = monster.Data;
        Vector2 direction = (targetTransform.position - monsterTransform.position).normalized;
        monsterTransform.position += (Vector3)(direction * (data.moveSpeed * Time.deltaTime));
    }

    public void Move()
    {
        Transform monsterTransfrom = monster.transform;
        float direction = Mathf.Sign(monsterTransfrom.localScale.x);
        Vector2 moveDirection = new Vector2(direction, 0);
            
        monsterTransfrom.position += (Vector3)(moveDirection * (monster.Data.moveSpeed * Time.deltaTime));
    }
    public void Excute()
    {
        if (currentState != null)
        {
            currentState?.Excute();
        }
    }
}
