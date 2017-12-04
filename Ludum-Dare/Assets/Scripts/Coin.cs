﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject cube1, cube2;
    public ParticleSystem emitter;

    public float bobSpeed = 1.0f, bobHeight = 0.2f, spinSpeed = 1.0f;

    public int value;

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
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().AddCoin(value);
            StartCoroutine(DestroyCoin());
        }
    }

    public IEnumerator DestroyCoin()
    {
        cube1.GetComponent<Renderer>().enabled = false;
        cube2.GetComponent<Renderer>().enabled = false;
        emitter.Emit(30);
        Destroy(gameObject,1f);
        yield return null;
    }
}