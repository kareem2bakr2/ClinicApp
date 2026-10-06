namespace ClinicApp.Repository
{
    public class EquipmentRepository :
        GenericRepository<Equipment>
        ,IEquipmentRepository
    {
        public EquipmentRepository(ClinicAppContext context) : base(context) { }
    }
}
