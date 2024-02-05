namespace SQLServerAgent.API.Common;

public class Result<T>
{
    private Dictionary<Type, List<T>> entityList = new ();

    public void AddEntities<TType>(List<T> entities)
    {
        Type entityType = typeof(TType);

        if (!entityList.ContainsKey(entityType))
        {
            entityList.Add(entityType, entities);
        }
    }

    public List<T> GetAllEntities() => 
        entityList.Values.SelectMany(list => list).ToList();
}