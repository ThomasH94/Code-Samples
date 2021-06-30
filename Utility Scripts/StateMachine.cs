using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines StateMachines for any object that needs to track it's state
/// This class will also handle setting transitions and updating the current state via the Tick method
/// </summary>
public class StateMachine
{
    #region Transitions
    private List<StateTransition> stateTransitions = new List<StateTransition>();
    private List<StateTransition> anyStateTransitions = new List<StateTransition>();    // Special states that can be transitioned to from any state
    #endregion

    #region States
    private IState currentState;
    public IState CurrentState => currentState;    // Get not set
    public event Action<IState> OnStateChanged;
    #endregion

    /// <summary>
    /// Adds a transition to any state with a condition
    /// </summary>
    /// <param name="to"></param>
    /// <param name="condition"></param>
    public void AddAnyTransition(IState to, Func<bool> condition)
    {
        var stateTransition = new StateTransition(null, to, condition);
        anyStateTransitions.Add(stateTransition);
    }

    /// <summary>
    /// Adds a transition state from a specific transition to any desired state with a condition
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="condition"></param>
    public void AddTransition(IState from, IState to, Func<bool> condition)
    {
        var stateTransition = new StateTransition(from, to, condition);
        stateTransitions.Add(stateTransition);
    }

    /// <summary>
    /// Sets the current state of the gameobject
    /// </summary>
    /// <param name="state"></param>
    public void SetState(IState state)
    {
        if(currentState == state)
            return;
        
        currentState?.OnExit();
        
        currentState = state;
        Debug.Log($"Changed to {state}");
        
        currentState.OnEnter();
        OnStateChanged?.Invoke(currentState);
    }

    /// <summary>
    /// Updates the state of the object every frame and checks for any needed transitions
    /// </summary>
    public void Tick()
    {
        StateTransition transition = CheckForTransition();
        if (transition != null)
        {
            SetState(transition.To);
        }
        
        currentState.Tick();
    }

    /// <summary>
    /// Checks if the state needs to be transitioned to every frame
    /// </summary>
    /// <returns></returns>
    private StateTransition CheckForTransition()
    {
        foreach (StateTransition transition in anyStateTransitions)
        {
            if(transition.Condition())
                return transition;
        }
        
        foreach (var transition in stateTransitions)
        {
            if (transition.From == currentState && transition.Condition())
                return transition;
        }
        
        return null;
    }
}