using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpawner : MonoBehaviour
{
    public GameObject regularSegment;
    public GameObject boostSegment;
    public GameObject smallBlocker;
    public GameObject mediumBlocker;
    public GameObject largeBlocker;
    public GameObject coin;

    private List<GameObject> m_regSegPool;
    private List<GameObject> m_medBlockerPool;
    private List<GameObject> m_coinPool;

    public int segmentPoolSize = 50;
    public int coinPoolSize = 50;

    void Awake()
    {
        CreatePool();
    }

    void CreatePool()
    {
        m_regSegPool = new List<GameObject>();
        m_coinPool = new List<GameObject>();

        for (int i = 0; i < segmentPoolSize; i++)
        {
            GameObject obj = GameObject.Instantiate(regularSegment);
            obj.SetActive(false);
            m_regSegPool.Add(obj);
        }
        for (int i = 0; i < coinPoolSize; i++)
        {
            GameObject obj = GameObject.Instantiate(coin);
            coin.GetComponent<Coin>().worldSpawner = this;
            obj.SetActive(false);
            m_regSegPool.Add(obj);
        }
    }

    public GameObject SpawnCoin(Vector3 position)
    {
        for(int i = 0; i < m_coinPool.Count; i++)
        {
            if (!m_coinPool[i].activeInHierarchy)
            {
                GameObject coin = m_coinPool[i];
                coin.transform.position = position;
                coin.SetActive(true);

                return coin;
            }
        }

        return null;
    }

    public void DestroyCoin(GameObject coin)
    {
        coin.SetActive(false);
    }
}