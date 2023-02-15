using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public Text highScoreText;

    public void Start() {
        //ハイスコアを表示
        highScoreText.text = "High Score ：" + PlayerPrefs.GetInt("HighScore") + "m";
    }

    public void OnStartButtonClicked() {
        SceneManager.LoadScene("Main");//startボタンが押されたらシーンをメインに遷移する処理
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
