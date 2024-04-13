namespace RedisAgent.API.Services.Concrete;

public class JsonToTupleService : IJsonToTupleService
{
    public List<Tuple<string, JObject>> ParseJson(string jsonString)
    {
        List<Tuple<string, JObject>> result = new();
        JArray jsonArray = JArray.Parse(jsonString);

        foreach (JToken item in jsonArray)
        {
            string individualJson = item.ToString(Formatting.Indented);
            JObject jsonObject = JObject.Parse(individualJson);

            string entity = (string)jsonObject["entity"];
            JObject data = (JObject)jsonObject["data"];
            result.Add(new Tuple<string, JObject>(entity, data));
        }

        return result;
    }
}