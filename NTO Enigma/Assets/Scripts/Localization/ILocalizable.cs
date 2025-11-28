namespace NTO
{
    public interface ILocalizable
    {
        string Key { get; }
        void SetValue(string value);
    }
}