namespace Resiliency.CircuitBreaker
{
    public interface ICircuitStateVisitor
    {
        void Visit(OpenState state);
        void Visit(ClosedState state);
        void Visit(HalfOpenState state);
        void Visit(PermanentlyClosedState state);
    }
}