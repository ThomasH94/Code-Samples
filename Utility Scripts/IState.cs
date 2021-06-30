/// <summary>
/// Interface to define what states require when being created
/// </summary>
public interface IState
{
    void Tick();
    void OnEnter();
    void OnExit();
}