using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UIコンポーネントを扱うのに必要
using DG.Tweening;

public class GUIManager : MonoBehaviour
{
	// ステータスウィンドウUI
	public GameObject statusWindow; // ステータスウィンドウオブジェクト
	public Text nameText; // 名前Text
	public Text hpName; // HP
	public Text hpText; // HPText

	// キャラクターのコマンドボタン
	public GameObject commandButtons; // 全コマンドボタンの親オブジェクト

	// バトル結果表示UI処理クラス
	public BattleWindowUI battleWindowUI;

	// 各種ロゴ画像
	public Image playerTurnImage; // プレイヤーターン開始時画像
	public Image enemyTurnImage; // 敵ターン開始時画像
	public Image gameClearImage; // ゲームクリア画像
	public Image gameOverImage; // ゲームオーバー画像

	// 移動キャンセルボタンUI
	public GameObject moveCancelButton;

	// 行動決定・キャンセルボタンUI
	public GameObject decideButtons;

	void Start()
	{
		// UI初期化
		HideStatusWindow(); // ステータスウィンドウを隠す
		HideCommandButtons(); // コマンドボタンを隠す
		HideMoveCancelButton(); // 移動キャンセルボタンを隠す
		HideDecideButtons(); // 行動決定・キャンセルボタンを隠す
	}

	/// <summary>
	/// ステータスウィンドウを表示する
	/// </summary>
	/// <param name="charaData">表示キャラクターデータ</param>
	public void ShowStatusWindow(Character charaData)
	{
		// オブジェクトアクティブ化
		statusWindow.SetActive(true);

		// 名前Text表示
		nameText.text = charaData.charaName;
	}

	/// <summary>
	/// ステータスウィンドウを隠す
	/// </summary>
	public void HideStatusWindow()
	{
		// オブジェクト非アクティブ化
		statusWindow.SetActive(false);
	}

	/// <summary>
	/// コマンドボタンを表示する
	/// </summary>
	public void ShowCommandButtons()
	{
		commandButtons.SetActive(true);
	}

	/// <summary>
	/// コマンドボタンを隠す
	/// </summary>
	public void HideCommandButtons()
	{
		commandButtons.SetActive(false);
	}

	/// <summary>
	/// プレイヤーのターンに切り替わった時のロゴ画像を表示する
	/// </summary>
	public void ShowLogo_PlayerTurn()
	{
		// 徐々に表示→非表示を行うアニメーション(Tween)
		playerTurnImage
			.DOFade(1.0f, // 指定数値まで画像のalpha値を変化
				1.0f) // アニメーション時間(秒)
			.SetEase(Ease.OutCubic) // イージング(変化の度合)を設定
			.SetLoops(2, LoopType.Yoyo); // ループ回数・方式を指定
	}
	/// <summary>
	/// 敵のターンに切り替わった時のロゴ画像を表示する
	/// </summary>
	public void ShowLogo_EnemyTurn()
	{
		// 徐々に表示→非表示を行うアニメーション(Tween)
		enemyTurnImage
			.DOFade(1.0f, // 指定数値まで画像のalpha値を変化
				1.0f) // アニメーション時間(秒)
			.SetEase(Ease.OutCubic) // イージング(変化の度合)を設定
			.SetLoops(2, LoopType.Yoyo); // ループ回数・方式を指定
	}

	/// <summary>
	/// 移動キャンセルボタンを表示する
	/// </summary>
	public void ShowMoveCancelButton()
	{
		moveCancelButton.SetActive(true);
	}
	/// <summary>
	/// 移動キャンセルボタンを非表示にする
	/// </summary>
	public void HideMoveCancelButton()
	{
		moveCancelButton.SetActive(false);
	}

	/// <summary>
	/// ゲームクリア時のロゴ画像を表示する
	/// </summary>
	public void ShowLogo_GameClear()
	{
		// 徐々に表示するアニメーション
		gameClearImage
			.DOFade(1.0f, // 指定数値まで画像のalpha値を変化
				1.0f) // アニメーション時間(秒)
			.SetEase(Ease.OutCubic); // イージング(変化の度合)を設定

		// 拡大→縮小を行うアニメーション
		gameClearImage.transform
			.DOScale(1.5f, // 指定数値まで画像のalpha値を変化
				1.0f) // アニメーション時間(秒)
			.SetEase(Ease.OutCubic) // イージング(変化の度合)を設定
			.SetLoops(2, LoopType.Yoyo); // ループ回数・方式を指定
	}
	/// <summary>
	/// ゲームオーバーのロゴ画像を表示する
	/// </summary>
	public void ShowLogo_GameOver()
	{
		// 徐々に表示するアニメーション
		gameOverImage.
			DOFade(1.0f, // 指定数値まで画像のalpha値を変化
				1.0f) // アニメーション時間(秒)
			.SetEase(Ease.OutCubic); // イージング(変化の度合)を設定
	}

	/// <summary>
	/// 行動決定・キャンセルボタンを表示する
	/// </summary>
	public void ShowDecideButtons()
	{
		decideButtons.SetActive(true);
	}
	/// <summary>
	/// 行動決定・キャンセルボタンを非表示にする
	/// </summary>
	public void HideDecideButtons()
	{
		decideButtons.SetActive(false);
	}

}