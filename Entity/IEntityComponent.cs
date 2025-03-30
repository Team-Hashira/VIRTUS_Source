namespace Hashira.Entities
{
    public interface IEntityComponent
    {
        public void Initialize(Entity entity);
    }

    public interface IAfterInitialzeComponent
    {
        public void AfterInit();
    }

    public interface IEntityDisposeComponent
    {
        public void Dispose();
    }
}