namespace NTO
{
    public interface ISavingReceiver
    {
        string GetSavedData();
        void LoadData(string data);
        string Id { get; }
    }
}