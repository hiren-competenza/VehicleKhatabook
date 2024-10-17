namespace VehicleKhatabook.Entities
{
    public abstract class EntityBase
    {
        public EntityBase() 
        {
            CreatedOn = DateTime.UtcNow;
            CreatedBy = 1;
        }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set;}
    }
}
