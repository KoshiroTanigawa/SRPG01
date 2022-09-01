using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlock : MonoBehaviour
{
	// 強調表示オブジェクト
	private GameObject selectionBlockObj;

	// 強調表示マテリアル
	[Header("強調表示マテリアル：選択時")]
	public Material selMat_Select; // 選択時
	[Header("強調表示マテリアル：到達可能")]
	public Material selMat_Reachable; // キャラクターが到達可能
	[Header("強調表示マテリアル：攻撃可能")]
	public Material selMat_Attackable; // キャラクターが攻撃可能
									   // ブロックの強調表示モードを定義する(列挙型)
	public enum Highlight
	{
		Off, // オフ
		Select, // 選択時
		Reachable, // キャラクターが到達可能
		Attackable, // キャラクターが攻撃可能
	}

	// ブロックデータ
	[HideInInspector] // インスペクタ上で非表示にする属性
	public int xPos; // X方向の位置
	[HideInInspector]
	public int zPos; // Z方向の位置
	[Header("通行可能フラグ(trueなら通行可能である)")]
	public bool passable; // 通行可能フラグ

	void Start()
	{
		// 強調表示オブジェクトを取得
		selectionBlockObj = transform.GetChild(0).gameObject; // 子の１番目にあるオブジェクト

		// 初期状態では強調表示をしない
		SetSelectionMode(Highlight.Off);
	}

	/// <summary>
	/// 選択状態表示オブジェクトの表示・非表示を設定する
	/// </summary>
	/// <param name="mode">強調表示モード</param>
	public void SetSelectionMode(Highlight mode)
	{
		switch (mode)
		{
			// 強調表示なし
			case Highlight.Off:
				selectionBlockObj.SetActive(false);
				break;
			// 選択時
			case Highlight.Select:
				selectionBlockObj.GetComponent<Renderer>().material = selMat_Select;
				selectionBlockObj.SetActive(true);
				break;
			// キャラクターが到達可能
			case Highlight.Reachable:
				selectionBlockObj.GetComponent<Renderer>().material = selMat_Reachable;
				selectionBlockObj.SetActive(true);
				break;
			// キャラクターが攻撃可能
			case Highlight.Attackable:
				selectionBlockObj.GetComponent<Renderer>().material = selMat_Attackable;
				selectionBlockObj.SetActive(true);
				break;
		}
	}

}