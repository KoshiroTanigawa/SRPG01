using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	private MapManager mapManager; // マップマネージャ
	private CharactersManager charactersManager; // 全キャラクター管理クラス
	private GUIManager guiManager; // GUIマネージャ

	// 進行管理変数
	private Character selectingChara; // 選択中のキャラクター(誰も選択していないならfalse)
	private List<MapBlock> reachableBlocks; // 選択中のキャラクターの移動可能ブロックリスト
	private List<MapBlock> attackableBlocks; // 選択中のキャラクターの攻撃可能ブロックリスト
	private bool isGameSet; // ゲーム終了フラグ(決着がついた後ならtrue)

	// 行動キャンセル処理用変数
	private int charaStartPos_X; // 選択キャラクターの移動前の位置(X方向)
	private int charaStartPos_Z; // 選択キャラクターの移動前の位置(Z方向)
	private MapBlock attackBlock; // 選択キャラクターの攻撃先のブロック

	// ターン進行モード一覧
	private enum Phase
	{
		MyTurn_Start,       // 自分のターン：開始時
		MyTurn_Moving,      // 自分のターン：移動先選択中
		MyTurn_Command,     // 自分のターン：移動後のコマンド選択中
		MyTurn_Targeting,   // 自分のターン：攻撃の対象を選択中
		MyTurn_Result,      // 自分のターン：行動結果表示中
		EnemyTurn_Start,    // 敵のターン：開始時
		EnemyTurn_Result    // 敵のターン：行動結果表示中
	}
	private Phase nowPhase; // 現在の進行モード

	void Start()
	{
		// 参照取得
		mapManager = GetComponent<MapManager>();
		charactersManager = GetComponent<CharactersManager>();
		guiManager = GetComponent<GUIManager>();

		// リストを初期化
		reachableBlocks = new List<MapBlock>();
		attackableBlocks = new List<MapBlock>();

		nowPhase = Phase.MyTurn_Start; // 開始時の進行モード
	}

	void Update()
	{
		// ゲーム終了後なら処理せず終了
		if (isGameSet)
			return;

		// タップ検出処理
		if (Input.GetMouseButtonDown(0) &&
			!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) // (←UIへのタップを検出する)
		{// UIでない部分でタップが行われた
		 // タップ先にあるブロックを取得して選択処理を開始する
			GetMapBlockByTapPos();
		}
	}

	/// <summary>
	/// タップした場所にあるオブジェクトを見つけ、選択処理などを開始する
	/// </summary>
	private void GetMapBlockByTapPos()
	{
		GameObject targetObject = null; // タップ対象のオブジェクト

		// タップした方向にカメラからRayを飛ばす
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{// Rayに当たる位置に存在するオブジェクトを取得(対象にColliderが付いている必要がある)
			targetObject = hit.collider.gameObject;
		}

		// 対象オブジェクト(マップブロック)が存在する場合の処理
		if (targetObject != null)
		{
			// ブロック選択時処理
			SelectBlock(targetObject.GetComponent<MapBlock>());
		}
	}

	/// <summary>
	/// 指定したブロックを選択状態にする処理
	/// </summary>
	/// <param name="targetMapBlock">対象のブロックデータ</param>
	private void SelectBlock(MapBlock targetBlock)
	{
		// 現在の進行モードごとに異なる処理を開始する
		switch (nowPhase)
		{
			// 自分のターン：開始時
			case Phase.MyTurn_Start:
				Debug.Log("現在のフェーズは-" + nowPhase + "-");
				// 全ブロックの選択状態を解除
				mapManager.AllSelectionModeClear();
                // ブロックを選択状態の表示にする
                targetBlock.SetSelectionMode(MapBlock.Highlight.Select);

				// 選択した位置に居るキャラクターのデータを取得
				var charaData =
					charactersManager.GetCharacterDataByPos(targetBlock.xPos, targetBlock.zPos);
				if (charaData != null)
				{// キャラクターが存在する
				 // 選択中のキャラクター情報に記憶
					selectingChara = charaData;
					// 選択キャラクターの現在位置を記憶
					charaStartPos_X = selectingChara.xPos;
					charaStartPos_Z = selectingChara.zPos;
					// キャラクターのステータスをUIに表示する
					guiManager.ShowStatusWindow(selectingChara);

					// 移動可能な場所リストを取得する
					reachableBlocks = mapManager.SearchReachableBlocks(charaData.xPos, charaData.zPos);

					// 移動可能な場所リストを表示する
					foreach (MapBlock mapBlock in reachableBlocks)
						mapBlock.SetSelectionMode(MapBlock.Highlight.Reachable);

					// 移動キャンセルボタン表示
					guiManager.ShowMoveCancelButton();
					// 進行モードを進める：移動先選択中
					ChangePhase(Phase.MyTurn_Moving);
				}
				else
				{// キャラクターが存在しない
				 // 選択中のキャラクター情報を初期化する
					ClearSelectingChara();
				}
				break;

			// 自分のターン：移動先選択中
			case Phase.MyTurn_Moving:
				Debug.Log("現在のフェーズは-" + nowPhase + "-");
				// 敵キャラクターを選択中なら移動をキャンセルして終了
				if (selectingChara.isEnemy)
				{
					CancelMoving();
					break;
				}

				// 選択ブロックが移動可能な場所リスト内にある場合、移動処理を開始
				if (reachableBlocks.Contains(targetBlock))
				{
					// 選択中のキャラクターを移動させる
					selectingChara.MovePosition(targetBlock.xPos, targetBlock.zPos);

					// 移動可能な場所リストを初期化する
					reachableBlocks.Clear();

					// 全ブロックの選択状態を解除
					mapManager.AllSelectionModeClear();

					// 移動キャンセルボタン非表示
					guiManager.HideMoveCancelButton();

					// 指定秒数経過後に処理を実行する(DoTween)
					DOVirtual.DelayedCall(
						0.5f, // 遅延時間(秒)
						() =>
						{// 遅延実行する内容
						 // コマンドボタンを表示する
							guiManager.ShowCommandButtons();
							// 進行モードを進める：移動後のコマンド選択中
							ChangePhase(Phase.MyTurn_Command);
						}
					);
				}
				break;

			// 自分のターン：移動後のコマンド選択中
			case Phase.MyTurn_Command:
				Debug.Log("現在のフェーズは-" + nowPhase + "-");
				// 攻撃範囲のブロックを選択した時、行動するかの確認ボタンを表示する
				if (attackableBlocks.Contains(targetBlock))
				{
					// 攻撃先のブロック情報を記憶
					attackBlock = targetBlock;
					// 行動決定・キャンセルボタンを表示する
					guiManager.ShowDecideButtons();

					// 攻撃可能な場所リストを初期化する
					attackableBlocks.Clear();
					// 全ブロックの選択状態を解除
					mapManager.AllSelectionModeClear();

					// 攻撃先のブロックを強調表示する
					attackBlock.SetSelectionMode(MapBlock.Highlight.Attackable);

					// 進行モードを進める：攻撃の対象を選択中
					ChangePhase(Phase.MyTurn_Targeting);
				}
				break;
		}
	}

	/// <summary>
	/// 選択中のキャラクター情報を初期化する
	/// </summary>
	private void ClearSelectingChara()
	{
		// 選択中のキャラクターを初期化する
		selectingChara = null;
		// キャラクターのステータスUIを非表示にする
		guiManager.HideStatusWindow();
	}

	/// <summary>
	/// 攻撃コマンドボタン処理
	/// </summary>
	public void AttackCommand()
	{
		// 攻撃範囲を取得して表示する
		GetAttackableBlocks();
	}

	/// <summary>
	/// 攻撃コマンド選択後に対象ブロックを表示する処理
	/// </summary>
	private void GetAttackableBlocks()
	{
		// コマンドボタンを非表示にする
		guiManager.HideCommandButtons();

		// 攻撃可能な場所リストを表示する
		foreach (MapBlock mapBlock in attackableBlocks)
			mapBlock.SetSelectionMode(MapBlock.Highlight.Attackable);
	}

	/// <summary>
	/// 待機コマンドボタン処理
	/// </summary>
	public void StandbyCommand()
	{
		// コマンドボタンを非表示にする
		guiManager.HideCommandButtons();
		// 進行モードを進める(敵のターンへ)
		ChangePhase(Phase.EnemyTurn_Start);
	}

	/// <summary>
	/// 行動内容決定ボタン処理
	/// </summary>
	public void ActionDecideButton()
	{
		// 行動決定・キャンセルボタンを非表示にする
		guiManager.HideDecideButtons();
		// 攻撃先のブロックの強調表示を解除する
		attackBlock.SetSelectionMode(MapBlock.Highlight.Off);

		// 攻撃対象の位置に居るキャラクターのデータを取得
		var targetChara =
			charactersManager.GetCharacterDataByPos(attackBlock.xPos, attackBlock.zPos);
		if (targetChara != null)
		{// 攻撃対象のキャラクターが存在する
		 // キャラクター攻撃処理
			CharaAttack(selectingChara, targetChara);

			// 進行モードを進める(行動結果表示へ)
			ChangePhase(Phase.MyTurn_Result);
			return;
		}
		else
		{// 攻撃対象が存在しない
		 // 進行モードを進める(敵のターンへ)
			ChangePhase(Phase.EnemyTurn_Start);
		}
	}
	/// <summary>
	/// 行動内容リセットボタン処理
	/// </summary>
	public void ActionCancelButton()
	{
		// 行動決定・キャンセルボタンを非表示にする
		guiManager.HideDecideButtons();
		// 攻撃先のブロックの強調表示を解除する
		attackBlock.SetSelectionMode(MapBlock.Highlight.Off);

		// キャラクターを移動前の位置に戻す
		selectingChara.MovePosition(charaStartPos_X, charaStartPos_Z);
		// キャラクターの選択を解除する
		ClearSelectingChara();

		// 進行モードを戻す(ターンの最初へ)
		ChangePhase(Phase.MyTurn_Start, true);
	}

	/// <summary>
	/// キャラクターが他のキャラクターに攻撃する処理
	/// </summary>
	/// <param name="attackChara">攻撃側キャラデータ</param>
	/// <param name="defenseChara">防御側キャラデータ</param>
	private void CharaAttack(Character attackChara, Character defenseChara)
	{
		// ダメージ計算処理
		int damageValue; // ダメージ量
		int attackPoint = attackChara.atk; // 攻撃側の攻撃力
		int defencePoint = defenseChara.def; // 防御側の防御力
		// ダメージ＝攻撃力－防御力で計算
		damageValue = attackPoint - defencePoint;

		// アニメーション内で攻撃が当たったくらいのタイミングでSEを再生
		DOVirtual.DelayedCall(
			0.45f, // 遅延時間(秒)
			() =>
			{// 遅延実行する内容
			 // AudioSourceを再生
				GetComponent<AudioSource>().Play();
			}
		);

		// バトル結果表示ウィンドウの表示設定
		// (HPの変更前に行う)
		guiManager.battleWindowUI.ShowWindow(defenseChara, damageValue);

		// ダメージ量分防御側のHPを減少
		defenseChara.nowHP -= damageValue;
		// HPが0～最大値の範囲に収まるよう補正
		defenseChara.nowHP = Mathf.Clamp(defenseChara.nowHP, 0, defenseChara.maxHP);

		// HP0になったキャラクターを削除する
		if (defenseChara.nowHP == 0)
			charactersManager.DeleteCharaData(defenseChara);

		// ターン切り替え処理(遅延実行)
		DOVirtual.DelayedCall(
			2.0f, // 遅延時間(秒)
			() =>
			{// 遅延実行する内容
			 // ウィンドウを非表示化
				guiManager.battleWindowUI.HideWindow();
				// ターンを切り替える
				if (nowPhase == Phase.MyTurn_Result) // 敵のターンへ
					ChangePhase(Phase.EnemyTurn_Start);
				else if (nowPhase == Phase.EnemyTurn_Result) // 自分のターンへ
					ChangePhase(Phase.MyTurn_Start);
			}
		);
	}


	/// <summary>
	/// ターン進行モードを変更する
	/// </summary>
	/// <param name="newPhase">変更先モード</param>
	/// <param name="noLogos">ロゴ非表示フラグ(省略可能・省略するとfalse)</param>
	private void ChangePhase(Phase newPhase, bool noLogos = false)
	{
		// ゲーム終了後なら処理せず終了
		if (isGameSet)
			return;

		// モード変更を保存
		nowPhase = newPhase;

		// 特定のモードに切り替わったタイミングで行う処理
		switch (nowPhase)
		{
			// 自分のターン：開始時
			case Phase.MyTurn_Start:
				Debug.Log("現在のフェーズは-" + nowPhase + "-");
				// 自分のターン開始時のロゴを表示
				if (!noLogos)
					guiManager.ShowLogo_PlayerTurn();
				break;

			// 敵のターン：開始時
			case Phase.EnemyTurn_Start:
				Debug.Log("現在のフェーズは-" + nowPhase + "-");
				// 敵のターン開始時のロゴを表示
				if (!noLogos)
					guiManager.ShowLogo_EnemyTurn();

				// 敵の行動を開始する処理
				// (ロゴ表示後に開始したいので遅延処理にする)
				DOVirtual.DelayedCall(
					1.0f, // 遅延時間(秒)
					() =>
					{// 遅延実行する内容
						EnemyCommand();
					}
				);
				break;
		}
	}

	/// <summary>
	/// (敵のターン開始時に呼出)
	/// 敵キャラクターのうちいずれか一体を行動させてターンを終了する
	/// </summary>
	private void EnemyCommand()
	{
		// 生存中の敵キャラクターのリストを作成する
		var enemyCharas = new List<Character>(); // 敵キャラクターリスト
		foreach (Character charaData in charactersManager.characters)
		{// 全生存キャラクターから敵フラグの立っているキャラクターをリストに追加
			if (charaData.isEnemy)
				enemyCharas.Add(charaData);
		}

		// 攻撃可能なキャラクター・位置の組み合わせの内１つをランダムに取得
		var actionPlan = TargetFinder.GetRandomActionPlan
			(mapManager, charactersManager, enemyCharas);
		// 組み合わせのデータが存在すれば攻撃開始
		if (actionPlan != null)
		{
			// 敵キャラクター移動処理
			actionPlan.charaData.MovePosition(actionPlan.toMoveBlock.xPos, actionPlan.toMoveBlock.zPos);
			// 敵キャラクター攻撃処理
			// (移動後のタイミングで攻撃開始するよう遅延実行)
			DOVirtual.DelayedCall(
				1.0f, // 遅延時間(秒)
				() =>
				{// 遅延実行する内容
					CharaAttack(actionPlan.charaData, actionPlan.toAttackChara);
				}
			);

			// 進行モードを進める(行動結果表示へ)
			ChangePhase(Phase.EnemyTurn_Result);
			return;
		}

		// 攻撃可能な相手が見つからなかった場合
		// 移動させる１体をランダムに選ぶ
		int randId = Random.Range(0, enemyCharas.Count);
		Character targetEnemy = enemyCharas[randId]; // 行動対象の敵データ
													 // 対象の移動可能場所リストの中から1つの場所をランダムに選ぶ
		reachableBlocks =
			mapManager.SearchReachableBlocks(targetEnemy.xPos, targetEnemy.zPos);
		randId = Random.Range(0, reachableBlocks.Count);
		MapBlock targetBlock = reachableBlocks[randId]; // 移動対象のブロックデータ
														// 敵キャラクター移動処理
		targetEnemy.MovePosition(targetBlock.xPos, targetBlock.zPos);

		// 移動場所・攻撃場所リストをクリアする
		reachableBlocks.Clear();
		attackableBlocks.Clear();
		// 進行モードを進める(自分のターンへ)
		ChangePhase(Phase.MyTurn_Start);
	}

	/// <summary>
	/// 選択中のキャラクターの移動入力待ち状態を解除する
	/// </summary>
	public void CancelMoving()
	{
		// 全ブロックの選択状態を解除
		mapManager.AllSelectionModeClear();
		// 移動可能な場所リストを初期化する
		reachableBlocks.Clear();
		// 選択中のキャラクター情報を初期化する
		ClearSelectingChara();
		// 移動やめるボタン非表示
		guiManager.HideMoveCancelButton();
		// フェーズを元に戻す(ロゴを表示しない設定)
		ChangePhase(Phase.MyTurn_Start, true);
	}

	/// <summary>
	/// ゲームの終了条件を満たすか確認し、満たすならゲームを終了する
	/// </summary>
	public void CheckGameSet()
	{
		// プレイヤー勝利フラグ(生きている敵がいるならOffになる)
		bool isWin = true;
		// プレイヤー敗北フラグ(生きている味方がいるならOffになる)
		bool isLose = true;

		// それぞれ生きている敵・味方が存在するかをチェック
		foreach (var charaData in charactersManager.characters)
		{
			if (charaData.isEnemy) // 敵が居るので勝利フラグOff
				isWin = false;
			else // 味方が居るので敗北フラグOff
				isLose = false;
		}

		// 勝利または敗北のフラグが立ったままならゲームを終了する
		// (どちらのフラグも立っていないなら何もせずターンが進行する)
		if (isWin || isLose)
		{
			// ゲーム終了フラグを立てる
			isGameSet = true;

			// ロゴUIとフェードインを表示する(遅延実行)
			DOVirtual.DelayedCall(
				1.5f, () =>
				{
					if (isWin) // ゲームクリア演出
						guiManager.ShowLogo_GameClear();
					else // ゲームオーバー演出
						guiManager.ShowLogo_GameOver();

					// 移動可能な場所リストを初期化する
					reachableBlocks.Clear();
					// 全ブロックの選択状態を解除
					mapManager.AllSelectionModeClear();
				}
			);

			// Gameシーンの再読み込み(遅延実行)
			DOVirtual.DelayedCall(
				7.0f, () =>
				{
					SceneManager.LoadScene("Enhance");
				}
			);
		}
	}

}