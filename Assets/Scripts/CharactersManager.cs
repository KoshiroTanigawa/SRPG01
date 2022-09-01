using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CharactersManager : MonoBehaviour
{
	// オブジェクト
	public Transform charactersParent; // 全キャラクターオブジェクトの親オブジェクトTransform

	// 全キャラクターデータ
	[HideInInspector]
	public List<Character> characters;

	void Start()
	{
		// マップ上の全キャラクターデータを取得
		// (charactersParent以下の全Characterコンポーネントを検索しリストに格納)
		characters = new List<Character>();
		charactersParent.GetComponentsInChildren(characters);
	}

	/// <summary>
	/// 指定した位置に存在するキャラクターデータを検索して返す
	/// </summary>
	/// <param name="xPos">X位置</param>
	/// <param name="zPos">Z位置</param>
	/// <returns>対象のキャラクターデータ</returns>
	public Character GetCharacterDataByPos(int xPos, int zPos)
	{
		// 検索処理
		// (foreachでマップ内の全キャラクターデータ１体１体ずつに同じ処理を行う)
		foreach (Character charaData in characters)
		{
			// キャラクターの位置が指定された位置と一致しているかチェック
			if ((charaData.xPos == xPos) && // X位置が同じ
				(charaData.zPos == zPos)) // Z位置が同じ
			{// 位置が一致している
				return charaData; // データを返して終了
			}
		}

		// データが見つからなければnullを返す
		return null;
	}

	/// <summary>
	/// 指定したキャラクターを削除する
	/// </summary>
	/// <param name="charaData">対象キャラデータ</param>
	public void DeleteCharaData(Character charaData)
	{
		// リストからデータを削除
		characters.Remove(charaData);
		// オブジェクト削除(遅延実行)
		DOVirtual.DelayedCall(
			0.5f, // 遅延時間(秒)
			() =>
			{// 遅延実行する内容
				Destroy(charaData.gameObject);
			}
		);
		// ゲーム終了判定を行う
		GetComponent<GameManager>().CheckGameSet();
	}
}