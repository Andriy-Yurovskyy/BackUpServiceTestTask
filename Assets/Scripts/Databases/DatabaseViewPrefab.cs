namespace BackUpServiceTestTask.Database
{

    using UnityEngine;
    using UnityEngine.UI;
    using BackUpServiceTestTask.Database.Structs;

    public class DatabaseViewPrefab : MonoBehaviour
    {
        public Text volume;
        public Text savings;



        private void Awake()
        {
            gameObject.SetActive(false);
        }


        public void Init(DatabaseDetails data)
        {
            volume.text = data.volume.ToString();
            savings.text = data.savings.ToString();
        }
    }



}


