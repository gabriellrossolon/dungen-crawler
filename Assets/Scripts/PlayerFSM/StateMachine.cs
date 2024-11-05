using System.Collections.Generic;
using System;
using UnityEngine;

// A interface que os estados vão implementar
public interface IState
{
    void OnEnter();
    void OnExit();
    void OnTick();
    void OnFixedTick();
}

// A StateMachine em si, que vai ser responsável por manter uma lista de estados e fazer as transições
public class StateMachine
{
    public Dictionary<string, IState> States = new();
    public IState CurrentState;

    public virtual void Tick() => CurrentState?.OnTick();
    public virtual void FixedTick() => CurrentState?.OnFixedTick();

    public void AddState(string key, IState state)
    {
        if (States.ContainsKey(key))
        {
            Debug.LogWarning($"State already found on the dictionary: {key}");
            return;
        }

        States.Add(key, state);
    }

    public void SetState(string key)
    {
        if (States.TryGetValue(key, out IState state))
        {
            CurrentState?.OnExit();
            CurrentState = state;
            CurrentState.OnEnter();
        }
        else
        {
            throw new ArgumentException("Invalid state key.");
        }
    }

    public void SetInitialState(string key)
    {
        SetState(key);
    }
}

