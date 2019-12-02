namespace MNCD.Core
{
    public interface INetworkAttribute<T>
    {
        public T Value { get; set; }
        public string Name { get; set; }
    }

    public class NumericAttribute : INetworkAttribute<double>
    {
        public double Value { get; set; }
        public string Name { get; set; }
    }
}
