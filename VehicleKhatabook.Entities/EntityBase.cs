﻿using System.ComponentModel.DataAnnotations;

namespace Bonobo.Entities
{
    public abstract class EntityBase
    {
        public EntityBase() 
        {
            CreatedOn = DateTime.UtcNow;
            CreatedBy = 1;
        }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set;}
    }
}
