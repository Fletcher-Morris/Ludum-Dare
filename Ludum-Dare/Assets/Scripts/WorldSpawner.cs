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

    private int m_segmentCount = 0;

    void Awake()
    {
        CreatePool();

        for(int i = 0; i < 6; i++)
        {
            GenerateNextSection();
        }
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

    public GameObject SpawnRegularSegment(Vector3 position)
    {
        for (int i = 0; i < m_regSegPool.Count; i++)
        {
            if (!m_regSegPool[i].activeInHierarchy)
            {
                GameObject segment = m_regSegPool[i];
                segment.transform.position = position;
                segment.SetActive(true);

                return segment;
            }
        }

        return null;
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





    public void GenerateNextSection()
    {
        GameObject[] segments = new GameObject[8];
        for(int i = 0; i < 8; i++)
        {
            float segY = 0f;
            float segX = 0f;
            Vector2 segXY = Vector2.zero;
            Vector3 segRot = Vector3.zero;

            //if(i == 0)
            //    segY = 12f;
            //else if(i == 1 | i == 7)
            //    segY = 8.5f;
            //else if(i == 2 || i == 6)
            //    segY = 0f;
            //else if (i == 3 || i == 5)
            //    segY = -8.5f;
            //else if (i == 4)
            //    segY = -12f;

            //if (i == 0 || i == 4)
            //    segX = 0f;
            //else if (i == 1 | i == 3)
            //    segX = 8.5f;
            //else if (i == 2)
            //    segX = 12f;
            //else if (i == 5 || i == 7)
            //    segX = -8.5f;
            //else if (i == 6)
            //    segX = -12f;

            if (i == 0)
                segXY = new Vector2(0, 1).normalized;
            else if (i == 1)
                segXY = new Vector2(1, 1).normalized;
            else if (i == 2)
                segXY = new Vector2(1, 0).normalized;
            else if (i == 3)
                segXY = new Vector2(1, -1).normalized;
            else if (i == 4)
                segXY = new Vector2(0, -1).normalized;
            else if (i == 5)
                segXY = new Vector2(-1, -1).normalized;
            else if (i == 6)
                segXY = new Vector2(-1, 0).normalized;
            else if (i == 7)
                segXY = new Vector2(-1, 1).normalized;

            if (i == 0)
                segRot = new Vector3(90, 0, 180);
            else if (i == 1)
                segRot = new Vector3(45, -90, 90);
            else if (i == 2)
                segRot = new Vector3(180, 90, 90);
            else if (i == 3)
                segRot = new Vector3(-135, 90, 90);
            else if (i == 4)
                segRot = new Vector3(-90, 0, 0);
            else if (i == 5)
                segRot = new Vector3(-135, -90, 90);
            else if (i == 6)
                segRot = new Vector3(180, -90, 90);
            else if (i == 7)
                segRot = new Vector3(135, -90, 90);

            segments[i] = SpawnRegularSegment(new Vector3(segXY.x * 12.05f, segXY.y * 12.05f, m_segmentCount * 30));
            segments[i].transform.eulerAngles = segRot;
        }

        m_segmentCount++;
    }
}