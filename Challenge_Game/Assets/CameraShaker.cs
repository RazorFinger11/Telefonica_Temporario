using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraShaker : MonoBehaviour
{
    [System.Serializable]
    public class Properties
    {
        public float angle;
        public float strength;
        public float speed;
        public float duration;

        [Range(0, 1)]
        public float noisePercent;
        [Range(0, 1)]
        public float dampingPercent;
        [Range(0, 1)]
        public float rotationPercent;

    }

    public Properties testProperties;
    IEnumerator curShakeCoroutine;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartShake(testProperties);
        }
    }

    public void StartShake(Properties properties)
    {
        if (curShakeCoroutine != null)
        {
            StopCoroutine(curShakeCoroutine);
        }

        curShakeCoroutine = Shake(properties);
        StartCoroutine(Shake(properties));
    }

    IEnumerator Shake(Properties properties)
    {
        float completionPercent = 0;
        float movePercent = 0;

        float angle_radians = properties.angle * Mathf.Deg2Rad - Mathf.PI;
        Vector3 previousWaypoint = new Vector3(0, 0, transform.position.z);
        Vector3 curWaypoint = new Vector3(0, 0, transform.position.z);
        float moveDistance = 0;

        while(true)
        {
            if (movePercent >= 1 || completionPercent == 0)
            {
                float noiseAngle = (Random.value - .5f) * Mathf.PI;
                angle_radians += Mathf.PI + noiseAngle * properties.noisePercent;

                curWaypoint = new Vector3(Mathf.Cos(angle_radians), Mathf.Sin(angle_radians)) * properties.strength;
                previousWaypoint = transform.localPosition;

                moveDistance = Vector3.Distance(curWaypoint, previousWaypoint);
                movePercent = 0;
            }

            completionPercent += Time.deltaTime/properties.duration;
            movePercent += Time.deltaTime / moveDistance * properties.speed;
            transform.localPosition = Vector3.Lerp(previousWaypoint, curWaypoint, movePercent);

            yield return null;
        }
    }
}
