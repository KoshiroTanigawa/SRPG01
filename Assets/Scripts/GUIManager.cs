using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI�R���|�[�l���g�������̂ɕK�v
using DG.Tweening;

public class GUIManager : MonoBehaviour
{
	// �X�e�[�^�X�E�B���h�EUI
	public GameObject statusWindow; // �X�e�[�^�X�E�B���h�E�I�u�W�F�N�g
	public Text nameText; // ���OText
	public Text hpName; // HP
	public Text hpText; // HPText

	// �L�����N�^�[�̃R�}���h�{�^��
	public GameObject commandButtons; // �S�R�}���h�{�^���̐e�I�u�W�F�N�g

	// �o�g�����ʕ\��UI�����N���X
	public BattleWindowUI battleWindowUI;

	// �e�탍�S�摜
	public Image playerTurnImage; // �v���C���[�^�[���J�n���摜
	public Image enemyTurnImage; // �G�^�[���J�n���摜
	public Image gameClearImage; // �Q�[���N���A�摜
	public Image gameOverImage; // �Q�[���I�[�o�[�摜

	// �ړ��L�����Z���{�^��UI
	public GameObject moveCancelButton;

	// �s������E�L�����Z���{�^��UI
	public GameObject decideButtons;

	void Start()
	{
		// UI������
		HideStatusWindow(); // �X�e�[�^�X�E�B���h�E���B��
		HideCommandButtons(); // �R�}���h�{�^�����B��
		HideMoveCancelButton(); // �ړ��L�����Z���{�^�����B��
		HideDecideButtons(); // �s������E�L�����Z���{�^�����B��
	}

	/// <summary>
	/// �X�e�[�^�X�E�B���h�E��\������
	/// </summary>
	/// <param name="charaData">�\���L�����N�^�[�f�[�^</param>
	public void ShowStatusWindow(Character charaData)
	{
		// �I�u�W�F�N�g�A�N�e�B�u��
		statusWindow.SetActive(true);

		// ���OText�\��
		nameText.text = charaData.charaName;
	}

	/// <summary>
	/// �X�e�[�^�X�E�B���h�E���B��
	/// </summary>
	public void HideStatusWindow()
	{
		// �I�u�W�F�N�g��A�N�e�B�u��
		statusWindow.SetActive(false);
	}

	/// <summary>
	/// �R�}���h�{�^����\������
	/// </summary>
	public void ShowCommandButtons()
	{
		commandButtons.SetActive(true);
	}

	/// <summary>
	/// �R�}���h�{�^�����B��
	/// </summary>
	public void HideCommandButtons()
	{
		commandButtons.SetActive(false);
	}

	/// <summary>
	/// �v���C���[�̃^�[���ɐ؂�ւ�������̃��S�摜��\������
	/// </summary>
	public void ShowLogo_PlayerTurn()
	{
		// ���X�ɕ\������\�����s���A�j���[�V����(Tween)
		playerTurnImage
			.DOFade(1.0f, // �w�萔�l�܂ŉ摜��alpha�l��ω�
				1.0f) // �A�j���[�V��������(�b)
			.SetEase(Ease.OutCubic) // �C�[�W���O(�ω��̓x��)��ݒ�
			.SetLoops(2, LoopType.Yoyo); // ���[�v�񐔁E�������w��
	}
	/// <summary>
	/// �G�̃^�[���ɐ؂�ւ�������̃��S�摜��\������
	/// </summary>
	public void ShowLogo_EnemyTurn()
	{
		// ���X�ɕ\������\�����s���A�j���[�V����(Tween)
		enemyTurnImage
			.DOFade(1.0f, // �w�萔�l�܂ŉ摜��alpha�l��ω�
				1.0f) // �A�j���[�V��������(�b)
			.SetEase(Ease.OutCubic) // �C�[�W���O(�ω��̓x��)��ݒ�
			.SetLoops(2, LoopType.Yoyo); // ���[�v�񐔁E�������w��
	}

	/// <summary>
	/// �ړ��L�����Z���{�^����\������
	/// </summary>
	public void ShowMoveCancelButton()
	{
		moveCancelButton.SetActive(true);
	}
	/// <summary>
	/// �ړ��L�����Z���{�^�����\���ɂ���
	/// </summary>
	public void HideMoveCancelButton()
	{
		moveCancelButton.SetActive(false);
	}

	/// <summary>
	/// �Q�[���N���A���̃��S�摜��\������
	/// </summary>
	public void ShowLogo_GameClear()
	{
		// ���X�ɕ\������A�j���[�V����
		gameClearImage
			.DOFade(1.0f, // �w�萔�l�܂ŉ摜��alpha�l��ω�
				1.0f) // �A�j���[�V��������(�b)
			.SetEase(Ease.OutCubic); // �C�[�W���O(�ω��̓x��)��ݒ�

		// �g�偨�k�����s���A�j���[�V����
		gameClearImage.transform
			.DOScale(1.5f, // �w�萔�l�܂ŉ摜��alpha�l��ω�
				1.0f) // �A�j���[�V��������(�b)
			.SetEase(Ease.OutCubic) // �C�[�W���O(�ω��̓x��)��ݒ�
			.SetLoops(2, LoopType.Yoyo); // ���[�v�񐔁E�������w��
	}
	/// <summary>
	/// �Q�[���I�[�o�[�̃��S�摜��\������
	/// </summary>
	public void ShowLogo_GameOver()
	{
		// ���X�ɕ\������A�j���[�V����
		gameOverImage.
			DOFade(1.0f, // �w�萔�l�܂ŉ摜��alpha�l��ω�
				1.0f) // �A�j���[�V��������(�b)
			.SetEase(Ease.OutCubic); // �C�[�W���O(�ω��̓x��)��ݒ�
	}

	/// <summary>
	/// �s������E�L�����Z���{�^����\������
	/// </summary>
	public void ShowDecideButtons()
	{
		decideButtons.SetActive(true);
	}
	/// <summary>
	/// �s������E�L�����Z���{�^�����\���ɂ���
	/// </summary>
	public void HideDecideButtons()
	{
		decideButtons.SetActive(false);
	}

}