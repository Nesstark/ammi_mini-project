using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    public int score = 0;
    public int health = 3;

    [Header("UI")]
    public TMP_Text scoreText;      // TextMeshPro score
    public Image[] heartImages;     // Heart icons for HP

    [Header("Player")]
    public GameObject playerObject;

    private void Awake()
    {
        Instance = this;
        UpdateUI();
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
            playerObject.SetActive(false);
            Debug.Log("Player is dead!");
        }
    }

    private void UpdateUI()
    {
        // Update score text
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();

        // Update hearts
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = (i < health);
        }
    }
}
