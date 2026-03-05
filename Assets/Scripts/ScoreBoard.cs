using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    TMP_Text scoretext;
    int score = 0;

    public int Score { get; set; }

    void Awake()
    {
        scoretext = GetComponent<TMP_Text>();
    } 

    void UpdateBoard(){
        if(GameManager.i.CurrentGameState == GameStates.Playing){
            scoretext.text = $"Score: {score}";
        }
    }
}
