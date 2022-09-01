using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	private MapManager mapManager; // �}�b�v�}�l�[�W��
	private CharactersManager charactersManager; // �S�L�����N�^�[�Ǘ��N���X
	private GUIManager guiManager; // GUI�}�l�[�W��

	// �i�s�Ǘ��ϐ�
	private Character selectingChara; // �I�𒆂̃L�����N�^�[(�N���I�����Ă��Ȃ��Ȃ�false)
	private List<MapBlock> reachableBlocks; // �I�𒆂̃L�����N�^�[�̈ړ��\�u���b�N���X�g
	private List<MapBlock> attackableBlocks; // �I�𒆂̃L�����N�^�[�̍U���\�u���b�N���X�g
	private bool isGameSet; // �Q�[���I���t���O(������������Ȃ�true)

	// �s���L�����Z�������p�ϐ�
	private int charaStartPos_X; // �I���L�����N�^�[�̈ړ��O�̈ʒu(X����)
	private int charaStartPos_Z; // �I���L�����N�^�[�̈ړ��O�̈ʒu(Z����)
	private MapBlock attackBlock; // �I���L�����N�^�[�̍U����̃u���b�N

	// �^�[���i�s���[�h�ꗗ
	private enum Phase
	{
		MyTurn_Start,       // �����̃^�[���F�J�n��
		MyTurn_Moving,      // �����̃^�[���F�ړ���I��
		MyTurn_Command,     // �����̃^�[���F�ړ���̃R�}���h�I��
		MyTurn_Targeting,   // �����̃^�[���F�U���̑Ώۂ�I��
		MyTurn_Result,      // �����̃^�[���F�s�����ʕ\����
		EnemyTurn_Start,    // �G�̃^�[���F�J�n��
		EnemyTurn_Result    // �G�̃^�[���F�s�����ʕ\����
	}
	private Phase nowPhase; // ���݂̐i�s���[�h

	void Start()
	{
		// �Q�Ǝ擾
		mapManager = GetComponent<MapManager>();
		charactersManager = GetComponent<CharactersManager>();
		guiManager = GetComponent<GUIManager>();

		// ���X�g��������
		reachableBlocks = new List<MapBlock>();
		attackableBlocks = new List<MapBlock>();

		nowPhase = Phase.MyTurn_Start; // �J�n���̐i�s���[�h
	}

	void Update()
	{
		// �Q�[���I����Ȃ珈�������I��
		if (isGameSet)
			return;

		// �^�b�v���o����
		if (Input.GetMouseButtonDown(0) &&
			!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) // (��UI�ւ̃^�b�v�����o����)
		{// UI�łȂ������Ń^�b�v���s��ꂽ
		 // �^�b�v��ɂ���u���b�N���擾���đI���������J�n����
			GetMapBlockByTapPos();
		}
	}

	/// <summary>
	/// �^�b�v�����ꏊ�ɂ���I�u�W�F�N�g�������A�I�������Ȃǂ��J�n����
	/// </summary>
	private void GetMapBlockByTapPos()
	{
		GameObject targetObject = null; // �^�b�v�Ώۂ̃I�u�W�F�N�g

		// �^�b�v���������ɃJ��������Ray���΂�
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{// Ray�ɓ�����ʒu�ɑ��݂���I�u�W�F�N�g���擾(�Ώۂ�Collider���t���Ă���K�v������)
			targetObject = hit.collider.gameObject;
		}

		// �ΏۃI�u�W�F�N�g(�}�b�v�u���b�N)�����݂���ꍇ�̏���
		if (targetObject != null)
		{
			// �u���b�N�I��������
			SelectBlock(targetObject.GetComponent<MapBlock>());
		}
	}

	/// <summary>
	/// �w�肵���u���b�N��I����Ԃɂ��鏈��
	/// </summary>
	/// <param name="targetMapBlock">�Ώۂ̃u���b�N�f�[�^</param>
	private void SelectBlock(MapBlock targetBlock)
	{
		// ���݂̐i�s���[�h���ƂɈقȂ鏈�����J�n����
		switch (nowPhase)
		{
			// �����̃^�[���F�J�n��
			case Phase.MyTurn_Start:
				Debug.Log("���݂̃t�F�[�Y��-" + nowPhase + "-");
				// �S�u���b�N�̑I����Ԃ�����
				mapManager.AllSelectionModeClear();
                // �u���b�N��I����Ԃ̕\���ɂ���
                targetBlock.SetSelectionMode(MapBlock.Highlight.Select);

				// �I�������ʒu�ɋ���L�����N�^�[�̃f�[�^���擾
				var charaData =
					charactersManager.GetCharacterDataByPos(targetBlock.xPos, targetBlock.zPos);
				if (charaData != null)
				{// �L�����N�^�[�����݂���
				 // �I�𒆂̃L�����N�^�[���ɋL��
					selectingChara = charaData;
					// �I���L�����N�^�[�̌��݈ʒu���L��
					charaStartPos_X = selectingChara.xPos;
					charaStartPos_Z = selectingChara.zPos;
					// �L�����N�^�[�̃X�e�[�^�X��UI�ɕ\������
					guiManager.ShowStatusWindow(selectingChara);

					// �ړ��\�ȏꏊ���X�g���擾����
					reachableBlocks = mapManager.SearchReachableBlocks(charaData.xPos, charaData.zPos);

					// �ړ��\�ȏꏊ���X�g��\������
					foreach (MapBlock mapBlock in reachableBlocks)
						mapBlock.SetSelectionMode(MapBlock.Highlight.Reachable);

					// �ړ��L�����Z���{�^���\��
					guiManager.ShowMoveCancelButton();
					// �i�s���[�h��i�߂�F�ړ���I��
					ChangePhase(Phase.MyTurn_Moving);
				}
				else
				{// �L�����N�^�[�����݂��Ȃ�
				 // �I�𒆂̃L�����N�^�[��������������
					ClearSelectingChara();
				}
				break;

			// �����̃^�[���F�ړ���I��
			case Phase.MyTurn_Moving:
				Debug.Log("���݂̃t�F�[�Y��-" + nowPhase + "-");
				// �G�L�����N�^�[��I�𒆂Ȃ�ړ����L�����Z�����ďI��
				if (selectingChara.isEnemy)
				{
					CancelMoving();
					break;
				}

				// �I���u���b�N���ړ��\�ȏꏊ���X�g���ɂ���ꍇ�A�ړ��������J�n
				if (reachableBlocks.Contains(targetBlock))
				{
					// �I�𒆂̃L�����N�^�[���ړ�������
					selectingChara.MovePosition(targetBlock.xPos, targetBlock.zPos);

					// �ړ��\�ȏꏊ���X�g������������
					reachableBlocks.Clear();

					// �S�u���b�N�̑I����Ԃ�����
					mapManager.AllSelectionModeClear();

					// �ړ��L�����Z���{�^����\��
					guiManager.HideMoveCancelButton();

					// �w��b���o�ߌ�ɏ��������s����(DoTween)
					DOVirtual.DelayedCall(
						0.5f, // �x������(�b)
						() =>
						{// �x�����s������e
						 // �R�}���h�{�^����\������
							guiManager.ShowCommandButtons();
							// �i�s���[�h��i�߂�F�ړ���̃R�}���h�I��
							ChangePhase(Phase.MyTurn_Command);
						}
					);
				}
				break;

			// �����̃^�[���F�ړ���̃R�}���h�I��
			case Phase.MyTurn_Command:
				Debug.Log("���݂̃t�F�[�Y��-" + nowPhase + "-");
				// �U���͈͂̃u���b�N��I���������A�s�����邩�̊m�F�{�^����\������
				if (attackableBlocks.Contains(targetBlock))
				{
					// �U����̃u���b�N�����L��
					attackBlock = targetBlock;
					// �s������E�L�����Z���{�^����\������
					guiManager.ShowDecideButtons();

					// �U���\�ȏꏊ���X�g������������
					attackableBlocks.Clear();
					// �S�u���b�N�̑I����Ԃ�����
					mapManager.AllSelectionModeClear();

					// �U����̃u���b�N�������\������
					attackBlock.SetSelectionMode(MapBlock.Highlight.Attackable);

					// �i�s���[�h��i�߂�F�U���̑Ώۂ�I��
					ChangePhase(Phase.MyTurn_Targeting);
				}
				break;
		}
	}

	/// <summary>
	/// �I�𒆂̃L�����N�^�[��������������
	/// </summary>
	private void ClearSelectingChara()
	{
		// �I�𒆂̃L�����N�^�[������������
		selectingChara = null;
		// �L�����N�^�[�̃X�e�[�^�XUI���\���ɂ���
		guiManager.HideStatusWindow();
	}

	/// <summary>
	/// �U���R�}���h�{�^������
	/// </summary>
	public void AttackCommand()
	{
		// �U���͈͂��擾���ĕ\������
		GetAttackableBlocks();
	}

	/// <summary>
	/// �U���R�}���h�I����ɑΏۃu���b�N��\�����鏈��
	/// </summary>
	private void GetAttackableBlocks()
	{
		// �R�}���h�{�^�����\���ɂ���
		guiManager.HideCommandButtons();

		// �U���\�ȏꏊ���X�g��\������
		foreach (MapBlock mapBlock in attackableBlocks)
			mapBlock.SetSelectionMode(MapBlock.Highlight.Attackable);
	}

	/// <summary>
	/// �ҋ@�R�}���h�{�^������
	/// </summary>
	public void StandbyCommand()
	{
		// �R�}���h�{�^�����\���ɂ���
		guiManager.HideCommandButtons();
		// �i�s���[�h��i�߂�(�G�̃^�[����)
		ChangePhase(Phase.EnemyTurn_Start);
	}

	/// <summary>
	/// �s�����e����{�^������
	/// </summary>
	public void ActionDecideButton()
	{
		// �s������E�L�����Z���{�^�����\���ɂ���
		guiManager.HideDecideButtons();
		// �U����̃u���b�N�̋����\������������
		attackBlock.SetSelectionMode(MapBlock.Highlight.Off);

		// �U���Ώۂ̈ʒu�ɋ���L�����N�^�[�̃f�[�^���擾
		var targetChara =
			charactersManager.GetCharacterDataByPos(attackBlock.xPos, attackBlock.zPos);
		if (targetChara != null)
		{// �U���Ώۂ̃L�����N�^�[�����݂���
		 // �L�����N�^�[�U������
			CharaAttack(selectingChara, targetChara);

			// �i�s���[�h��i�߂�(�s�����ʕ\����)
			ChangePhase(Phase.MyTurn_Result);
			return;
		}
		else
		{// �U���Ώۂ����݂��Ȃ�
		 // �i�s���[�h��i�߂�(�G�̃^�[����)
			ChangePhase(Phase.EnemyTurn_Start);
		}
	}
	/// <summary>
	/// �s�����e���Z�b�g�{�^������
	/// </summary>
	public void ActionCancelButton()
	{
		// �s������E�L�����Z���{�^�����\���ɂ���
		guiManager.HideDecideButtons();
		// �U����̃u���b�N�̋����\������������
		attackBlock.SetSelectionMode(MapBlock.Highlight.Off);

		// �L�����N�^�[���ړ��O�̈ʒu�ɖ߂�
		selectingChara.MovePosition(charaStartPos_X, charaStartPos_Z);
		// �L�����N�^�[�̑I������������
		ClearSelectingChara();

		// �i�s���[�h��߂�(�^�[���̍ŏ���)
		ChangePhase(Phase.MyTurn_Start, true);
	}

	/// <summary>
	/// �L�����N�^�[�����̃L�����N�^�[�ɍU�����鏈��
	/// </summary>
	/// <param name="attackChara">�U�����L�����f�[�^</param>
	/// <param name="defenseChara">�h�䑤�L�����f�[�^</param>
	private void CharaAttack(Character attackChara, Character defenseChara)
	{
		// �_���[�W�v�Z����
		int damageValue; // �_���[�W��
		int attackPoint = attackChara.atk; // �U�����̍U����
		int defencePoint = defenseChara.def; // �h�䑤�̖h���
		// �_���[�W���U���́|�h��͂Ōv�Z
		damageValue = attackPoint - defencePoint;

		// �A�j���[�V�������ōU���������������炢�̃^�C�~���O��SE���Đ�
		DOVirtual.DelayedCall(
			0.45f, // �x������(�b)
			() =>
			{// �x�����s������e
			 // AudioSource���Đ�
				GetComponent<AudioSource>().Play();
			}
		);

		// �o�g�����ʕ\���E�B���h�E�̕\���ݒ�
		// (HP�̕ύX�O�ɍs��)
		guiManager.battleWindowUI.ShowWindow(defenseChara, damageValue);

		// �_���[�W�ʕ��h�䑤��HP������
		defenseChara.nowHP -= damageValue;
		// HP��0�`�ő�l�͈̔͂Ɏ��܂�悤�␳
		defenseChara.nowHP = Mathf.Clamp(defenseChara.nowHP, 0, defenseChara.maxHP);

		// HP0�ɂȂ����L�����N�^�[���폜����
		if (defenseChara.nowHP == 0)
			charactersManager.DeleteCharaData(defenseChara);

		// �^�[���؂�ւ�����(�x�����s)
		DOVirtual.DelayedCall(
			2.0f, // �x������(�b)
			() =>
			{// �x�����s������e
			 // �E�B���h�E���\����
				guiManager.battleWindowUI.HideWindow();
				// �^�[����؂�ւ���
				if (nowPhase == Phase.MyTurn_Result) // �G�̃^�[����
					ChangePhase(Phase.EnemyTurn_Start);
				else if (nowPhase == Phase.EnemyTurn_Result) // �����̃^�[����
					ChangePhase(Phase.MyTurn_Start);
			}
		);
	}


	/// <summary>
	/// �^�[���i�s���[�h��ύX����
	/// </summary>
	/// <param name="newPhase">�ύX�惂�[�h</param>
	/// <param name="noLogos">���S��\���t���O(�ȗ��\�E�ȗ������false)</param>
	private void ChangePhase(Phase newPhase, bool noLogos = false)
	{
		// �Q�[���I����Ȃ珈�������I��
		if (isGameSet)
			return;

		// ���[�h�ύX��ۑ�
		nowPhase = newPhase;

		// ����̃��[�h�ɐ؂�ւ�����^�C�~���O�ōs������
		switch (nowPhase)
		{
			// �����̃^�[���F�J�n��
			case Phase.MyTurn_Start:
				Debug.Log("���݂̃t�F�[�Y��-" + nowPhase + "-");
				// �����̃^�[���J�n���̃��S��\��
				if (!noLogos)
					guiManager.ShowLogo_PlayerTurn();
				break;

			// �G�̃^�[���F�J�n��
			case Phase.EnemyTurn_Start:
				Debug.Log("���݂̃t�F�[�Y��-" + nowPhase + "-");
				// �G�̃^�[���J�n���̃��S��\��
				if (!noLogos)
					guiManager.ShowLogo_EnemyTurn();

				// �G�̍s�����J�n���鏈��
				// (���S�\����ɊJ�n�������̂Œx�������ɂ���)
				DOVirtual.DelayedCall(
					1.0f, // �x������(�b)
					() =>
					{// �x�����s������e
						EnemyCommand();
					}
				);
				break;
		}
	}

	/// <summary>
	/// (�G�̃^�[���J�n���Ɍďo)
	/// �G�L�����N�^�[�̂��������ꂩ��̂��s�������ă^�[�����I������
	/// </summary>
	private void EnemyCommand()
	{
		// �������̓G�L�����N�^�[�̃��X�g���쐬����
		var enemyCharas = new List<Character>(); // �G�L�����N�^�[���X�g
		foreach (Character charaData in charactersManager.characters)
		{// �S�����L�����N�^�[����G�t���O�̗����Ă���L�����N�^�[�����X�g�ɒǉ�
			if (charaData.isEnemy)
				enemyCharas.Add(charaData);
		}

		// �U���\�ȃL�����N�^�[�E�ʒu�̑g�ݍ��킹�̓��P�������_���Ɏ擾
		var actionPlan = TargetFinder.GetRandomActionPlan
			(mapManager, charactersManager, enemyCharas);
		// �g�ݍ��킹�̃f�[�^�����݂���΍U���J�n
		if (actionPlan != null)
		{
			// �G�L�����N�^�[�ړ�����
			actionPlan.charaData.MovePosition(actionPlan.toMoveBlock.xPos, actionPlan.toMoveBlock.zPos);
			// �G�L�����N�^�[�U������
			// (�ړ���̃^�C�~���O�ōU���J�n����悤�x�����s)
			DOVirtual.DelayedCall(
				1.0f, // �x������(�b)
				() =>
				{// �x�����s������e
					CharaAttack(actionPlan.charaData, actionPlan.toAttackChara);
				}
			);

			// �i�s���[�h��i�߂�(�s�����ʕ\����)
			ChangePhase(Phase.EnemyTurn_Result);
			return;
		}

		// �U���\�ȑ��肪������Ȃ������ꍇ
		// �ړ�������P�̂������_���ɑI��
		int randId = Random.Range(0, enemyCharas.Count);
		Character targetEnemy = enemyCharas[randId]; // �s���Ώۂ̓G�f�[�^
													 // �Ώۂ̈ړ��\�ꏊ���X�g�̒�����1�̏ꏊ�������_���ɑI��
		reachableBlocks =
			mapManager.SearchReachableBlocks(targetEnemy.xPos, targetEnemy.zPos);
		randId = Random.Range(0, reachableBlocks.Count);
		MapBlock targetBlock = reachableBlocks[randId]; // �ړ��Ώۂ̃u���b�N�f�[�^
														// �G�L�����N�^�[�ړ�����
		targetEnemy.MovePosition(targetBlock.xPos, targetBlock.zPos);

		// �ړ��ꏊ�E�U���ꏊ���X�g���N���A����
		reachableBlocks.Clear();
		attackableBlocks.Clear();
		// �i�s���[�h��i�߂�(�����̃^�[����)
		ChangePhase(Phase.MyTurn_Start);
	}

	/// <summary>
	/// �I�𒆂̃L�����N�^�[�̈ړ����͑҂���Ԃ���������
	/// </summary>
	public void CancelMoving()
	{
		// �S�u���b�N�̑I����Ԃ�����
		mapManager.AllSelectionModeClear();
		// �ړ��\�ȏꏊ���X�g������������
		reachableBlocks.Clear();
		// �I�𒆂̃L�����N�^�[��������������
		ClearSelectingChara();
		// �ړ���߂�{�^����\��
		guiManager.HideMoveCancelButton();
		// �t�F�[�Y�����ɖ߂�(���S��\�����Ȃ��ݒ�)
		ChangePhase(Phase.MyTurn_Start, true);
	}

	/// <summary>
	/// �Q�[���̏I�������𖞂������m�F���A�������Ȃ�Q�[�����I������
	/// </summary>
	public void CheckGameSet()
	{
		// �v���C���[�����t���O(�����Ă���G������Ȃ�Off�ɂȂ�)
		bool isWin = true;
		// �v���C���[�s�k�t���O(�����Ă��閡��������Ȃ�Off�ɂȂ�)
		bool isLose = true;

		// ���ꂼ�ꐶ���Ă���G�E���������݂��邩���`�F�b�N
		foreach (var charaData in charactersManager.characters)
		{
			if (charaData.isEnemy) // �G������̂ŏ����t���OOff
				isWin = false;
			else // ����������̂Ŕs�k�t���OOff
				isLose = false;
		}

		// �����܂��͔s�k�̃t���O���������܂܂Ȃ�Q�[�����I������
		// (�ǂ���̃t���O�������Ă��Ȃ��Ȃ牽�������^�[�����i�s����)
		if (isWin || isLose)
		{
			// �Q�[���I���t���O�𗧂Ă�
			isGameSet = true;

			// ���SUI�ƃt�F�[�h�C����\������(�x�����s)
			DOVirtual.DelayedCall(
				1.5f, () =>
				{
					if (isWin) // �Q�[���N���A���o
						guiManager.ShowLogo_GameClear();
					else // �Q�[���I�[�o�[���o
						guiManager.ShowLogo_GameOver();

					// �ړ��\�ȏꏊ���X�g������������
					reachableBlocks.Clear();
					// �S�u���b�N�̑I����Ԃ�����
					mapManager.AllSelectionModeClear();
				}
			);

			// Game�V�[���̍ēǂݍ���(�x�����s)
			DOVirtual.DelayedCall(
				7.0f, () =>
				{
					SceneManager.LoadScene("Enhance");
				}
			);
		}
	}

}