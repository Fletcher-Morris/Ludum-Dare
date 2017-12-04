using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject cube1, cube2;
    public ParticleSystem emitter;

    public float bobSpeed = 1.0f, bobHeight = 0.2f, spinSpeed = 1.0f;

    public int value;
    [HideInInspector]
    public WorldSpawner worldSpawner;
    private bool isDestroyed = false;

    void Start()
    {

    }

    void Update()
    {
        Vector3 newPos = Vector3.zero;
        newPos.y = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        cube1.transform.localPosition = newPos;
        cube2.transform.localPosition = newPos;
        cube1.transform.Rotate(Vector3.right * spinSpeed * Time.deltaTime);
        cube2.transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDestroyed)
        {
            other.GetComponent<Player>().AddCoin(value);
            StartCoroutine(DestroyCoin());
        }
    }

    public IEnumerator DestroyCoin()
    {
        isDestroyed = true;

        if (!worldSpawner)
        {
            worldSpawner = GameObject.Find("World Spawner").GetComponent<WorldSpawner>();
        }

        cube1.GetComponent<Renderer>().enabled = false;
        cube2.GetComponent<Renderer>().enabled = false;
        emitter.Emit(30);
        yield return new WaitForSeconds(1.1f);
        worldSpawner.DestroyCoin(gameObject);
        yield return null;
    }
}