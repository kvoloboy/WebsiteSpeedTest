using System;

namespace RequestSpeedTest.BusinessLogic.Exceptions
{
    public class EntityNotFoundException<TEntity> : Exception
    {
        public EntityNotFoundException(int id)
            : base(PrepareMessage(id))
        {
        }

        private static string PrepareMessage(int id) => $"Not found {typeof(TEntity).Name} with id {id}";
    }
}
