using UnityEngine;

public abstract class  Monster : MonoBehaviour
{
    [SerializeField] protected MonsterStateMachine MonstersStateMachine;
    [SerializeField] protected Monster MonsterCondition;
    [SerializeField] protected MonsterSO MonsterData;
    public MonsterSO Data => MonsterData;
}
