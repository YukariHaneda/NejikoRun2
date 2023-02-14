using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
        const int MinLane = -2;
        const int MaxLane = 2;
        const float LaneWidth = 1.0f;
        const int DefaultLife = 3;
        const float StunDuration = 0.5f;

        CharacterController controller;
        Animator animator;

        Vector3 moveDirection = Vector3.zero;
        int targetLane;
        int life = DefaultLife;
        float recoverTime = 0.0f;

        public float gravity;
        public float speedZ;
        public float speedX;//横方向
        public float speedJump;
        public float accelerationZ;//加速度

        public int Life() {
            return life;
        }

        bool IsStun() {
            return recoverTime > 0.0f || life <= 0;
        }

    // Start is called before the first frame update
    void Start()
    {
        //必要なコンポーネントを自動取得
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //デバック用
        if(Input.GetKeyDown("left")) MoveToLeft();
        if(Input.GetKeyDown("right")) MoveToRight();
        if(Input.GetKeyDown("space")) Jump();

        if (IsStun()) {
            //動きを止め気絶状態からの復帰カウントを進める
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;
            recoverTime -= Time.deltaTime;
        }else {
            //徐々に加速しZ方向に常に前身させる
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
            moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);//speedZに達したらその後は低速度

            //x方向は目標のポジションまでの差分の割合で速度を計算
            float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = ratioX * speedX;
        }

        
        /*
        //地面に接触しているときの処理
        //controller.isGrounded：足元が地面に接触したかを判定する。地面がまっ平でないと使えない（判定があてにならないから）。
        if(controller.isGrounded) {
            if(Input.GetAxis("Vertical") > 0.0f) {
                moveDirection.z = Input.GetAxis("Vertical") * speedZ;//押していたらzのプラス方向に進む

            }else {
                moveDirection.z = 0;
            }

            transform.Rotate(0,Input.GetAxis("Horizontal") * 3, 0); //y軸回転する

            if (Input.GetButton("Jump")) {//上方向を押されたら
                moveDirection.y = speedJump;//y(上)の方向に行く
                animator.SetTrigger("jump");//animator.SetTrigger("jump")でtrueになってジャンプの動きをする
            }
        }
        */

        //地面に接触していないときの処理
        //重力分の力を毎フレーム追加
        moveDirection.y -= gravity * Time.deltaTime;

        //移動実行
        Vector3 globalDirection = transform.TransformDirection(moveDirection);//ねじこの向きを考慮した座標に変換する
        controller.Move(globalDirection * Time.deltaTime);//Time.deltaTimeを掛け算することで、1フレームではなく、1秒間あたりの移動量に変換

        //移動後接地してたらy方向の速度がリセットする
        if(controller.isGrounded) moveDirection.y = 0;

        //速度が0以上から走っているフラグをtrueにする
        animator.SetBool("run",moveDirection.z > 0.0f);
    }

    //左レーンに移動を開始
    public void MoveToLeft() {
        if(IsStun()) return;
        if(controller.isGrounded && targetLane > MinLane) targetLane--;
    }

    //右のレーンに移動を開始
    public void MoveToRight() {
        if(IsStun()) return;
        if(controller.isGrounded && targetLane < MaxLane) targetLane++;
    }

    public void Jump() {
        if(IsStun()) return;
        if(controller.isGrounded) {
            moveDirection.y = speedJump;

            //ジャンプトリガーを設定
            animator.SetTrigger("jump");
        }
    }

    //CharactoeControllerに衝突判定が生じたときの処理
    void OnControllerColliderHit(ControllerColliderHit hit) {
        if(IsStun()) return;

        if(hit.gameObject.tag == "Robo") {
            //ライフを減らして気絶状態に移行
            life--;
            recoverTime = StunDuration;

            //ダメージトリガーを設定
            animator.SetTrigger("damage");

            //ヒットしたオブジェクトは削除
            Destroy(hit.gameObject);
        }
    }
}

