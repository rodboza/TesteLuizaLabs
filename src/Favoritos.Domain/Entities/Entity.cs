using System;
using System.ComponentModel.DataAnnotations;

namespace Favoritos.Domain.Entities
{
    public abstract class Entity : IEquatable<Entity>
    {
        public Entity()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        public bool Equals(Entity other)
        {
            return Id == other.Id;
        }
    }
}