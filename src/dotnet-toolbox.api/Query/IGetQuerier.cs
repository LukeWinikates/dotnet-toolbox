namespace dotnet_toolbox.api.Query
{
    public interface IGetQuerier<T>
    {
        T Get(string key);
    }
}