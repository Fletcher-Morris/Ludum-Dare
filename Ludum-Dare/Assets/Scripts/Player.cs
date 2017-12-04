using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float startSpeed = 1.0f;
    public float targetSpeed = 1;
    public float rotateSpeed;
    public AnimationCurve rotationCurve;
    private bool m_isRotating = false;
    private Rigidbody rb;
    public int score = 0;
    public Text scoreText;
    public Text speedText;
    private float m_speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

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

        if(m_speed < targetSpeed)
        {
            rb.AddForce(transform.forward * targetSpeed * Time.deltaTime, ForceMode.Impulse);
        }

        m_speed = rb.velocity.z;
        speedText.text = Mathf.RoundToInt(m_speed) + "MPH";
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
            yield return new WaitForEndOfFrame();
        }
        transform.localEulerAngles = targetRot;
        m_isRotating = false;

        yield return null;
    }

    public void AddCoin(int value)
    {
        score += value;
        scoreText.text = "SCORE: " + score;

        targetSpeed = score + 20;
    }
}