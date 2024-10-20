﻿using System.ComponentModel.DataAnnotations;

namespace BRIX.Entity
{
    public abstract class EntityBase<TKey> where TKey : struct
    {
        [Key]
        public TKey Id { get; set; }

        [Timestamp]
        public DateTime Timestamp { get; set; }

        public virtual void Configure() { }
    }
}
