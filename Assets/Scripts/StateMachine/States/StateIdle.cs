public class StateIdle : State, IRestState
{
    protected override void Work() { }

    protected override bool CanHandle() => false;
}
