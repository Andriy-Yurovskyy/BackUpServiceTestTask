
namespace BackUpServiceTestTask
{
    using UnityEngine;
    using BackUpServiceTestTask.Data.ScriptableObjects;
    using UnityEngine.UI;
    using BackUpServiceTestTask.Databases;
    using BackUpServiceTestTask.CloudService;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using BackUpServiceTestTask.Database;
    using BackUpServiceTestTask.Database.Structs;

    public class AppManager : MonoBehaviour
    {

        public BackUpDetails backUpDetails;

        [SerializeField]
        private GameObject databaseDoneContainer;

        [SerializeField]
        private Transform databaseDoneContentContainer;

        [SerializeField]
        private Text databaseDoneResultText;


        [SerializeField]
        private GameObject databaseNotDoneContainer;

        [SerializeField]
        private Transform databaseNotDoneContentContainer;

        [SerializeField]
        private Text databaseNotDoneResultText;


        [SerializeField]
        private Button calculateBackUpButton;

        [SerializeField]
        private Text storageTextAmount;

        [SerializeField]
        private DatabaseViewPrefab databaseViewPrefab;

        private BackupService backupService;

        private List<int> databasesDone;


        // Start is called before the first frame update
        void Awake()
        {
            InitBackUpService();
            UpdateStorageAmountText();
            InitUiElements();
            InitBackUpButton();
        }


        private void PerformResultUiActions()
        {
            InstantiateDataBaseView();
            UpdateStorageAmountText();
            UpdateTesultsTexts();
            ShowHideObjectsAfterResult();
        }


        private void InitBackUpButton()
        {
            calculateBackUpButton.onClick.AddListener(
                async delegate {
                    await BackUpDatabases();
                }
            );
        }


        private void InitUiElements()
        {
            databaseDoneContainer.SetActive(false);
            databaseNotDoneContainer.SetActive(false);
            storageTextAmount.text = string.Format(storageTextAmount.text, backUpDetails.ServerDetails.storageAmount.ToString());
        }


        private void InitBackUpService()
        {
            backupService = new BackupService(backUpDetails.ServerDetails);
        }


        private async Task BackUpDatabases()
        {
            BackupDatabase backupDatabase = new BackupDatabase(backUpDetails.DatabasesDetails, backupService);
            databasesDone = await backupDatabase.DoActions();
            PerformResultUiActions();

        }


        private void InstantiateDataBaseView()
        {
            for (int i = 0; i < backUpDetails.DatabasesDetails.Length; i++)
            {
                DatabaseViewPrefab item = GameObject.Instantiate(databaseViewPrefab);
                DatabaseDetails currentDatabase = backUpDetails.DatabasesDetails[i];
                item.savings.text = string.Format(item.savings.text, currentDatabase.savings);
                item.volume.text = string.Format(item.volume.text, currentDatabase.volume);

                item.gameObject.transform.SetParent(WasDatabaseBackedup(i) ? databaseDoneContentContainer : databaseNotDoneContentContainer);
                item.gameObject.transform.localScale = Vector3.one;
                item.gameObject.SetActive(true);
                
            }
        }


        private void UpdateTesultsTexts()
        {
            int totalBackupCount = 0;
            float totalBackupStorage = 0;
            float totalBackupSavings = 0;
            int totalSkippedCount = 0;
            float totalSkippedStorage = 0;
            float totalSkippedLost = 0;
            for (int i = 0; i < backUpDetails.DatabasesDetails.Length; i++)
            {
                DatabaseDetails currentDataabse = backUpDetails.DatabasesDetails[i];
                if (WasDatabaseBackedup(i))
                {
                    totalBackupStorage += currentDataabse.volume;
                    totalBackupSavings += currentDataabse.savings;
                    totalBackupCount++;
                }
                else
                {
                    totalSkippedStorage += currentDataabse.volume;
                    totalSkippedLost += currentDataabse.savings;
                    totalSkippedCount++;
                }
            }

            databaseDoneResultText.text = string.Format(databaseDoneResultText.text, totalBackupCount, totalBackupStorage, totalBackupSavings);
            databaseNotDoneResultText.text = string.Format(databaseNotDoneResultText.text, totalSkippedCount, totalSkippedStorage, totalSkippedLost);


        }


        private void ShowHideObjectsAfterResult()
        {
            databaseDoneContainer.SetActive(true);
            databaseNotDoneContainer.SetActive(true);
            calculateBackUpButton.gameObject.SetActive(false);

        }


        private bool WasDatabaseBackedup (int index)
        {
            return databasesDone.Contains(index);
        }


        private void UpdateStorageAmountText()
        {
            storageTextAmount.text = $"{Mathf.Round(backupService.GetAvailableStorage() * 100) / 100}GB";
        }


    }

}


