using System;

namespace Shared.Domain.Model.Bases
{
    public class BaseEntity<T>
    {
        public T Id { get; set; }

    }
}