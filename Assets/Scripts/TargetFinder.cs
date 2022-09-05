using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetFinder
{
	public class ActionPlan
	{
		public Character charaData; // 行動する敵キャラクター
		public MapBlock toMoveBlock; // 移動先の位置
		public Character toAttackChara; // 攻撃相手のキャラクター
	}

	/// <summary>
	/// 攻撃可能な行動プランを全て検索し、その内の１つをランダムに返す処理
	/// </summary>
	/// <param name="mapManager">シーン内のMapManagerの参照</param>
	/// <param name="charactersManager">シーン内のCharactersManagerの参照</param>
	/// <param name="enemyCharas">敵キャラクターのリスト</param>
	/// <returns></returns>
	public static ActionPlan GetRandomActionPlan(MapManager mapManager, CharactersManager charactersManager, List<Character> enemyCharas)
	{
		// 全行動プラン(攻撃可能な相手が見つかる度に追加される)
		var actionPlans = new List<ActionPlan>();
		// 移動範囲リスト
		var reachableBlocks = new List<MapBlock>();
		// 攻撃範囲リスト
		var attackableBlocks = new List<MapBlock>();

		// 全行動プラン検索処理
		foreach (Character enemyData in enemyCharas)
		{
			// 移動可能な場所リストを取得する
			reachableBlocks =
				mapManager.SearchReachableBlocks(enemyData.xPos, enemyData.zPos);
			// それぞれの移動可能な場所ごとの処理
			foreach (MapBlock block in reachableBlocks)
			{
				// 攻撃可能な場所リストを取得する
				attackableBlocks = mapManager.SearchAttackableBlocks(block.xPos, block.zPos);
				// それぞれの攻撃可能な場所ごとの処理
				foreach (MapBlock attackBlock in attackableBlocks)
				{
					// 攻撃できる相手キャラクター(プレイヤー側のキャラクター)を探す
					Character targetChara =
						charactersManager.GetCharacterDataPos(attackBlock.xPos, attackBlock.zPos);
					if (targetChara != null &&
						!targetChara._isEnemy)
					{// 相手キャラクターが存在する
					 // 行動プランを新規作成する
						var newPlan = new ActionPlan();
						newPlan.charaData = enemyData;
						newPlan.toMoveBlock = block;
						newPlan.toAttackChara = targetChara;

						// 全行動プランリストに追加
						actionPlans.Add(newPlan);
					}
				}
			}
		}

		// 検索終了後、行動プランが１つでもあるならその内の１つをランダムに返す
		if (actionPlans.Count > 0)
			return actionPlans[Random.Range(0, actionPlans.Count)];
		else // 行動プランが無いならnullを返す
			return null;
	}
}