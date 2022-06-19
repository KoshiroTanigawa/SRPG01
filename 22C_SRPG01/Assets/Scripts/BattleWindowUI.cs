using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BattleWindowUI : MonoBehaviour
{
	// バトル結果表示ウィンドウUI
	public Text nameText; // 名前Text
	public Text hpText; // HPText
	public Text damageText; // ダメージ量Text

	void Start()
	{
		// 初期化時にウィンドウを隠す
		HideWindow();
	}

	/// <summary>
	/// バトル結果ウィンドウを表示する
	/// </summary>
	/// <param name="charaData">攻撃されたキャラクターのデータ</param>
	/// <param name="damageValue">ダメージ量</param>
	public void ShowWindow(Character charaData, int damageValue)
	{
		// オブジェクトアクティブ化
		gameObject.SetActive(true);

		// 名前Text表示
		nameText.text = charaData.charaName;

		// ダメージ計算後の残りHPを取得する
		// (ここでは対象キャラクターデータのHPは変化させない)
		int nowHP = charaData.nowHP - damageValue;
		// HPが0～最大値の範囲に収まるよう補正
		nowHP = Mathf.Clamp(nowHP, 0, charaData.maxHP);

		// HPText表示(現在値と最大値両方を表示)
		hpText.text = nowHP + "/" + charaData.maxHP;
		// ダメージ量Text表示
		// ダメージ量Text表示
		if (damageValue >= 0)// ダメージ発生時
			damageText.text = damageValue + "ダメージ！";
		else// HP回復時
			damageText.text = -damageValue + "回復！";
	}
	/// <summary>
	/// バトル結果ウィンドウを隠す
	/// </summary>
	public void HideWindow()
	{
		// オブジェクト非アクティブ化
		gameObject.SetActive(false);
	}
}