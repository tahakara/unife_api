using Core.Entities.Base.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Base.Concrete
{
    public abstract class EntityBaseClass : IEntity
    {
    }

    public abstract class BaseEntity : EntityBaseClass
    {
        public abstract DateTime CreatedAt { get; set; }
        public abstract DateTime UpdatedAt { get; set; }
    }

    public abstract class LogBaseEntity : EntityBaseClass
    {
        public abstract long LogId { get; set; }
        public abstract int LogTypeId { get; set; }
        public abstract DateTime LoggedAt { get; set; } 
        public abstract string? LoggedBy { get; set; }
    }
}
