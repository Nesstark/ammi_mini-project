using UnityEngine;
using UnityEngine.UI;
using TMPro;

// This code was made in collaboration with ChatGPT

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    [Header("Game State")]
    public int score = 0;
    public int health = 3;

    [Header("UI")]
    public TMP_Text scoreText;       // TextMeshPro score
    public Image[] heartImages;      // Heart icons for HP
    public TMP_Text finalScoreText;  // TMP for final score display

    [Header("Disable on Death")]
    public GameObject lightControllerObject; // LightController
    public GameObject boxParentObject;       // Box parent with all buttons

    private void Awake()
    {
        Instance = this;
        UpdateUI();

        // Start hidden
        if (finalScoreText != null) finalScoreText.gameObject.SetActive(false);
        if (scoreText != null) scoreText.gameObject.SetActive(true);
    }

    public void AddPoint()
    {
        score++;
        UpdateUI();
    }

    public void WrongLamp()
    {
        health--;
        UpdateUI();
        CheckDeath();
    }

    public void MissedLamp()
    {
        health--;
        UpdateUI();
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            // Stop LightController og Box interaktion
            if (lightControllerObject != null) lightControllerObject.SetActive(false);
            if (boxParentObject != null) boxParentObject.SetActive(false);

            ShowFinalScore();
        }
    }

    private void ShowFinalScore()
    {
        // Skjul den normale score
        if (scoreText != null)
            scoreText.gameObject.SetActive(false);

        // Vis final score
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + score.ToString();
            finalScoreText.gameObject.SetActive(true);
        }

        Debug.Log("Player is dead! Final Score: " + score);
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] != null)
                heartImages[i].enabled = (i < health);
        }
    }
}
