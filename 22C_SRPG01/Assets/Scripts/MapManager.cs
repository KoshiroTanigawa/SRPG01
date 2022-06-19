using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform blockParent; // マップブロックの親オブジェクトのTransform
    public GameObject blockPrefab_Metal1; 
    public GameObject blockPrefab_Metal2; 

	// マップデータ
	public MapBlock[,] mapBlocks;


	// 定数定義
	public const int MAP_WIDTH = 9; // マップの横幅
    public const int MAP_HEIGHT = 9; // マップの縦(奥行)の幅
    private const int GENERATE_RATIO_METAL = 90; // 金属ブロックが生成される確率

    void Start()
    {
		// マップデータを初期化
		mapBlocks = new MapBlock[MAP_WIDTH, MAP_HEIGHT];

		// ブロック生成位置の基点となる座標を設定
		Vector3 defaultPos = new Vector3(0.0f, 0.0f, 0.0f); // x:0.0f y:0.0f z:0.0f のVector3変数defaultPosを宣言
        defaultPos.x = -(MAP_WIDTH / 2); // x座標の基点
        defaultPos.z = -(MAP_HEIGHT / 2); // z座標の基点

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
				bool isGrass = false; // 草ブロック生成フラグ(初期状態はfalse)
									  // 乱数値が草ブロック確率値より低ければ草ブロックを生成する
				if (rand < GENERATE_RATIO_METAL)
					isGrass = true;

				// オブジェクトを生成
				GameObject obj; // 生成するオブジェクトの参照
				if (isGrass)
				{// 草ブロック生成フラグ：ON
					obj = Instantiate(blockPrefab_Metal1, blockParent); // blockParentの子に草ブロックを生成
				}
				else
				{// 草ブロック生成フラグ：OFF
					obj = Instantiate(blockPrefab_Metal2, blockParent); // blockParentの子に水場ブロックを生成
				}
				// オブジェクトの座標を適用
				obj.transform.position = pos;

				// 配列mapBlocksにブロックデータを格納
				var mapBlock = obj.GetComponent<MapBlock>(); // オブジェクトのMapBlockを取得
				mapBlocks[i, j] = mapBlock;
				// ブロックデータ設定
				mapBlock.xPos = (int)pos.x; // X位置を記録
				mapBlock.zPos = (int)pos.z; // Z位置を記録
			}
		}
	}

	/// <summary>
	/// 全てのブロックの選択状態を解除する
	/// </summary>
	public void AllSelectionModeClear()
	{
		for (int i = 0; i < MAP_WIDTH; i++)
			for (int j = 0; j < MAP_HEIGHT; j++)
				mapBlocks[i, j].SetSelectionMode(MapBlock.Highlight.Off);
	}

	/// <summary>
	/// 渡された位置からキャラクターが到達できる場所のブロックをリストにして返す
	/// </summary>
	/// <param name="xPos">基点x位置</param>
	/// <param name="zPos">基点z位置</param>
	/// <returns>条件を満たすブロックのリスト</returns>
	public List<MapBlock> SearchReachableBlocks(int xPos, int zPos)
	{
		// 条件を満たすブロックのリスト
		var results = new List<MapBlock>();

		// 基点となるブロックの配列内番号(index)を検索
		int baseX = -1, baseZ = -1; // 配列内番号(検索前は-1が入っている)
									// 検索処理
		for (int i = 0; i < MAP_WIDTH; i++)
		{
			for (int j = 0; j < MAP_HEIGHT; j++)
			{
				if ((mapBlocks[i, j].xPos == xPos) &&
					(mapBlocks[i, j].zPos == zPos))
				{// 指定された座標に一致するマップブロックを発見
				 // 配列内番号を取得してループを終了
					baseX = i;
					baseZ = j;
					break; // 2個目のループを抜ける
				}
			}
			// 既に発見済みなら1個目のループを抜ける
			if (baseX != -1)
				break;
		}

		// 移動するキャラクターの移動方法を取得
		var moveType = Character.MoveType.Rook; // 移動方法
		var moveChara = GetComponent<CharactersManager>().GetCharacterDataByPos(xPos, zPos); // 移動するキャラ
		if (moveChara != null)
			moveType = moveChara.moveType; // キャラクターデータから移動方法を取得

		// キャラクターの移動方法に合わせて異なる方向のブロックデータを取得していく
		// 縦・横
		if (moveType == Character.MoveType.Rook ||
			moveType == Character.MoveType.Queen)
		{
			// X+方向
			for (int i = baseX + 1; i < MAP_WIDTH; i++)
				if (AddReachableList(results, mapBlocks[i, baseZ]))
					break;
			// X-方向
			for (int i = baseX - 1; i >= 0; i--)
				if (AddReachableList(results, mapBlocks[i, baseZ]))
					break;
			// Z+方向
			for (int j = baseZ + 1; j < MAP_HEIGHT; j++)
				if (AddReachableList(results, mapBlocks[baseX, j]))
					break;
			// Z-方向
			for (int j = baseZ - 1; j >= 0; j--)
				if (AddReachableList(results, mapBlocks[baseX, j]))
					break;
		}
		// 斜めへの移動
		if (moveType == Character.MoveType.Bishop ||
			moveType == Character.MoveType.Queen)
		{
			// X+Z+方向
			for (int i = baseX + 1, j = baseZ + 1;
				i < MAP_WIDTH && j < MAP_HEIGHT;
				i++, j++)
				if (AddReachableList(results, mapBlocks[i, j]))
					break;
			// X-Z+方向
			for (int i = baseX - 1, j = baseZ + 1;
				i >= 0 && j < MAP_HEIGHT;
				i--, j++)
				if (AddReachableList(results, mapBlocks[i, j]))
					break;
			// X+Z-方向
			for (int i = baseX + 1, j = baseZ - 1;
				i < MAP_WIDTH && j >= 0;
				i++, j--)
				if (AddReachableList(results, mapBlocks[i, j]))
					break;
			// X-Z-方向
			for (int i = baseX - 1, j = baseZ - 1;
				i >= 0 && j >= 0;
				i--, j--)
				if (AddReachableList(results, mapBlocks[i, j]))
					break;
		}
		// 足元のブロック
		results.Add(mapBlocks[baseX, baseZ]);

		return results;
	}

	/// <summary>
	/// (キャラクター到達ブロック検索処理用)
	/// 指定したブロックを到達可能ブロックリストに追加する
	/// </summary>
	/// <param name="reachableList">到達可能ブロックリスト</param>
	/// <param name="targetBlock">対象ブロック</param>
	/// <returns>行き止まりフラグ(行き止まりならtrueが返る)</returns>
	private bool AddReachableList(List<MapBlock> reachableList, MapBlock targetBlock)
	{
		// 対象のブロックが通行不可ならそこを行き止まりとして終了
		if (!targetBlock.passable)
			return true;

		// 対象の位置に他のキャラが既にいるなら到達不可にして終了(行き止まりにはしない)
		var charaData =
			GetComponent<CharactersManager>().GetCharacterDataByPos(targetBlock.xPos, targetBlock.zPos);
		if (charaData != null)
			return false;

		// 到達可能ブロックリストに追加する
		reachableList.Add(targetBlock);
		return false;
	}

	/// <summary>
	/// 渡された位置からキャラクターが攻撃できる場所のマップブロックをリストにして返す
	/// </summary>
	/// <param name="xPos">基点x位置</param>
	/// <param name="zPos">基点z位置</param>
	/// <returns>条件を満たすマップブロックのリスト</returns>
	public List<MapBlock> SearchAttackableBlocks(int xPos, int zPos)
	{
		// 条件を満たすマップブロックのリスト
		var results = new List<MapBlock>();

		// 基点となるブロックの配列内番号(index)を検索
		int baseX = -1, baseZ = -1; // 配列内番号(検索前は-1が入っている)
									// 検索処理
		for (int i = 0; i < MAP_WIDTH; i++)
		{
			for (int j = 0; j < MAP_HEIGHT; j++)
			{
				if ((mapBlocks[i, j].xPos == xPos) &&
					(mapBlocks[i, j].zPos == zPos))
				{// 指定された座標に一致するマップブロックを発見
				 // 配列内番号を取得してループを終了
					baseX = i;
					baseZ = j;
					break; // 2個目のループを抜ける
				}
			}
			// 既に発見済みなら1個目のループを抜ける
			if (baseX != -1)
				break;
		}

		// 4方向に1マス進んだ位置のブロックをそれぞれセット
		// (縦・横4マス)
		// X+方向
		AddAttackableList(results, baseX + 1, baseZ);
		// X-方向
		AddAttackableList(results, baseX - 1, baseZ);
		// Z+方向
		AddAttackableList(results, baseX, baseZ + 1);
		// Z-方向
		AddAttackableList(results, baseX, baseZ - 1);
		// (斜め4マス)
		// X+Z+方向
		AddAttackableList(results, baseX + 1, baseZ + 1);
		// X-Z+方向
		AddAttackableList(results, baseX - 1, baseZ + 1);
		// X+Z-方向
		AddAttackableList(results, baseX + 1, baseZ - 1);
		// X-Z-方向
		AddAttackableList(results, baseX - 1, baseZ - 1);

		return results;
	}

	/// <summary>
	/// (キャラクター攻撃可能ブロック検索処理用)
	/// マップデータの指定された配列内番号に対応するブロックを攻撃可能ブロックリストに追加する
	/// </summary>
	/// <param name="attackableList">攻撃可能ブロックリスト</param>
	/// <param name="indexX">X方向の配列内番号</param>
	/// <param name="indexZ">Z方向の配列内番号</param>
	private void AddAttackableList(List<MapBlock> attackableList, int indexX, int indexZ)
	{
		// 指定された番号が配列の外に出ていたら追加せず終了
		if (indexX < 0 || indexX >= MAP_WIDTH ||
			indexZ < 0 || indexZ >= MAP_HEIGHT)
			return;

		// 到達可能ブロックリストに追加する
		attackableList.Add(mapBlocks[indexX, indexZ]);
	}

	/// <summary>
	/// マップデータ配列をリストにして返す
	/// </summary>
	/// <returns>マップデータのリスト</returns>
	public List<MapBlock> MapBlocksToList()
	{
		// 結果用リスト
		var results = new List<MapBlock>();

		// マップデータ配列の中身を順番にリストに格納
		for (int i = 0; i < MAP_WIDTH; i++)
		{
			for (int j = 0; j < MAP_HEIGHT; j++)
			{
				results.Add(mapBlocks[i, j]);
			}
		}

		return results;
	}

}
