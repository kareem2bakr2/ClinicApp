
namespace ClinicApp.Service 
{
     public interface IEquipmentService
    {
        Task<EquipmentViewModel> GetEquipmentDetailsByIdAsync(int id);
        Task<ItemIndexViewModel> GetAllEquipmentsAsync(ItemIndexViewModel indexmodel);
        Task<ReturnResult> DeleteEquipmentAsync(int id);
        Task<Equipment> AddEquipmentAsync(EquipmentViewModel model);

        Task<ReturnResult> EditEquipmentAsync(EquipmentViewModel model);
        Task<EquipmentViewModel> CreateEquipmentStaticData();
        


        }
    }
