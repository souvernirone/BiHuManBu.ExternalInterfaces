namespace BiHuManBu.ExternalInterfaces.Infrastructure.Specification
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T candidate);

        ISpecification<T> And(ISpecification<T> other);

        ISpecification<T> Not();
    }
}
