using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CharactersManager : MonoBehaviour
{
	public Transform _charaParent; // 全キャラクターオブジェクトの親オブジェクトTransform
	[HideInInspector] public List<Character> characters;// 全キャラクターデータ

	void Start()
	{
		characters = new List<Character>();
		_charaParent.GetComponentsInChildren(characters);//charactersParent以下の全Characterコンポーネントを検索しリストに格納
	}

	/// <summary>
	/// 指定した位置に存在するキャラクターデータを検索して返す
	/// </summary>
	/// <param name="xPos">X位置</param>
	/// <param name="zPos">Z位置</param>
	/// <returns>対象のキャラクターデータ</returns>
	public Character GetCharacterDataPos(int xPos, int zPos)
	{
		foreach (Character charaData in characters)
		{
			if ((charaData.xPos == xPos) && (charaData.zPos == zPos))
			{
				return charaData; 
			}
		}
		return null;
	}

	/// <summary>
	/// 指定したキャラクターを削除する
	/// </summary>
	/// <param name="charaData">対象キャラデータ</param>
	public void DeleteCharaData(Character charaData)
	{
		characters.Remove(charaData);// リストからデータを削除
		DOVirtual.DelayedCall(0.5f,() =>{Destroy(charaData.gameObject);});
		GetComponent<GameManager>().CheckGameSet();// ゲーム終了判定を行う
	}
}