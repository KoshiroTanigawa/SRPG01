using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Character : MonoBehaviour
{
	// メインカメラ
	private Camera mainCamera;

	// キャラクター初期位置
	[SerializeField] [Header("初期X位置(-4～4)")] int _startPosX;
	[SerializeField] [Header("初期Z位置(-4～4)")] int _startPosZ;

	//エネミーフラグ
	[Header("敵フラグ(ONで敵キャラとして扱う)")] public bool _isEnemy;

	// キャラクターデータ(初期ステータス)
	[Header("キャラクター名")] public string charaName;
	[Header("最大HP(初期HP)")] public int maxHP;
	[Header("攻撃力")] public int atk;
	[Header("防御力")] public int def;
	[Header("移動方法")] public MoveType moveType;
	

	// ゲーム中に変化するキャラクターデータ
	[HideInInspector] public int xPos; // 現在のx座標
	[HideInInspector] public int zPos; // 現在のz座標
	[HideInInspector] public int nowHP; // 現在HP

	// キャラクター移動方法
	public enum MoveType
	{
		Rook, // 縦・横
		Bishop, // 斜め
		Queen, // 縦・横・斜め
	}

	void Start()
	{
		Vector3 pos = new Vector3();
		pos.x = _startPosX;
		pos.y = 1.0f;
		pos.z = _startPosZ;
		transform.position = pos;

		xPos = _startPosX;
		zPos = _startPosZ;
		nowHP = maxHP;
	}

	/// <summary>
	/// 対象の座標へとキャラクターを移動させる
	/// </summary>
	/// <param name="targetXPos">x座標</param>
	/// <param name="targetZPos">z座標</param>
	public void MovePosition(int targetXPos, int targetZPos)
	{
		Vector3 movePos = Vector3.zero; 
		movePos.x = targetXPos - xPos; // x方向の相対距離
		movePos.z = targetZPos - zPos; // z方向の相対距離

		/*
		transform.DOMove(movePos, // 指定座標まで移動する
				0.5f) // アニメーション時間(秒)
			.SetEase(Ease.Linear) // イージング(変化の度合)を設定
			.SetRelative(); // パラメータを相対指定にする
		*/

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