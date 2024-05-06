namespace MongoAgent.API.Common;

public class DeserializeParsedDataHelper<T>
{
    public T DeserializeData(string json) => 
        JsonConvert.DeserializeObject<T>(json);
}