using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour {
    TMP_Text scoretext;
    float score = 0f;
    EnemyController enemyController;

    public int Score { get; set; }

    void Awake() {
        scoretext = GetComponent<TMP_Text>();
    }

    void UpdateBoard() {
        if(GameManager.i.CurrentGameState == GameStates.Playing) {
            scoretext.text = $"Score: {score}";
        }
    }

    public void IncreaseScore(float points) {
        score += points;
        UpdateBoard();
    }

    public void ResetScore() {
        score = 0;
        UpdateBoard();
    }

    void OnEnable() {
        EnemyController.OnDeath += IncreaseScore;
    }

    void OnDisable() {
        EnemyController.OnDeath -= IncreaseScore;
    }
}
