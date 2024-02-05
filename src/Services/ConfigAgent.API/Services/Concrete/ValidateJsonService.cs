namespace ConfigAgent.API.Services.Concrete;

public class ValidateJsonService : IValidateJsonService
{
    public bool IsValidJson(string jsonString)
    {
        try
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                throw new ArgumentNullException(string.Format(Constants.Exceptions.ItemIsNullOrEmpty, nameof(jsonString))); 
            }
            
            JToken.Parse(jsonString);
            return true;
        }
        catch (JsonReaderException)
        {
            return false;
        }
    }
}