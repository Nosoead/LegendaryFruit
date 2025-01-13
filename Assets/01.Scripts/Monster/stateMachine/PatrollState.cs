using UnityEngine;

public class PatrollState : IState
{
    private MonsterController monsterController;
    private float idleTime;
    private float idleTimer = 0f;
    private bool isGround;

    public PatrollState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }

    public void Enter()
    {
        monsterController.animationController.OnMove(true);
        idleTime = Random.Range(5, 10);
        idleTimer = 0f;
    }

    public void Execute()
    {
        idleTimer += Time.deltaTime;

        if (monsterController.DetectPlayer())
        {
            monsterController.FllowPlayer(); 
            if(monsterController.InAttackRange())
            {
                monsterController.StateMachine.TransitionToState(monsterController.StateMachine.attackState);
                return;
            }
        }

        if (idleTimer >= idleTime)
        {
            monsterController.StateMachine.TransitionToState(monsterController.StateMachine.idleState);
        }

        monsterController.MoveWithGroundDetection();
    }

    public void Exit()
    {
        monsterController.animationController.OnMove(false);
    }
   
    public void UpdateStat(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }  
}
