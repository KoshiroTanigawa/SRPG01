using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	//フィールド変数
    [SerializeField]Transform _blockParent;
	[SerializeField] GameObject _blockPrefab_Metal;
	[SerializeField] MapBlock[,] _mapBlocks;

	[Tooltip("マップの横幅")] int _mapWidth = 9;
	[Tooltip("マップの縦の幅")] int _mapHeight = 9;
	//

    void Start()
    {
		_mapBlocks = new MapBlock[_mapWidth, _mapHeight];
		Vector3 defPos = new Vector3(0.0f, 0.0f, 0.0f);		// ブロック生成の基点となる座標
		defPos.x = -(_mapWidth / 2);	// x座標の基点
        defPos.z = -(_mapHeight / 2);	// z座標の基点
		for (int i = 0; i < _mapWidth; i++)
		{
			for (int j = 0; j < _mapHeight; j++)
			{
				Vector3 pos = defPos;	// 基点の座標を元に変数posを宣言
				pos.x += i;		// 繰り返し回数分x座標をずらす
				pos.z += j;		// 繰り返し回数分z座標をずらす

				GameObject obj;		// 生成するオブジェクトの参照
				obj = Instantiate(_blockPrefab_Metal, _blockParent); 
				obj.transform.position = pos;	// オブジェクトの座標を適用

				var mapBlock = obj.GetComponent<MapBlock>();
				_mapBlocks[i, j] = mapBlock;
				mapBlock.xPos = (int)pos.x;		// X位置を記録
				mapBlock.zPos = (int)pos.z;		// Z位置を記録
			}
		}
	}

	/// <summary>
	/// 全てのブロックの選択状態を解除する処理
	/// </summary>
	public void AllSelectClear()
	{
		for (int i = 0; i < _mapWidth; i++)
		{
			for (int j = 0; j < _mapHeight; j++)
			{
				_mapBlocks[i, j].SetSelectionMode(MapBlock.Highlight.Off);
			}
		}
	}

	/// <summary>
	/// 渡された位置からキャラクターが到達できるブロックをリストにして返す
	/// </summary>
	/// <param name="xPos">基点x位置</param>
	/// <param name="zPos">基点z位置</param>
	/// <returns>条件を満たすブロックのリスト</returns>
	public List<MapBlock> SearchReachableBlocks(int xPos, int zPos)
	{
		var results = new List<MapBlock>();

		int baseX = -1;
		int	baseZ = -1;
		for (int i = 0; i < _mapWidth; i++)// 基点となるブロックの配列内番号(index)を検索
		{
			for (int j = 0; j < _mapHeight; j++)
			{
				if ((_mapBlocks[i, j].xPos == xPos) && (_mapBlocks[i, j].zPos == zPos))
				{
					baseX = i;  // 配列内番号を取得
					baseZ = j;
					break;
				}
			}
			if (baseX != -1)// 既に発見済みなら1個目のループを抜ける
			{
				break;
			}
		}

		// 移動するキャラクターの移動方法を取得
		var moveType = Character.MoveType.Rook; 
		var moveChara = GetComponent<CharactersManager>().GetCharacterDataPos(xPos, zPos);
		if (moveChara != null)
		{
			moveType = moveChara.moveType; // キャラクターデータから移動方法を取得
		}

		// キャラクターの移動方法に合わせて異なる方向のブロックデータを取得していく
		if (moveType == Character.MoveType.Rook || moveType == Character.MoveType.Queen)	// 縦・横
		{
			// X+方向
			for (int i = baseX + 1; i < _mapWidth; i++)
				if (AddReachableList(results, _mapBlocks[i, baseZ]))
					break;
			// X-方向
			for (int i = baseX - 1; i >= 0; i--)
				if (AddReachableList(results, _mapBlocks[i, baseZ]))
					break;
			// Z+方向
			for (int j = baseZ + 1; j < _mapHeight; j++)
				if (AddReachableList(results, _mapBlocks[baseX, j]))
					break;
			// Z-方向
			for (int j = baseZ - 1; j >= 0; j--)
				if (AddReachableList(results, _mapBlocks[baseX, j]))
					break;
		}
		if (moveType == Character.MoveType.Bishop || moveType == Character.MoveType.Queen)	// 斜めへの移動
		{
			// X+Z+方向
			for (int i = baseX + 1, j = baseZ + 1;
				i < _mapWidth && j < _mapHeight;
				i++, j++)
				if (AddReachableList(results, _mapBlocks[i, j]))
					break;
			// X-Z+方向
			for (int i = baseX - 1, j = baseZ + 1;
				i >= 0 && j < _mapHeight;
				i--, j++)
				if (AddReachableList(results, _mapBlocks[i, j]))
					break;
			// X+Z-方向
			for (int i = baseX + 1, j = baseZ - 1;
				i < _mapWidth && j >= 0;
				i++, j--)
				if (AddReachableList(results, _mapBlocks[i, j]))
					break;
			// X-Z-方向
			for (int i = baseX - 1, j = baseZ - 1;
				i >= 0 && j >= 0;
				i--, j--)
				if (AddReachableList(results, _mapBlocks[i, j]))
					break;
		}
		results.Add(_mapBlocks[baseX, baseZ]);	// 足元のブロック
		return results;
	}

	/// <summary>
	/// 指定したブロックを到達可能ブロックリストに追加する
	/// </summary>
	/// <param name="reachableList">到達可能ブロックリスト</param>
	/// <param name="targetBlock">対象ブロック</param>
	/// <returns>行き止まりフラグ(行き止まりならtrueが返る)</returns>
	private bool AddReachableList(List<MapBlock> reachableList, MapBlock targetBlock)
	{
		if (!targetBlock.passable)// 対象のブロックが通行不可ならそこを行き止まりとして終了
		{
			return true;
		}

		var charaData = GetComponent<CharactersManager>().GetCharacterDataPos(targetBlock.xPos, targetBlock.zPos);
		if (charaData != null)
		{
			return false;
		}
		reachableList.Add(targetBlock);// 到達可能ブロックリストに追加する
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
		var results = new List<MapBlock>();
		int baseX = -1;
		int baseZ = -1;
		for (int i = 0; i < _mapWidth; i++)
		{
			for (int j = 0; j < _mapHeight; j++)
			{
				if ((_mapBlocks[i, j].xPos == xPos) && (_mapBlocks[i, j].zPos == zPos))
				{
					baseX = i;
					baseZ = j;
					break;
				}
			}

			if (baseX != -1)// 既に発見済みなら1個目のループを抜ける
			{
				break;
			}
		}

		// 4方向に1マス進んだ位置のブロックをそれぞれセット
		// (縦・横4マス)
		AddAttackableList(results, baseX + 1, baseZ);	// X+方向
		AddAttackableList(results, baseX - 1, baseZ);	// X-方向
		AddAttackableList(results, baseX, baseZ + 1);	// Z+方向
		AddAttackableList(results, baseX, baseZ - 1);	// Z-方向
		// (斜め4マス)
		AddAttackableList(results, baseX + 1, baseZ + 1);	// X+Z+方向
		AddAttackableList(results, baseX - 1, baseZ + 1);	// X-Z+方向
		AddAttackableList(results, baseX + 1, baseZ - 1);	// X+Z-方向
		AddAttackableList(results, baseX - 1, baseZ - 1);	// X-Z-方向

		return results;
	}

	/// <summary>
	/// マップデータの指定された配列内番号に対応するブロックを攻撃可能ブロックリストに追加する
	/// </summary>
	/// <param name="attackableList">攻撃可能ブロックリスト</param>
	/// <param name="indexX">X方向の配列内番号</param>
	/// <param name="indexZ">Z方向の配列内番号</param>
	private void AddAttackableList(List<MapBlock> attackableList, int indexX, int indexZ)
	{
		if (indexX < 0 || indexX >= _mapWidth || indexZ < 0 || indexZ >= _mapHeight)// 指定された番号が配列の外に出ていたら追加せず終了
		{
			return;
		}
		attackableList.Add(_mapBlocks[indexX, indexZ]);// 到達可能ブロックリストに追加する
	}

	/// <summary>
	/// マップデータ配列をリストにして返す
	/// </summary>
	/// <returns>マップデータのリスト</returns>
	public List<MapBlock> MapBlocksToList()
	{
		var results = new List<MapBlock>();
		for (int i = 0; i < _mapWidth; i++)		// マップデータ配列の中身を順番にリストに格納
		{
			for (int j = 0; j < _mapHeight; j++)
			{
				results.Add(_mapBlocks[i, j]);
			}
		}
		return results;
	}
}
