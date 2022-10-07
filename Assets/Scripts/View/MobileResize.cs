using UnityEngine;

public class MobileResize : MonoBehaviour
{
    [SerializeField] private RectTransform rt;
    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer)
        {
            rt.localScale = new Vector3(.75f, .75f, 0);
        }
    }
}
