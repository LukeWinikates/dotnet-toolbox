namespace dotnet_toolbox.api.Query
{
    public interface IGetSetQuerier<T>
    {
        T Get(string key);
        void Set(string key, T value);
    }    
}