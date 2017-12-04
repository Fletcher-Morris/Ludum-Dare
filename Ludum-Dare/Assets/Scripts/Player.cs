using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;

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
    public float minFov = 60;
    public float maxFov = 90;
    public float hueSpeed = 1.0f;
    private float m_hue = 0f;

    public GameObject menuPanel;
    public GameObject pausePanel;

    public bool paused = false;
    public bool canControll = false;

    public Material surfaceMat;
    public PostProcessingProfile profile;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_cam = Camera.main;
        targetSpeed = startSpeed;

        var grading = profile.colorGrading.settings;
        grading.basic.hueShift = 0;
        profile.colorGrading.settings = grading;
    }

    public void Go()
    {
        Time.timeScale = 1.0f;
        canControll = true;
        menuPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Pause()
    {
        paused = !paused;

        if (paused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1.0f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canControll)
        {
            Pause();
        }

        decayedScore = score - scoreDecay * Time.deltaTime;
        score = Mathf.RoundToInt(decayedScore);
        if(score <= 0)
        {
            score = 0;
        }

        float targetFov = 60f;
        Vector3 newCamRot = m_cam.transform.localEulerAngles;
        if (canControll)
        {
            scoreText.text = "SCORE: " + score;

            if (Input.GetAxis("Horizontal") >= 0.5f || Input.GetAxis("Horizontal") <= -0.5f)
            {
                if (!m_isRotating)
                {
                    StartCoroutine(ChangeDirection(Mathf.RoundToInt(Input.GetAxis("Horizontal")) * 45));
                    Debug.Log("Rotating");
                }
            }

            if (!paused)
            {
                m_hue += hueSpeed * Time.deltaTime;
                if(m_hue > 179.5f)
                {
                    m_hue = -179.5f;
                }
                else if(m_hue < -179.5f)
                {
                    m_hue = 179.5f;
                }

                var grading = profile.colorGrading.settings;
                grading.basic.hueShift = m_hue;
                profile.colorGrading.settings = grading;
            }

            newCamRot.x = Mathf.Lerp(newCamRot.x, 15f, 0.05f);

            targetFov = Mathf.Lerp(minFov, maxFov, (m_speed / maxSpeed));
        }
        else
        {
            newCamRot.x = Mathf.Lerp(newCamRot.x, 0f, 0.05f);
            targetFov = 45f;
        }
        m_cam.transform.localEulerAngles = newCamRot;

        targetSpeed = Mathf.Lerp(m_speed, decayedScore + 21, 0.9f);

        targetSpeed = Mathf.Clamp(targetSpeed, 0, maxSpeed);
        if(m_speed < targetSpeed)
        {
            rb.AddForce(transform.forward * targetSpeed * Time.deltaTime, ForceMode.Impulse);
        }

        m_speed = rb.velocity.z;
        speedText.text = Mathf.FloorToInt(m_speed) - 1 + "MPH";

        m_cam.fov = Mathf.Lerp(m_cam.fov, targetFov, 0.05f);



        if(m_speed <= 150f)
        {
            surfaceMat.SetTextureScale("_MainTex", new Vector2(3f, 0.5f));
        }
        else if(m_speed <= 300f)
        {
            surfaceMat.SetTextureScale("_MainTex", new Vector2(3f, 0.25f));
        }
        else
        {
            surfaceMat.SetTextureScale("_MainTex", new Vector2(3f, 0.125f));
        }

        if (paused)
        {
            surfaceMat.SetTextureScale("_MainTex", new Vector2(3f, 0.5f));
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