using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform blockParent; // マップブロックの親オブジェクトのTransform
    public GameObject blockPrefab_MossStone; // 苔石ブロック
	public GameObject blockPrefab_HardStone; // 硬い石ブロック
	public GameObject blockPrefab_DeepWater; // 水ブロック

	// 定数を定義
	public const int MAP_WIDTH = 15; // マップの横幅
	public const int MAP_HEIGHT = 15; // マップの縦幅
	public const int  genarateMossStone = 80; // 苔石ブロックが生成される確率
	public const int generateHardStone = 10; // 硬い石ブロックが生成される確率

	void Start()
    {
        Vector3 defaultPos = DefaultPos();
        Generateblock(defaultPos);
    }

    private static Vector3 DefaultPos()
    {
        // ブロック生成位置の基点となる座標を設定
        Vector3 defaultPos = new Vector3(0f, 0f, 0f);
        defaultPos.x = -(MAP_WIDTH / 2); // x座標の基点
        defaultPos.z = -(MAP_HEIGHT / 2); // z座標の基点
        return defaultPos;
    }

    private void Generateblock(Vector3 defaultPos)
    {
        // ブロック生成処理
        for (int i = 0; i < MAP_WIDTH; i++)
        {// マップの横幅分繰り返し処理
            for (int j = 0; j < MAP_HEIGHT; j++)
            {// マップの縦幅分繰り返し処理
             // ブロックの場所を決定
                Vector3 pos = defaultPos; // 基点の座標を元に変数posを宣言
                pos.x += i; // 1個目のfor分の繰り返し回数分x座標をずらす
                pos.z += j; // 2個目のfor分の繰り返し回数分z座標をずらす

                // ブロックの種類を決定
                int rand = Random.Range(0, 100); // 0~99の中から1つランダムな数字を取得

                // オブジェクトを生成
                GameObject obj; // 生成するオブジェクトの参照

                if (rand < genarateMossStone && rand > generateHardStone)
                {
                    
                    obj = Instantiate(blockPrefab_MossStone, blockParent); // blockParentの子に草ブロックを生成
                }

                else if ( rand <generateHardStone )
                {
                    obj = Instantiate(blockPrefab_HardStone, blockParent); // blockParentの子に硬い石ブロックを生成
                }

                else
                {
                    obj = Instantiate(blockPrefab_DeepWater, blockParent); // blockParentの子に硬い石ブロックを生成
                }

                obj.transform.position = pos; // オブジェクトの座標を適用
            }
        }
    }
}
