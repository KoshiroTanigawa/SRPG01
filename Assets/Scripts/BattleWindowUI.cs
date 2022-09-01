using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BattleWindowUI : MonoBehaviour
{
	// �o�g�����ʕ\���E�B���h�EUI
	public Text nameText; // ���OText
	public Text hpText; // HPText
	public Text damageText; // �_���[�W��Text

	void Start()
	{
		// ���������ɃE�B���h�E���B��
		HideWindow();
	}

	/// <summary>
	/// �o�g�����ʃE�B���h�E��\������
	/// </summary>
	/// <param name="charaData">�U�����ꂽ�L�����N�^�[�̃f�[�^</param>
	/// <param name="damageValue">�_���[�W��</param>
	public void ShowWindow(Character charaData, int damageValue)
	{
		// �I�u�W�F�N�g�A�N�e�B�u��
		gameObject.SetActive(true);

		// ���OText�\��
		nameText.text = charaData.charaName;

		// �_���[�W�v�Z��̎c��HP���擾����
		// (�����ł͑ΏۃL�����N�^�[�f�[�^��HP�͕ω������Ȃ�)
		int nowHP = charaData.nowHP - damageValue;
		// HP��0�`�ő�l�͈̔͂Ɏ��܂�悤�␳
		nowHP = Mathf.Clamp(nowHP, 0, charaData.maxHP);

		// HPText�\��(���ݒl�ƍő�l������\��)
		hpText.text = nowHP + "/" + charaData.maxHP;
		// �_���[�W��Text�\��
		// �_���[�W��Text�\��
		if (damageValue >= 0)// �_���[�W������
			damageText.text = damageValue + "�_���[�W�I";
		else// HP�񕜎�
			damageText.text = -damageValue + "�񕜁I";
	}
	/// <summary>
	/// �o�g�����ʃE�B���h�E���B��
	/// </summary>
	public void HideWindow()
	{
		// �I�u�W�F�N�g��A�N�e�B�u��
		gameObject.SetActive(false);
	}
}