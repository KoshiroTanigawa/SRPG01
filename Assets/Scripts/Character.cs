using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Character : MonoBehaviour
{
	// メインカメラ
	private Camera mainCamera;

	// キャラクター初期設定(インスペクタから入力)
	[Header("初期X位置(-4～4)")]
	public int initPos_X; // 初期X位置
	[Header("初期Z位置(-4～4)")]
	public int initPos_Z; // 初期Z位置
	[Header("敵フラグ(ONで敵キャラとして扱う)")]
	public bool isEnemy; // 敵フラグ
	// キャラクターデータ(初期ステータス)
	[Header("キャラクター名")]
	public string charaName; // キャラクター名
	[Header("最大HP(初期HP)")]
	public int maxHP; // 最大HP
	[Header("攻撃力")]
	public int atk; // 攻撃力
	[Header("防御力")]
	public int def; // 防御力
	[Header("移動方法")]
	public MoveType moveType; // 移動方法
	

	// ゲーム中に変化するキャラクターデータ
	[HideInInspector]
	public int xPos; // 現在のx座標
	[HideInInspector]
	public int zPos; // 現在のz座標
	[HideInInspector]
	public int nowHP; // 現在HP

	// キャラクター移動方法定義(列挙型)
	public enum MoveType
	{
		Rook, // 縦・横
		Bishop, // 斜め
		Queen, // 縦・横・斜め
	}

	void Start()
	{

		// 初期位置に対応する座標へオブジェクトを移動させる
		Vector3 pos = new Vector3();
		pos.x = initPos_X; // x座標：1ブロックのサイズが1(1.0f)なのでそのまま代入
		pos.y = 1.0f; // y座標（固定）
		pos.z = initPos_Z; // z座標
		transform.position = pos; // オブジェクトの座標を変更

		// その他変数初期化
		xPos = initPos_X;
		zPos = initPos_Z;
		nowHP = maxHP;
	}

	/// <summary>
	/// 対象の座標へとキャラクターを移動させる
	/// </summary>
	/// <param name="targetXPos">x座標</param>
	/// <param name="targetZPos">z座標</param>
	public void MovePosition(int targetXPos, int targetZPos)
	{
		// オブジェクトを移動させる
		// 移動先座標への相対座標を取得
		Vector3 movePos = Vector3.zero; // (0.0f, 0.0f, 0.0f)でVector3で初期化
		movePos.x = targetXPos - xPos; // x方向の相対距離
		movePos.z = targetZPos - zPos; // z方向の相対距離

		// DoTweenのTweenを使用して徐々に位置が変化するアニメーションを行う
		transform.DOMove(movePos, // 指定座標まで移動する
				0.5f) // アニメーション時間(秒)
			.SetEase(Ease.Linear) // イージング(変化の度合)を設定
			.SetRelative(); // パラメータを相対指定にする

		// キャラクターデータに位置を保存
		xPos = targetXPos;
		zPos = targetZPos;
	}

	/// <summary>
	/// キャラクターの近接攻撃アニメーション
	/// </summary>
	/// <param name="targetChara">相手キャラクター</param>
	public void AttackAnimation(Character targetChara)
	{
		// 攻撃アニメーション(DoTween)
		// 相手キャラクターの位置へジャンプで近づき、同じ動きで元の場所に戻る
		transform.DOJump(targetChara.transform.position, // 指定座標までジャンプしながら移動する
				1.0f, // ジャンプの高さ
				1, // ジャンプ回数
				0.5f) // アニメーション時間(秒)
			.SetEase(Ease.Linear) // イージング(変化の度合)を設定
			.SetLoops(2, LoopType.Yoyo); // ループ回数・方式を指定
	}
}