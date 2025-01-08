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
            monsterController.Move();
            if(monsterController.InAttackRange())
            {
                monsterController.StateMachine.TransitionToState(monsterController.StateMachine.attackState);
            }
            else
            {
                return;
            }
        }

        if (idleTimer >= idleTime)
        {
            monsterController.StateMachine.TransitionToState(monsterController.StateMachine.idleState);
        }

        if (monsterController.monsterGround.GetOnGround())
        {
            isGround = false;
            monsterController.Move();
        }
        else
        {
            if (!isGround)
            {
                monsterController.ReverseDirection();
                isGround = true;
            }
            monsterController.Move();
        }
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
