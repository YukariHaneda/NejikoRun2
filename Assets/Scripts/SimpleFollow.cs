using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
        Vector3 diff;

        public GameObject target;
        public float followSpeed;

    // Start is called before the first frame update
    void Start()
    {
        diff = target.transform.position - transform.position; //メインカメラからねじこへのベクトル
    }

    // Update is called once per frame
    void LateUpdate()//Updateが終わったあとに動くのがLateUpdate関数
    {
        transform.position = Vector3.Lerp (
            transform.position,//今いるカメラのポジション
            target.transform.position -diff,//今のねじこに対するカメラのポジション
            Time.deltaTime * followSpeed //0~1の範囲になる。
            //ねじこが動いてちょっと後に動く。このちょいずれがいらなければねじこの子要素にメインカメラを持ってくる。
            //第3引数が0.5だった場合、第1引数と第2引数の間のポジションを返す
            //同じ50%でも距離が遠いと最初は速くて、ゴールに近づいたらゆっくりになる。
        );
    }
}
