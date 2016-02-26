namespace AppGene.Common.Core
{
    public interface IEntityModel<TEntity>
    {
        TEntity Entity { get; set; }
    }
}
