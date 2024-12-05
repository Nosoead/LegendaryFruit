public class MonsterController : Monster
{
    private MonsterStateMachine stateMachine;
    public MonsterStateMachine StateMachine => stateMachine;
    private AttributeLogics attributeLogics;

    private void Awake()
    {
        stateMachine = new MonsterStateMachine(this);
        attributeLogics = new NormalLogic(); // dddd
    }

    private void Start()
    {
        StateMachine.Initialize(StateMachine.patrollState);
    }

    private void Update()
    {
        StateMachine.Excute();
    }

    public void OnHit()
    {
        //어트리뷰트 딕셔너리에서 값 갖고오기
        
        //애니메이션에서 이벤트로 불러와짐.
    }
}
