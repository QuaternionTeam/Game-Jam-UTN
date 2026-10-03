using System;
using System.Collections.Generic;

public interface IFSM
{
  void ChangeState<T>() where T : IState;
}

public class FSM<T> : IFSM where T : IState
{
  private T _currentState;
  internal T CurrentState => _currentState;
  
  private readonly Dictionary<Type, T> _states = new();

  internal void RegisterState(T state)
  {
    _states[state.GetType()] = state;
    state.Initialize(this);
  }

  public void ChangeState<TState>() where TState : IState
  {
    if (!_states.TryGetValue(typeof(TState), out var newState))
    {
      UnityEngine.Debug.LogError($"El estado {typeof(T).Name} no ha sido registrado en la FSM.");
      return;
    }

    _currentState?.OnExit();
    _currentState = newState;
    _currentState.OnEnter();
  }

  internal void Update(float deltaTime)
  {
    _currentState?.Update(deltaTime);
  }
}