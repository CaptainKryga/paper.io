using System;
using TMPro;
using UnityEngine;

namespace Controller
{
    public class CustomInput : MonoBehaviour
    {
        private bool isMobile = false;
        // public Text phaseDisplayText;
        private Touch theTouch;
        private float timeTouchEnded;
        private float displayTime = 0.5f;
        
        private Vector2 touchStartPosition, touchEndPosition; 
        private Vector3Int direction;

        [SerializeField] private TMP_Text debug;
        
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
            if (isMobile)
            {
                if (Input.touchCount > 0)
                {
                    theTouch = Input.GetTouch(0);
                    if (theTouch.phase == TouchPhase.Ended)
                    {
                        // phaseDisplayText.text = theTouch.phase.ToString();
                        timeTouchEnded = Time.time;
                    }
                    else if (Time.time - timeTouchEnded > displayTime)
                    {
                        // phaseDisplayText.text = theTouch.phase.ToString();
                        timeTouchEnded = Time.time;
                    }
                }
                else if (Time.time - timeTouchEnded > displayTime)
                {
                    // phaseDisplayText.text = "";
                }
            }
            else
            {

            }

            if (Input.touchCount > 0)
            {
                theTouch = Input.GetTouch(0);
                if (theTouch.phase == TouchPhase.Began)
                {
                    touchStartPosition = theTouch.position;
                }
                else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
                {
                    touchEndPosition = theTouch.position;
                    float x = touchEndPosition.x - touchStartPosition.x;
                    float y = touchEndPosition.y - touchStartPosition.y;
                    if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                    {
                    }
                    else if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        direction = x > 0 ? Vector3Int.right : Vector3Int.left;
                    }
                    else
                    {
                        direction = y > 0 ? Vector3Int.up : Vector3Int.down;
                    }

                    debug.text = "move: " + direction;
                }
            }
        }
    }
}
