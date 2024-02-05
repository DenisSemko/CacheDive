namespace ConfigAgent.API.Services.Abstract;

public interface IValidateJsonService
{ 
    bool IsValidJson(string jsonString);
}