using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float startSpeed = 1.0f;
    public float targetSpeed = 1;
    public float maxSpeed = 400f;
    public float rotateSpeed;
    public AnimationCurve rotationCurve;
    private bool m_isRotating = false;
    private Rigidbody rb;
    private Camera m_cam;
    public int score = 0;
    public Text scoreText;
    public Text speedText;
    private float m_speed;
    public int scoreDecay = 10;
    private float decayedScore;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_cam = Camera.main;
        targetSpeed = startSpeed;
    }

    void Update()
    {
        decayedScore = score - scoreDecay * Time.deltaTime;
        score = Mathf.RoundToInt(decayedScore);
        if(score <= 0)
        {
            score = 0;
        }
        scoreText.text = "SCORE: " + score;

        if (Input.GetAxis("Horizontal") >= 0.5f || Input.GetAxis("Horizontal") <= -0.5f)
        {
            if (!m_isRotating)
            {
                StartCoroutine(ChangeDirection(Mathf.RoundToInt(Input.GetAxis("Horizontal")) * 45));
                Debug.Log("Rotating");
            }
        }

        targetSpeed = Mathf.Lerp(m_speed, decayedScore + 21, 0.9f);

        targetSpeed = Mathf.Clamp(targetSpeed, 0, maxSpeed);
        if(m_speed < targetSpeed)
        {
            rb.AddForce(transform.forward * targetSpeed * Time.deltaTime, ForceMode.Impulse);
        }

        m_speed = rb.velocity.z;
        speedText.text = Mathf.FloorToInt(m_speed) - 1 + "MPH";
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
        decayedScore = score;
        targetSpeed += value;
    }
}