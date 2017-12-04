using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float startSpeed = 1.0f;
    public float targetSpeed;
    public float rotateSpeed;
    public AnimationCurve rotationCurve;
    private bool m_isRotating = false;

    void Update()
    {
        if (Input.GetAxis("Horizontal") >= 0.5f || Input.GetAxis("Horizontal") <= -0.5f)
        {
            if (!m_isRotating)
            {
                StartCoroutine(ChangeDirection(Mathf.RoundToInt(Input.GetAxis("Horizontal")) * 45));
                Debug.Log("Rotating");
            }
        }
    }

    public IEnumerator ChangeDirection(float change)
    {
        float t = 0;
        float lerp = 0;
        Vector3 startRot = transform.localEulerAngles;
        Vector3 targetRot = transform.localEulerAngles + new Vector3(0,0,change);

        m_isRotating = true;
        while (rotationCurve.Evaluate(t) < 1)
        {
            lerp = Mathf.Lerp(startRot.z, targetRot.z, rotationCurve.Evaluate(t));
            t += Time.deltaTime * rotateSpeed;
            transform.localEulerAngles = new Vector3(0, 0, lerp);
            Debug.Log(rotationCurve.Evaluate(t));
            yield return new WaitForEndOfFrame();
        }
        transform.localEulerAngles = targetRot;
        m_isRotating = false;

        yield return null;
    }

    public void AddCoin(int value)
    {

    }
}