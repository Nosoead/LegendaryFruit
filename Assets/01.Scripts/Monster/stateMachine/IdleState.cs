using System.Collections;
using UnityEngine;

public class IdleState :IState
{
    private MonsterController monsterController;
    private float idleTime;
    private float idleTimer = 0f;

    public IdleState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }

    public void Enter()
    {
        idleTime = Random.Range(1, 5);
        idleTimer = 0f;
    }

    public void Execute()
    {
        idleTimer += Time.deltaTime;
       
        if (idleTimer >= idleTime)
        {
            if (Random.value < 0.3f)
            {
                monsterController.ReverseDirection(); 
            }
            monsterController.StateMachine.TransitionToState(monsterController.StateMachine.patrollState);
            return;
        }
        
        if (monsterController.DetectPlayer() && monsterController.InAttackRange())
        {
            monsterController.StateMachine.TransitionToState(monsterController.StateMachine.attackState);
            return;
        }
    }

    public void Exit()
    {

    }
    public void UpdateStat(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }
}