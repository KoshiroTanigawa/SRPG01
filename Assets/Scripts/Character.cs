using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Character : MonoBehaviour
{
	// ���C���J����
	private Camera mainCamera;

	// �L�����N�^�[�����ʒu
	[SerializeField] [Header("����X�ʒu(-4�`4)")] int _startPosX;
	[SerializeField] [Header("����Z�ʒu(-4�`4)")] int _startPosZ;

	//�G�l�~�[�t���O
	[Header("�G�t���O(ON�œG�L�����Ƃ��Ĉ���)")] public bool _isEnemy;

	// �L�����N�^�[�f�[�^(�����X�e�[�^�X)
	[Header("�L�����N�^�[��")] public string charaName;
	[Header("�ő�HP(����HP)")] public int maxHP;
	[Header("�U����")] public int atk;
	[Header("�h���")] public int def;
	[Header("�ړ����@")] public MoveType moveType;
	

	// �Q�[�����ɕω�����L�����N�^�[�f�[�^
	[HideInInspector] public int xPos; // ���݂�x���W
	[HideInInspector] public int zPos; // ���݂�z���W
	[HideInInspector] public int nowHP; // ����HP

	// �L�����N�^�[�ړ����@
	public enum MoveType
	{
		Rook, // �c�E��
		Bishop, // �΂�
		Queen, // �c�E���E�΂�
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
	/// �Ώۂ̍��W�ւƃL�����N�^�[���ړ�������
	/// </summary>
	/// <param name="targetXPos">x���W</param>
	/// <param name="targetZPos">z���W</param>
	public void MovePosition(int targetXPos, int targetZPos)
	{
		Vector3 movePos = Vector3.zero; 
		movePos.x = targetXPos - xPos; // x�����̑��΋���
		movePos.z = targetZPos - zPos; // z�����̑��΋���

		/*
		transform.DOMove(movePos, // �w����W�܂ňړ�����
				0.5f) // �A�j���[�V��������(�b)
			.SetEase(Ease.Linear) // �C�[�W���O(�ω��̓x��)��ݒ�
			.SetRelative(); // �p�����[�^�𑊑Ύw��ɂ���
		*/

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