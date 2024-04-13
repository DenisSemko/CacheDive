namespace RedisAgent.API.Common.Helpers;

public class DeserializeParsedDataHelper<T>
{
    public T DeserializeData(string json) => 
        JsonConvert.DeserializeObject<T>(json);
}