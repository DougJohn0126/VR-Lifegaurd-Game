using UnityEngine;
using TMPro;

public class ResultCanvas : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_YourGuessText;
    [SerializeField]
    private TextMeshProUGUI m_AnswerText;
    [SerializeField]
    private PlayerController m_PlayerController;

    private Canvas mCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mCanvas = this.GetComponent<Canvas>();
        mCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        mCanvas.enabled = true;
    }

    public void SetResultText(int answerNumber)
    {
        this.gameObject.SetActive(true);
        m_YourGuessText.text = "Your guess: " + m_PlayerController.GetPlayerGuessCount();
        m_AnswerText.text = "Answer: " + answerNumber;
        Time.timeScale = 0;
    }

}
