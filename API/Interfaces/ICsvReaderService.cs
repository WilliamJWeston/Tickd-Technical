namespace API.Interfaces
{
    public interface ICsvReaderService
    {
        Task<bool> ReadAsync();
        T GetRecord<T>() where T : class;
        void Initialise(Stream stream);
    }
}
