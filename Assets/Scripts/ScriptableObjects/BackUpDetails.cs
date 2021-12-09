

namespace BackUpServiceTestTask.Data.ScriptableObjects
{
    using BackUpServiceTestTask.CloudService.Structs;
    using BackUpServiceTestTask.Database.Structs;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ServerAndDatabasesData", order = 1)]
    public class BackUpDetails : ScriptableObject
    {
        public ServerDetails ServerDetails;
        public DatabaseDetails[] DatabasesDetails;
        
    }


}
