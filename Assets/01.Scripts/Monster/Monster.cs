using UnityEngine;

public abstract class  Monster : MonoBehaviour
{
    [SerializeField] protected MonsterStateMachine monstersStateMachine;
    [SerializeField] protected Monster monsterCondition;
    [SerializeField] protected MonsterSO monsterData;
    [SerializeField] protected MonsterController monsterController;
    public MonsterSO Data => monsterData;
}