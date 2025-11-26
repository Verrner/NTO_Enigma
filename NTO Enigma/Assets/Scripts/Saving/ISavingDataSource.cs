namespace NTO
{
    public interface ISavingDataSource
    {
        object GetSavedData();
        void LoadData(string data);
        string Id { get; }
    }
}