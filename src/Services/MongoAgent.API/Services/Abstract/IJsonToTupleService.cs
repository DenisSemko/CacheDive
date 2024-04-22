namespace MongoAgent.API.Services.Abstract;

public interface IJsonToTupleService
{
    List<Tuple<string, JObject>> ParseJson(string jsonString);
}