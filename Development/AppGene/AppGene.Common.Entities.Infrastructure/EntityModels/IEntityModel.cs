namespace AppGene.Common.Entities.Infrastructure.EntityModels
{
    public interface IEntityModel<TEntity>
    {
        TEntity Entity { get; set; }
    }
}
