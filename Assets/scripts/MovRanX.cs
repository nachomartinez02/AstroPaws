using System.Collections;
using UnityEngine;

public class MovRanX : MonoBehaviour
{
    public float distance = 1.0f;
    public float interval = 0.5f;
    public float speed = 1.0f;
    private bool movingRight = true; 
    private Vector3 basePosition;
    private Vector3 targetPosition; 
    private float journeyLength; 
    private float startTime; 
    private Coroutine oscillateCoroutine; 

    void Start()
    {
        basePosition = transform.position;
        targetPosition = basePosition;
        oscillateCoroutine = StartCoroutine(Oscillate());
    }

    void Update()
    {
        if (journeyLength > 0)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(basePosition, targetPosition, Mathf.SmoothStep(0, 1, fractionOfJourney));
        }
    }

    public void SetBasePosition(Vector3 newPosition)
    {
        basePosition = newPosition;
        targetPosition = basePosition;
        journeyLength = 0; 
    }

    public void StopOscillation()
    {
        if (oscillateCoroutine != null)
        {
            StopCoroutine(oscillateCoroutine);
            oscillateCoroutine = null;
        }
    }

    IEnumerator Oscillate()
    {
        while (true)
        {
            yield return new WaitUntil(() => Vector3.Distance(transform.position, targetPosition) < 0.01f);
            yield return new WaitForSeconds(interval);
            float chance = Random.Range(0f, 1f);

            if (chance <= 0.25f)
            {
                basePosition = transform.position;
                startTime = Time.time;

                if (movingRight)
                {
                    targetPosition = basePosition + new Vector3(distance, 0, 0);
                }
                else
                {
                    targetPosition = basePosition - new Vector3(distance, 0, 0);
                }

                journeyLength = Vector3.Distance(basePosition, targetPosition);
                movingRight = !movingRight;
            }
        }
    }
}
