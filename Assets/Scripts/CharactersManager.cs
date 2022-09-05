using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CharactersManager : MonoBehaviour
{
	public Transform _charaParent; // �S�L�����N�^�[�I�u�W�F�N�g�̐e�I�u�W�F�N�gTransform
	[HideInInspector] public List<Character> characters;// �S�L�����N�^�[�f�[�^

	void Start()
	{
		characters = new List<Character>();
		_charaParent.GetComponentsInChildren(characters);//charactersParent�ȉ��̑SCharacter�R���|�[�l���g�����������X�g�Ɋi�[
	}

	/// <summary>
	/// �w�肵���ʒu�ɑ��݂���L�����N�^�[�f�[�^���������ĕԂ�
	/// </summary>
	/// <param name="xPos">X�ʒu</param>
	/// <param name="zPos">Z�ʒu</param>
	/// <returns>�Ώۂ̃L�����N�^�[�f�[�^</returns>
	public Character GetCharacterDataPos(int xPos, int zPos)
	{
		foreach (Character charaData in characters)
		{
			if ((charaData.xPos == xPos) && (charaData.zPos == zPos))
			{
				return charaData; 
			}
		}
		return null;
	}

	/// <summary>
	/// �w�肵���L�����N�^�[���폜����
	/// </summary>
	/// <param name="charaData">�ΏۃL�����f�[�^</param>
	public void DeleteCharaData(Character charaData)
	{
		characters.Remove(charaData);// ���X�g����f�[�^���폜
		DOVirtual.DelayedCall(0.5f,() =>{Destroy(charaData.gameObject);});
		GetComponent<GameManager>().CheckGameSet();// �Q�[���I��������s��
	}
}