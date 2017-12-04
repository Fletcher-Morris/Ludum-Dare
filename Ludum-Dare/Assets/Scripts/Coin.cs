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
    private float r = 0;

    void Start()
    {
        r = Random.Range(0.0f, 100.0f);
    }

    void Update()
    {
        Vector3 newPos = Vector3.zero;
        newPos.z = Mathf.Sin((Time.time + (r/100f)) * bobSpeed) * bobHeight + bobHeight/2;
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
            StartCoroutine(DestroyCoin(other.gameObject));
        }
    }

    public void Reset()
    {
        cube1.GetComponent<Renderer>().enabled = true;
        cube2.GetComponent<Renderer>().enabled = true;
        isDestroyed = false;
    }

    public IEnumerator DestroyCoin(GameObject player)
    {
        isDestroyed = true;

        transform.parent = player.transform;

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