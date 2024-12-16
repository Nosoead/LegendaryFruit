public class PlayerMovementStateMachine
{
    public IState CurrentState { get; private set; }

    public PlayerIdleState idleState;
    public PlayerAirborneState airborneState;
    public PlayerMoveState moveState;
    public PlayerDashState dashState;
    //public PlayerJumpState jumpState;

    public PlayerMovementStateMachine(PlayerMovementHandler player)
    {
        this.idleState = new PlayerIdleState(player);
        this.airborneState = new PlayerAirborneState(player);
        this.moveState = new PlayerMoveState(player);
        this.dashState = new PlayerDashState(player);
        //this.jumpState = new PlayerJumpState(player);
    }

    public void Initialize(IState state)
    {
        CurrentState = state;
        state.Enter();
    }

    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
    }

    public void Execute()
    {
        if (CurrentState != null)
        {
            CurrentState.Execute();
        }
    }
}