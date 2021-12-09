namespace BackUpServiceTestTask.Databases
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BackUpServiceTestTask.CloudService;
    using BackUpServiceTestTask.Database.Structs;

    public class BackupDatabase
    {
        private List<DatabaseDetails> databaseDetails;
        private List<int> databasesKeysByCostEfficiency;
        private BackupService backupService;

        public BackupDatabase(DatabaseDetails[] data, BackupService service)
        {
            databaseDetails = data.ToList();
            databasesKeysByCostEfficiency = new List<int>();
            backupService = service;
        }


        public async Task<List<int>> DoActions()
        {
            
            SortDatabases();
            return await BackupDatabases();
        }


        private void SortDatabases()
        {
            
            float efficiency;
            SortedList<int, float> tempDatabasesKeysByCostEfficiency = new SortedList<int, float>();

            for (int i = 0; i < databaseDetails.Count; i++)
            {
                DatabaseDetails currentDatabase = databaseDetails[i];
                efficiency = currentDatabase.savings / currentDatabase.volume;

                tempDatabasesKeysByCostEfficiency.Add(i, efficiency);
            }
            var orderByVal = tempDatabasesKeysByCostEfficiency.OrderByDescending(key => key.Value);


            databasesKeysByCostEfficiency = (from kvp in orderByVal select kvp.Key).Distinct().ToList();
        }


        public async Task<List<int>> BackupDatabases()
        {

            List<int> result = new List<int>();

            foreach (int i in databasesKeysByCostEfficiency)
            {
                DatabaseDetails currentDatabase = databaseDetails[i];
                float availableServerStorage = backupService.GetAvailableStorage();
                if (currentDatabase.volume > availableServerStorage)
                {
                    continue;
                }

                if (await backupService.BackUpDatabase(currentDatabase))
                {
                    result.Add(i);
                }   
            }

            return result;
        }



    }
}

