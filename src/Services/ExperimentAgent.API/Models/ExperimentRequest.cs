namespace ExperimentAgent.API.Models;

public record ExperimentRequest(DatabaseType DatabaseType, ExperimentType ExperimentType, int QueryExecutionNumber, bool IsCacheCleaned);