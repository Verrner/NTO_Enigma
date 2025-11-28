namespace NTO
{
    public interface ISavingReceiver
    {
        object GetSavedData();
        void LoadData(string data);
        string Id { get; }
    }
}