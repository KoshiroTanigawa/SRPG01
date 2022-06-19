using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CharactersManager : MonoBehaviour
{
	// �I�u�W�F�N�g
	public Transform charactersParent; // �S�L�����N�^�[�I�u�W�F�N�g�̐e�I�u�W�F�N�gTransform

	// �S�L�����N�^�[�f�[�^
	[HideInInspector]
	public List<Character> characters;

	void Start()
	{
		// �}�b�v��̑S�L�����N�^�[�f�[�^���擾
		// (charactersParent�ȉ��̑SCharacter�R���|�[�l���g�����������X�g�Ɋi�[)
		characters = new List<Character>();
		charactersParent.GetComponentsInChildren(characters);
	}

	/// <summary>
	/// �w�肵���ʒu�ɑ��݂���L�����N�^�[�f�[�^���������ĕԂ�
	/// </summary>
	/// <param name="xPos">X�ʒu</param>
	/// <param name="zPos">Z�ʒu</param>
	/// <returns>�Ώۂ̃L�����N�^�[�f�[�^</returns>
	public Character GetCharacterDataByPos(int xPos, int zPos)
	{
		// ��������
		// (foreach�Ń}�b�v���̑S�L�����N�^�[�f�[�^�P�̂P�̂��ɓ����������s��)
		foreach (Character charaData in characters)
		{
			// �L�����N�^�[�̈ʒu���w�肳�ꂽ�ʒu�ƈ�v���Ă��邩�`�F�b�N
			if ((charaData.xPos == xPos) && // X�ʒu������
				(charaData.zPos == zPos)) // Z�ʒu������
			{// �ʒu����v���Ă���
				return charaData; // �f�[�^��Ԃ��ďI��
			}
		}

		// �f�[�^��������Ȃ����null��Ԃ�
		return null;
	}

	/// <summary>
	/// �w�肵���L�����N�^�[���폜����
	/// </summary>
	/// <param name="charaData">�ΏۃL�����f�[�^</param>
	public void DeleteCharaData(Character charaData)
	{
		// ���X�g����f�[�^���폜
		characters.Remove(charaData);
		// �I�u�W�F�N�g�폜(�x�����s)
		DOVirtual.DelayedCall(
			0.5f, // �x������(�b)
			() =>
			{// �x�����s������e
				Destroy(charaData.gameObject);
			}
		);
		// �Q�[���I��������s��
		GetComponent<GameManager>().CheckGameSet();
	}
}