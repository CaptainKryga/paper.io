using System;
using TMPro;
using UnityEngine;

namespace Controller
{
    public class CustomInput : MonoBehaviour
    {
        public static CustomInput Singleton { get; private set; }
        
        private bool isMobile = false;
        Vector3Int direction = Vector3Int.zero;
 
        private Vector2 touchStartPosition, touchEndPosition; 

        [SerializeField] private TMP_Text debug;

        public Action<Vector3Int> UpdateDirection_Action;

        private void Awake()
        {
            Singleton = this;
        }

        private void Start()
        {
            if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer)
            {
                isMobile = true;
            }
        }

        private void Update()
        {
            if (!isMobile)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                    direction = Vector3Int.FloorToInt(new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f));
                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                    direction = Vector3Int.FloorToInt(new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f));
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    Touch theTouch = Input.GetTouch(0);
                    if (theTouch.phase == TouchPhase.Began)
                    {
                        touchStartPosition = theTouch.position;
                    }
                    else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
                    {
                        touchEndPosition = theTouch.position;
                        float x = touchEndPosition.x - touchStartPosition.x;
                        float y = touchEndPosition.y - touchStartPosition.y;
                        if (Mathf.Abs(x) > Mathf.Abs(y))
                            direction = x > 0 ? Vector3Int.right : Vector3Int.left;
                        else
                            direction = y > 0 ? Vector3Int.up : Vector3Int.down;

                        debug.text = "move: " + direction;
                    }
                }
            }
            
            UpdateDirection_Action?.Invoke(direction);
        }
    }
}
