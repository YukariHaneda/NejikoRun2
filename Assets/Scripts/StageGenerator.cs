using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    const int StageChipSize = 30;

    int currentChipIndex;//生成済みの先頭チップのindex

    public Transform character;
    public GameObject[] stageChips;
    public int startChipIndex;
    public int preInstantiate;//生成先読み個数
    public List<GameObject> generatedStageList = new List<GameObject>();//生成したchipの住所を管理するため
    
    // Start is called before the first frame update
    void Start()
    {
        currentChipIndex = startChipIndex - 1;//インスタンス０まで生成
        UpdateStage(preInstantiate);
    }

    // Update is called once per frame
    void Update()
    {
        //キャラクターの位置から現在のステージチップのインデックスを計算
        int charaPositionIndex = (int)(character.position.z / StageChipSize);
        //ねじこのz座標をStageChipSize30で割るとそのステージindexかを算出できる

        //次のステージチップに入ったらステージの更新処理をおこなう
        //charaPositionIndexが1なら6つ目に作る
        if (charaPositionIndex + preInstantiate > currentChipIndex) {
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }

    //新しいのを1つ生成し、古いのを1つ削除するメソッド
    //指定のIndexまでのステージチップを生成して、管理化に置く
    void UpdateStage(int toChipIndex) {
        if(toChipIndex <= currentChipIndex) return;
        
        //指定のステージチップまでを作成
        for(int i = currentChipIndex + 1; i <= toChipIndex; i++) {//1,2,3,4,5までの地面を作っていく
            GameObject stageObject = GenerateStage(i);

            //生成したステージチップを管理リストに追加
            generatedStageList.Add(stageObject);//参照住所を追加。5回行われる
        }

        //ステージ保持上限内になるまで古いステージを削除
        while(generatedStageList.Count > preInstantiate + 2)//7個以上行ったら古いのを消す
        DestroyOldestStage();

        currentChipIndex = toChipIndex;
    }

    //指定のインデックス位置にStageオブジェクトをランダムに生成
    GameObject GenerateStage(int chipIndex) {
        int nextStageChip = Random.Range(0, stageChips.Length);

        GameObject stageObject = (GameObject)Instantiate(
            stageChips[nextStageChip],//最初は０
            new Vector3(0, 0, chipIndex * StageChipSize),//最初は０,0,30の場所にチップが1つできる
            Quaternion.identity
        );

        return stageObject;
    }

    //一番古いステージを削除
    void DestroyOldestStage() {
        GameObject oldStage = generatedStageList[0];//Listの最初のinndex(一番古いstage)をチョイス
        generatedStageList.RemoveAt(0);//Listから消す
        Destroy(oldStage);//実際にそのStageオブジェクトをゲーム画面から消す
    }
}
