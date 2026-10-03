public interface IState
{
  void Initialize(IFSM fsm);
  void OnEnter();
  void OnExit();
  void Update(float deltaTime);
}

public abstract class BaseState : IState
{
  protected IFSM Fsm { get; private set; }

  public virtual void Initialize(IFSM fsm)
  {
    Fsm = fsm;
  }

  protected virtual void Transition<T>() where T : IState
  {
    Fsm.ChangeState<T>();
  }

  public abstract void OnEnter();
  public abstract void OnExit();
  public abstract void Update(float deltaTime);
}
