using System;

namespace Hb.Domain.Entities.Base
{
    public interface IEntityBase
    {
    }

    public interface IEntityBase<out TKey> : IEntityBase where TKey : IEquatable<TKey>
    {
        public TKey Id { get; }
    }
}
