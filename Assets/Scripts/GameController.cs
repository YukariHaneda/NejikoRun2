using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public NejikoController nejiko;
    public Text scoreText;
    public LifePanel lifePanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int score = CalcScore();
        scoreText.text = "Score : " + score + "m";

        //ライフパネルを更新
        lifePanel.UpdateLife(nejiko.Life());
    }

    int CalcScore() {
        //ねじこの走行距離をスコアとする
        return (int)nejiko.transform.position.z;
    }
}
