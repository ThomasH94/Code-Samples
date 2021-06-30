using System;

/// <summary>
/// This class will handle state transitions with a state "From" and a state "To" with a "Condition"
/// </summary>
public class StateTransition
{
    #region States information
    public readonly IState From;
    public readonly IState To;
    public readonly Func<bool> Condition;
    #endregion

    /// <summary>
    /// CTOR
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="condition"></param>
    public StateTransition(IState from, IState to, Func<bool> condition)    // Use a function that returns back true/false in our state transition
    {
        From = @from;
        To = to;
        Condition = condition;
    }
}