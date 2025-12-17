using UnityEngine;

public class GameplaySwimmer : MonoBehaviour
{
    [SerializeField]
    private Animator m_Animator;
    private SplineGroup m_SplineGroup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public void SetAnimation(string animTrigger)
    {
        m_Animator.SetTrigger(animTrigger);
    }
}
