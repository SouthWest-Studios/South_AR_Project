using UnityEngine;

public class GestureDetector : MonoBehaviour
{
    private float lastTapTime = 0f; // Stores the last tap time
    private Vector2 startTouchPos;  // Stores the starting position of a swipe
    private float doubleTapDelay = 0.3f; // Maximum time between taps to be considered a double tap

    void Update()
    {
        // Check if there are any touches
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began: // When the touch begins
                    DetectDoubleTap();
                    startTouchPos = touch.position;
                    break;

                case TouchPhase.Ended: // When the touch ends, check for swipe
                    DetectSwipe(touch.position);
                    break;
            }

            // Check for two-finger swipe
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began)
                {
                    startTouchPos = (touch1.position + touch2.position) / 2; // Calculate middle point
                }
                else if (touch1.phase == TouchPhase.Ended && touch2.phase == TouchPhase.Ended)
                {
                    DetectTwoFingerSwipe((touch1.position + touch2.position) / 2);
                }
            }
        }
    }

    // Detects double tap gesture
    void DetectDoubleTap()
    {
        if (Time.time - lastTapTime < doubleTapDelay)
        {
            Debug.Log("Doble toque detectado");
            OnDoubleTap();
        }
        lastTapTime = Time.time;
    }

    // Detects one-finger swipe gestures
    void DetectSwipe(Vector2 endTouchPos)
    {
        Vector2 swipeVector = endTouchPos - startTouchPos; // Calculate swipe direction
        if (swipeVector.magnitude < 50) return; // Ignore small movements

        if (Mathf.Abs(swipeVector.x) < Mathf.Abs(swipeVector.y)) // Check if swipe is vertical
        {
            if (swipeVector.y > 0)
            {
                Debug.Log("Deslizar hacia arriba detectado");
                OnSwipeUp();
            }
            else
            {
                Debug.Log("Deslizar hacia abajo detectado");
                OnSwipeDown();
            }
        }
    }

    // Detects two-finger swipe up gesture
    void DetectTwoFingerSwipe(Vector2 endTouchPos)
    {
        Vector2 swipeVector = endTouchPos - startTouchPos; // Calculate swipe direction
        if (swipeVector.magnitude < 50) return; // Ignore small movements

        if (swipeVector.y > 0) // Check if swipe is upwards
        {
            Debug.Log("Deslizar con dos dedos hacia arriba detectado");
            OnTwoFingerSwipeUp();
        }
    }

    // Callback for single-finger swipe up
    void OnSwipeUp()
    {
        Debug.Log("Acción para deslizar hacia arriba");
    }

    // Callback for single-finger swipe down
    void OnSwipeDown()
    {
        Debug.Log("Acción para deslizar hacia abajo");
    }

    // Callback for double tap
    void OnDoubleTap()
    {
        Debug.Log("Acción para doble toque");
    }

    // Callback for two-finger swipe up
    void OnTwoFingerSwipeUp()
    {
        Debug.Log("Acción para deslizar con dos dedos hacia arriba");
    }
}
