﻿using System.ComponentModel.DataAnnotations;

namespace BRIX.GameService.Entities
{
    public abstract class EntityBase<TKey> where TKey : struct
    {
        [Key]
        public TKey Id { get; set; }

        public DateTime Created { get; set; }
    }
}
