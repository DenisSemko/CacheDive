namespace ExperimentAgent.API.Models;

public record ExperimentRequest(DatabaseType DatabaseType, string Query, int QueryExecutionNumber);