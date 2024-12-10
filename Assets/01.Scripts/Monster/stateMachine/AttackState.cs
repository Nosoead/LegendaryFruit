using UnityEngine;

public class AttackState :IMonster
{
    private MonsterController monsterController;
    private AttributeLogicsDictionary attributeLogicsDictionary;
    public AttackState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
        InitializeAttributeLogicsDictionary();
    }

    private void InitializeAttributeLogicsDictionary()
    {
        attributeLogicsDictionary = new AttributeLogicsDictionary();
        attributeLogicsDictionary.Initialize();
    }
    public void Enter()
    {
        Debug.Log("AttackState Enter");
    }

    public void Excute()
    {
        float distanceToTarget = Vector2.Distance(monsterController. transform.position,
            monsterController. target.transform.position);
        // currentDistance <AttackDistance-->때리기
        if (monsterController.InAttackRange())
        {
            Debug.Log("공격!!");
            OnHit();
            // if-> 때리는 애니메이션 ->애니메이션에서 OnHit
            return;
        }
        // player 놓치면 idleState 
        if (!monsterController.DetectPlayer())
        {
            monsterController.StateMachine.TransitionToState(monsterController.StateMachine.idleState);
        }
        // player 놓치면 idleState 
        if (monsterController.DetectPlayer())
        {
            monsterController.MoveToTarget();
            Debug.Log("돌진!!");
        }
    }

    public void Exit()
    {
        Debug.Log("AttackState Exit");
    }
    public void OnHit()
   {
       //어트리뷰트에서 데미지계산후 딕셔너리에 저장후 꺼내옴
       attributeLogicsDictionary.GetAttributeLogic(monsterController.Monster.Data.type);
       Debug.Log($"{monsterController}");
       Debug.Log($"Hit, type : {monsterController.Monster.Data.type}");
       //애니메이션
   }
}
