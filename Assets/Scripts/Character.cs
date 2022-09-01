using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Character : MonoBehaviour
{
	// ���C���J����
	private Camera mainCamera;

	// �L�����N�^�[�����ݒ�(�C���X�y�N�^�������)
	[Header("����X�ʒu(-4�`4)")]
	public int initPos_X; // ����X�ʒu
	[Header("����Z�ʒu(-4�`4)")]
	public int initPos_Z; // ����Z�ʒu
	[Header("�G�t���O(ON�œG�L�����Ƃ��Ĉ���)")]
	public bool isEnemy; // �G�t���O
	// �L�����N�^�[�f�[�^(�����X�e�[�^�X)
	[Header("�L�����N�^�[��")]
	public string charaName; // �L�����N�^�[��
	[Header("�ő�HP(����HP)")]
	public int maxHP; // �ő�HP
	[Header("�U����")]
	public int atk; // �U����
	[Header("�h���")]
	public int def; // �h���
	[Header("�ړ����@")]
	public MoveType moveType; // �ړ����@
	

	// �Q�[�����ɕω�����L�����N�^�[�f�[�^
	[HideInInspector]
	public int xPos; // ���݂�x���W
	[HideInInspector]
	public int zPos; // ���݂�z���W
	[HideInInspector]
	public int nowHP; // ����HP

	// �L�����N�^�[�ړ����@��`(�񋓌^)
	public enum MoveType
	{
		Rook, // �c�E��
		Bishop, // �΂�
		Queen, // �c�E���E�΂�
	}

	void Start()
	{

		// �����ʒu�ɑΉ�������W�փI�u�W�F�N�g���ړ�������
		Vector3 pos = new Vector3();
		pos.x = initPos_X; // x���W�F1�u���b�N�̃T�C�Y��1(1.0f)�Ȃ̂ł��̂܂ܑ��
		pos.y = 1.0f; // y���W�i�Œ�j
		pos.z = initPos_Z; // z���W
		transform.position = pos; // �I�u�W�F�N�g�̍��W��ύX

		// ���̑��ϐ�������
		xPos = initPos_X;
		zPos = initPos_Z;
		nowHP = maxHP;
	}

	/// <summary>
	/// �Ώۂ̍��W�ւƃL�����N�^�[���ړ�������
	/// </summary>
	/// <param name="targetXPos">x���W</param>
	/// <param name="targetZPos">z���W</param>
	public void MovePosition(int targetXPos, int targetZPos)
	{
		// �I�u�W�F�N�g���ړ�������
		// �ړ�����W�ւ̑��΍��W���擾
		Vector3 movePos = Vector3.zero; // (0.0f, 0.0f, 0.0f)��Vector3�ŏ�����
		movePos.x = targetXPos - xPos; // x�����̑��΋���
		movePos.z = targetZPos - zPos; // z�����̑��΋���

		// DoTween��Tween���g�p���ď��X�Ɉʒu���ω�����A�j���[�V�������s��
		transform.DOMove(movePos, // �w����W�܂ňړ�����
				0.5f) // �A�j���[�V��������(�b)
			.SetEase(Ease.Linear) // �C�[�W���O(�ω��̓x��)��ݒ�
			.SetRelative(); // �p�����[�^�𑊑Ύw��ɂ���

		// �L�����N�^�[�f�[�^�Ɉʒu��ۑ�
		xPos = targetXPos;
		zPos = targetZPos;
	}

	/// <summary>
	/// �L�����N�^�[�̋ߐڍU���A�j���[�V����
	/// </summary>
	/// <param name="targetChara">����L�����N�^�[</param>
	public void AttackAnimation(Character targetChara)
	{
		// �U���A�j���[�V����(DoTween)
		// ����L�����N�^�[�̈ʒu�փW�����v�ŋ߂Â��A���������Ō��̏ꏊ�ɖ߂�
		transform.DOJump(targetChara.transform.position, // �w����W�܂ŃW�����v���Ȃ���ړ�����
				1.0f, // �W�����v�̍���
				1, // �W�����v��
				0.5f) // �A�j���[�V��������(�b)
			.SetEase(Ease.Linear) // �C�[�W���O(�ω��̓x��)��ݒ�
			.SetLoops(2, LoopType.Yoyo); // ���[�v�񐔁E�������w��
	}
}