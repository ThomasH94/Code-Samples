using UnityEngine;

/// <summary>
/// This class will be an idle state for any gameobject that needs to be Idling
/// Ex: Enemies
/// </summary>
public class Idle : IState
{
    public void Tick()
    {
        Debug.Log("Idle");
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }
}