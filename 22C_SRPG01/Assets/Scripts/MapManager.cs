using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform blockParent; // マップブロックの親オブジェクトのTransform
    public GameObject blockPrefab_MossStone; // 苔石ブロック
    public GameObject blockPrefab_HardStone; // 石ブロック
    public GameObject blockPrefab_Grass; // 草ブロック

	// 定数を定義
	public const int MAP_WIDTH = 11; // マップの横幅
	public const int MAP_HEIGHT = 11; // マップの縦幅
    private int generateHardStone = 20; // 石ブロックが生成される確率
    private int generateGrass = 20; // 草ブロックが生成される確率


    void Start()
    {
        Vector3 defaultPos = DefaultPos();
        Generateblock( defaultPos );
    }

    private static Vector3 DefaultPos()
    {
        // ブロック生成位置の基点となる座標を設定
        Vector3 defaultPos = new Vector3(0f, 0f, 0f);
        defaultPos.x = -( MAP_WIDTH / 2 ); // x座標の基点
        defaultPos.z = -( MAP_HEIGHT / 2 ); // z座標の基点
        return defaultPos;
    }

    private void Generateblock(Vector3 defaultPos)
    {
        // ブロック生成処理
        for ( int i = 0; i < MAP_WIDTH; i++)// マップの横幅分繰り返し処理
        {
            for ( int j = 0; j < MAP_HEIGHT; j++)// マップの縦幅分繰り返し処理
            {
                Vector3 pos = defaultPos; 
                pos.x += i; // 1個目のfor分の繰り返し回数分x座標をずらす
                pos.z += j; // 2個目のfor分の繰り返し回数分z座標をずらす

                // ブロックの種類を決定する確率の乱数
                int rand = Random.Range(0, 100); 

                // オブジェクトを生成
                GameObject obj; 

                if ( rand <generateHardStone )
                {
                    
                    obj = Instantiate(blockPrefab_HardStone, blockParent); // blockParentの子に石ブロックを生成する
                }

                else if ( rand > generateHardStone && rand < generateHardStone + generateGrass )
                {
                    obj = Instantiate(blockPrefab_Grass, blockParent); // blockParentの子に草ブロックを生成する
                }

                else
                {
                    obj = Instantiate(blockPrefab_MossStone, blockParent); // blockParentの子に苔石ブロックを生成する
                }

                obj.transform.position = pos; // オブジェクトの座標を適用
            }
        }
    }
}
