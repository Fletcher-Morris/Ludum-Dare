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

    public int segmentPoolSize = 200;
    public int coinPoolSize = 50;

    private int m_segmentCount = 0;
    private GameObject m_player;
    private int m_oldestSegmentPos = 0;
    private int m_newestSegmentPos = 0;
    private int m_generatedSections = 0;

    void Awake()
    {
        m_player = GameObject.Find("Player");

        CreatePool();

        for (int i = 0; i < 20; i++)
        {
            GenerateNextSection();
        }
    }

    void FixedUpdate()
    {
        if(m_player.transform.position.z + 500 > m_newestSegmentPos)
        {
            m_oldestSegmentPos = Mathf.RoundToInt(m_player.transform.position.z) - 60;

            foreach (GameObject segment in m_regSegPool)
            {
                if(segment.transform.position.z <= m_oldestSegmentPos && segment.activeInHierarchy)
                {
                    DestroyRegularSegment(segment);
                }
            }

            foreach (GameObject coin in m_coinPool)
            {
                if (coin.transform.position.z <= m_oldestSegmentPos + 30 && coin.activeInHierarchy)
                {
                    DestroyRegularSegment(coin);
                }
            }



            if(m_generatedSections < segmentPoolSize - 8)
            {
                GenerateNextSection();
            }
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
            m_coinPool.Add(obj);
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
    public void DestroyRegularSegment(GameObject segment)
    {
        segment.SetActive(false);
        m_generatedSections--;
        m_generatedSections = Mathf.Clamp(m_generatedSections, 0, segmentPoolSize);
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
                coin.GetComponent<Coin>().Reset();

                return coin;
            }
        }

        return null;
    }
    public void DestroyCoin(GameObject coin)
    {
        coin.transform.parent = null;
        coin.SetActive(false);
    }





    public void GenerateNextSection()
    {
        for(int i = 0; i < 8; i++)
        {
            Vector2 segXY = Vector2.zero;
            Vector3 segRot = Vector3.zero;

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

            GameObject segment = SpawnRegularSegment(new Vector3(segXY.x * 12.05f, segXY.y * 12.05f, m_segmentCount * 30));
            segment.transform.eulerAngles = segRot;
            m_generatedSections++;
            m_generatedSections = Mathf.Clamp(m_generatedSections, 0, segmentPoolSize);
        }

        if(Random.Range(0,3) == 0)
        {
            Vector2 coinXY = Vector2.zero;
            int j = Random.Range(0, 7);
            if (j == 0)
                coinXY = new Vector2(0, 1).normalized;
            else if (j == 1)
                coinXY = new Vector2(1, 1).normalized;
            else if (j == 2)
                coinXY = new Vector2(1, 0).normalized;
            else if (j == 3)
                coinXY = new Vector2(1, -1).normalized;
            else if (j == 4)
                coinXY = new Vector2(0, -1).normalized;
            else if (j == 5)
                coinXY = new Vector2(-1, -1).normalized;
            else if (j == 6)
                coinXY = new Vector2(-1, 0).normalized;
            else if (j == 7)
                coinXY = new Vector2(-1, 1).normalized;


            Vector3 coinRot = Vector3.zero;
            if (j == 0)
                coinRot = new Vector3(90, 0, 180);
            else if (j == 1)
                coinRot = new Vector3(45, -90, 90);
            else if (j == 2)
                coinRot = new Vector3(180, 90, 90);
            else if (j == 3)
                coinRot = new Vector3(-135, 90, 90);
            else if (j == 4)
                coinRot = new Vector3(-90, 0, 0);
            else if (j == 5)
                coinRot = new Vector3(-135, -90, 90);
            else if (j == 6)
                coinRot = new Vector3(180, -90, 90);
            else if (j == 7)
                coinRot = new Vector3(135, -90, 90);

            GameObject newCoin = SpawnCoin(new Vector3(coinXY.x * 11f, coinXY.y * 11f, m_segmentCount * 30));
            newCoin.transform.eulerAngles = coinRot;
        }

        m_newestSegmentPos = m_segmentCount * 30;

        m_segmentCount++;
    }
}