using UnityEngine.AI;

/// <summary>
/// This class will be an idle state for any gameobject that needs to be Chasing the Player
/// Ex: Enemies
/// </summary>
public class ChasePlayer : IState
{
    private readonly NavMeshAgent navmeshAgent;
    private Player player;

    /// <summary>
    /// Chase Player assumes this object has a Navmesh agent attached so it can chase the player around
    /// </summary>
    /// <param name="naveMeshAgent"></param>
    /// <param name="newPlayer"></param>
    public ChasePlayer(NavMeshAgent naveMeshAgent, Player newPlayer)
    {
        navmeshAgent = naveMeshAgent;
        player = newPlayer;
    }
    
    /// <summary>
    /// Tick is required for IState and sets the navmesh to the players position every frame
    /// </summary>
    public void Tick()
    {
        navmeshAgent.SetDestination(player.transform.position);
    }

    /// <summary>
    /// Enables to navmesh when the player enters within a certain range
    /// </summary>
    public void OnEnter()
    {
        navmeshAgent.enabled = true;
    }

    /// <summary>
    /// Dsiables to navmesh when the player exits a certain range
    /// </summary>
    public void OnExit()
    {
        navmeshAgent.enabled = false;
    }
}