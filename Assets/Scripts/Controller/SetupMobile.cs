using UnityEngine;

namespace Controller
{
    public class SetupMobile : MonoBehaviour
    {
        private void Start()
        {
            if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer)
            {
            
            }
        }
    }
}
