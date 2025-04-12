using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GestureDetector : MonoBehaviour
{
    private float lastTapTime = 0f; // Stores the last tap time
    private Vector2 startTouchPos;  // Stores the starting position of a swipe
    private float doubleTapDelay = 0.3f; // Maximum time between taps to be considered a double tap
    private Camera mainCamera; // Reference to the main camera

    private LineRenderer lineRenderer; // Line renderer for trail effect
    private List<Vector3> trailPoints = new List<Vector3>(); // Stores trail points
    private float trailDuration = 2f; // Duration before trail fades
    private bool isDrawingTrail = false; // Track if user is actively drawing
    private GameObject doubleTapEffect; // Object for the double-tap effect

    public Image doDamage;
    public Color fireDamage, iceDamage, windDamage, earthDamage;

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera

        // Create LineRenderer component for trail effect
        GameObject trailObject = new GameObject("GestureTrail");
        lineRenderer = trailObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.3f; // Increased thickness
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white * 0.8f; // Light Gray
        lineRenderer.endColor = Color.white * 0.5f;
        lineRenderer.positionCount = 0;

        // Create the double-tap effect object
        doubleTapEffect = new GameObject("DoubleTapEffect");
        SpriteRenderer effectRenderer = doubleTapEffect.AddComponent<SpriteRenderer>();
        effectRenderer.material = new Material(Shader.Find("Sprites/Default"));
        effectRenderer.color = Color.gray;
        doubleTapEffect.SetActive(false); // Initially, disable the effect
    }

    void Update()
    {
        // Check if there are any touches
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    DetectDoubleTap();
                    startTouchPos = touch.position;
                    trailPoints.Clear();
                    isDrawingTrail = true;
                    break;

                case TouchPhase.Moved:
                    UpdateTrailEffect(touch.position);
                    break;

                case TouchPhase.Ended:
                    DetectSwipe(touch.position);
                    isDrawingTrail = false;
                    StartCoroutine(FadeTrailGradually());
                    break;
            }

            // Check for two-finger swipe
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began)
                {
                    startTouchPos = (touch1.position + touch2.position) / 2;
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
            GetComponent<FrustrumKiller>().KillEnemiesByType(EnemyType.Earth);
            doDamage.color = earthDamage;
            StartCoroutine(ShowDoubleTapEffect(Input.GetTouch(0).position));
        }
        lastTapTime = Time.time;
    }

    // Detects one-finger swipe gestures
    void DetectSwipe(Vector2 endTouchPos)
    {
        Vector2 swipeVector = endTouchPos - startTouchPos;
        if (swipeVector.magnitude < 50) return;

        if (Mathf.Abs(swipeVector.x) < Mathf.Abs(swipeVector.y))
        {
            if (swipeVector.y > 0)
            {
                Debug.Log("Deslizar hacia arriba detectado");
                doDamage.color = fireDamage;
                GetComponent<FrustrumKiller>().KillEnemiesByType(EnemyType.Fire);
            }
            else
            {
                Debug.Log("Deslizar hacia abajo detectado");
                doDamage.color = iceDamage;
                GetComponent<FrustrumKiller>().KillEnemiesByType(EnemyType.Ice);
            }
        }
    }

    // Detects two-finger swipe up gesture
    void DetectTwoFingerSwipe(Vector2 endTouchPos)
    {
        Vector2 swipeVector = endTouchPos - startTouchPos;
        if (swipeVector.magnitude < 50) return;

        if (swipeVector.y > 0)
        {
            Debug.Log("Deslizar con dos dedos hacia arriba detectado");
            doDamage.color = windDamage;
            GetComponent<FrustrumKiller>().KillEnemiesByType(EnemyType.Wind);
        }
    }

    // Displays the double tap effect at the tap position
    IEnumerator ShowDoubleTapEffect(Vector2 tapPosition)
    {
        doubleTapEffect.SetActive(true);
        doubleTapEffect.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(tapPosition.x, tapPosition.y, 10f));

        SpriteRenderer effectRenderer = doubleTapEffect.GetComponent<SpriteRenderer>();
        float scale = 0.1f;
        float maxScale = 1.0f;

        // Gradually expand the effect
        while (scale < maxScale)
        {
            scale += Time.deltaTime * 2f; // Speed of expansion
            doubleTapEffect.transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        // Fade out effect
        float fadeDuration = 1f;
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = 1f - (timeElapsed / fadeDuration);
            effectRenderer.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        // Disable the effect after fading
        doubleTapEffect.SetActive(false);
    }

    // Updates the trail effect while swiping
    void UpdateTrailEffect(Vector2 touchPosition)
    {
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10f));
        trailPoints.Add(worldPos);

        lineRenderer.positionCount = trailPoints.Count;
        lineRenderer.SetPositions(trailPoints.ToArray());
    }

    // Gradually fades the trail by removing points from the start
    IEnumerator FadeTrailGradually()
    {
        float time = 0f;
        while (time < trailDuration && trailPoints.Count > 0)
        {
            time += Time.deltaTime;

            // Remove points from the start of the trail to make it fade progressively
            int pointsToRemove = Mathf.CeilToInt(trailPoints.Count * (Time.deltaTime / trailDuration));
            for (int i = 0; i < pointsToRemove && trailPoints.Count > 0; i++)
            {
                trailPoints.RemoveAt(0);
            }

            lineRenderer.positionCount = trailPoints.Count;
            lineRenderer.SetPositions(trailPoints.ToArray());

            yield return null;
        }

        lineRenderer.positionCount = 0; // Ensure trail is completely removed
    }
}
