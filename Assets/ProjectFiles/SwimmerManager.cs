using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SwimmerManager : MonoBehaviour
{
    [SerializeField]
    private float m_MinimumSpawnInterval;

    [SerializeField]
    private float m_MaximumSpawnInterval;

    [SerializeField]
    private int m_MaxSpawnAtOnce;

    [SerializeField]
    private float m_GameTime;

    [SerializeField]
    private PlayerController m_PlayerController;

    [SerializeField]
    private ResultCanvas m_ResultCanvas;

    [SerializeField]
    private TextMeshProUGUI m_TimeleftText;

    [SerializeField]
    private List<GameObject> AllSwimmers;

    private float TimeRemaining;
    public bool TimerIsRunning = false;
    public bool GameIsEnd = false;
    private float mSpawnInterval;

    private List<SplineFollower> mSwimmerFollowers = new List<SplineFollower>();
    public int SwimmerCount;
    private List<string> mStringList = new List<string> { "goSwim", "goHop", "goTread", "goToEdge" };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( TimerIsRunning )
        {
            if (TimeRemaining > 0 )
            {
                TimeRemaining -= Time.deltaTime;
                m_TimeleftText.text = TimeRemaining.ToString("F2");
                mSpawnInterval -= Time.deltaTime;
                if (mSpawnInterval < 0)
                {
                    mSpawnInterval = Random.Range(m_MinimumSpawnInterval, m_MaximumSpawnInterval);
                    for (int i = 1; i <= m_MaxSpawnAtOnce; i ++)
                    {
                        GameObject swimmer = Instantiate(AllSwimmers[Random.RandomRange(0, AllSwimmers.Count - 1)], new Vector3(17.6000004f, 2.45000005f, 24.8600006f), Quaternion.identity);
                        mSwimmerFollowers.Add(swimmer.GetComponentInChildren<SplineFollower>());
                    }
                    
                }
            } else
            {
                TimerIsRunning = false;
                GameIsEnd = true;
            }

        }
        if (GameIsEnd)
        {
            m_ResultCanvas.SetResultText(this.GetAllSwimmersInPool());
            m_ResultCanvas.Show();
            GameIsEnd = false;
        }

    }

    public void StartGame()
    {
        TimeRemaining = m_GameTime;
        mSpawnInterval = Random.Range(m_MinimumSpawnInterval, m_MaximumSpawnInterval);
        TimerIsRunning = true;
        m_PlayerController.SetGameIsRunning();
    }

    public int GetAllSwimmersInPool()
    {
        int result = 0;
        foreach(SplineFollower swimmer in mSwimmerFollowers)
        {
            string currentAnim = swimmer.GetCurrentAnimation();
            if (mStringList.Contains(currentAnim))
            {
                result++; 
            }
            
         }
        return result;
    }
    
}
