namespace BackUpServiceTestTask.CloudService
{
    using System.Threading.Tasks;
    using BackUpServiceTestTask.CloudService.Structs;
    using BackUpServiceTestTask.Database.Structs;

    public class BackupService
    {
        private ServerDetails serverDetails;


        public BackupService(ServerDetails data)
        {
            serverDetails = data;
        }


        public async Task<bool> BackUpDatabase(DatabaseDetails details)
        {
            // some actions here 
            await Task.Delay(10);
            UpdateAvailableStorage(-details.volume);
            return true;
        }


        private void UpdateAvailableStorage(float amount)
        {
            serverDetails.storageAmount += amount;
        }


        public float GetAvailableStorage()
        {
            return serverDetails.storageAmount;
        }


    }

}

