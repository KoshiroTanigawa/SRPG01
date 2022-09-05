using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	//�t�B�[���h�ϐ�
    [SerializeField]Transform _blockParent;
	[SerializeField] GameObject _blockPrefab_Metal;
	[SerializeField] MapBlock[,] _mapBlocks;

	[Tooltip("�}�b�v�̉���")] int _mapWidth = 9;
	[Tooltip("�}�b�v�̏c�̕�")] int _mapHeight = 9;
	//

    void Start()
    {
		_mapBlocks = new MapBlock[_mapWidth, _mapHeight];
		Vector3 defPos = new Vector3(0.0f, 0.0f, 0.0f);		// �u���b�N�����̊�_�ƂȂ���W
		defPos.x = -(_mapWidth / 2);	// x���W�̊�_
        defPos.z = -(_mapHeight / 2);	// z���W�̊�_
		for (int i = 0; i < _mapWidth; i++)
		{
			for (int j = 0; j < _mapHeight; j++)
			{
				Vector3 pos = defPos;	// ��_�̍��W�����ɕϐ�pos��錾
				pos.x += i;		// �J��Ԃ��񐔕�x���W�����炷
				pos.z += j;		// �J��Ԃ��񐔕�z���W�����炷

				GameObject obj;		// ��������I�u�W�F�N�g�̎Q��
				obj = Instantiate(_blockPrefab_Metal, _blockParent); 
				obj.transform.position = pos;	// �I�u�W�F�N�g�̍��W��K�p

				var mapBlock = obj.GetComponent<MapBlock>();
				_mapBlocks[i, j] = mapBlock;
				mapBlock.xPos = (int)pos.x;		// X�ʒu���L�^
				mapBlock.zPos = (int)pos.z;		// Z�ʒu���L�^
			}
		}
	}

	/// <summary>
	/// �S�Ẵu���b�N�̑I����Ԃ��������鏈��
	/// </summary>
	public void AllSelectClear()
	{
		for (int i = 0; i < _mapWidth; i++)
		{
			for (int j = 0; j < _mapHeight; j++)
			{
				_mapBlocks[i, j].SetSelectionMode(MapBlock.Highlight.Off);
			}
		}
	}

	/// <summary>
	/// �n���ꂽ�ʒu����L�����N�^�[�����B�ł���u���b�N�����X�g�ɂ��ĕԂ�
	/// </summary>
	/// <param name="xPos">��_x�ʒu</param>
	/// <param name="zPos">��_z�ʒu</param>
	/// <returns>�����𖞂����u���b�N�̃��X�g</returns>
	public List<MapBlock> SearchReachableBlocks(int xPos, int zPos)
	{
		var results = new List<MapBlock>();

		int baseX = -1;
		int	baseZ = -1;
		for (int i = 0; i < _mapWidth; i++)// ��_�ƂȂ�u���b�N�̔z����ԍ�(index)������
		{
			for (int j = 0; j < _mapHeight; j++)
			{
				if ((_mapBlocks[i, j].xPos == xPos) && (_mapBlocks[i, j].zPos == zPos))
				{
					baseX = i;  // �z����ԍ����擾
					baseZ = j;
					break;
				}
			}
			if (baseX != -1)// ���ɔ����ς݂Ȃ�1�ڂ̃��[�v�𔲂���
			{
				break;
			}
		}

		// �ړ�����L�����N�^�[�̈ړ����@���擾
		var moveType = Character.MoveType.Rook; 
		var moveChara = GetComponent<CharactersManager>().GetCharacterDataPos(xPos, zPos);
		if (moveChara != null)
		{
			moveType = moveChara.moveType; // �L�����N�^�[�f�[�^����ړ����@���擾
		}

		// �L�����N�^�[�̈ړ����@�ɍ��킹�ĈقȂ�����̃u���b�N�f�[�^���擾���Ă���
		if (moveType == Character.MoveType.Rook || moveType == Character.MoveType.Queen)	// �c�E��
		{
			// X+����
			for (int i = baseX + 1; i < _mapWidth; i++)
				if (AddReachableList(results, _mapBlocks[i, baseZ]))
					break;
			// X-����
			for (int i = baseX - 1; i >= 0; i--)
				if (AddReachableList(results, _mapBlocks[i, baseZ]))
					break;
			// Z+����
			for (int j = baseZ + 1; j < _mapHeight; j++)
				if (AddReachableList(results, _mapBlocks[baseX, j]))
					break;
			// Z-����
			for (int j = baseZ - 1; j >= 0; j--)
				if (AddReachableList(results, _mapBlocks[baseX, j]))
					break;
		}
		if (moveType == Character.MoveType.Bishop || moveType == Character.MoveType.Queen)	// �΂߂ւ̈ړ�
		{
			// X+Z+����
			for (int i = baseX + 1, j = baseZ + 1;
				i < _mapWidth && j < _mapHeight;
				i++, j++)
				if (AddReachableList(results, _mapBlocks[i, j]))
					break;
			// X-Z+����
			for (int i = baseX - 1, j = baseZ + 1;
				i >= 0 && j < _mapHeight;
				i--, j++)
				if (AddReachableList(results, _mapBlocks[i, j]))
					break;
			// X+Z-����
			for (int i = baseX + 1, j = baseZ - 1;
				i < _mapWidth && j >= 0;
				i++, j--)
				if (AddReachableList(results, _mapBlocks[i, j]))
					break;
			// X-Z-����
			for (int i = baseX - 1, j = baseZ - 1;
				i >= 0 && j >= 0;
				i--, j--)
				if (AddReachableList(results, _mapBlocks[i, j]))
					break;
		}
		results.Add(_mapBlocks[baseX, baseZ]);	// �����̃u���b�N
		return results;
	}

	/// <summary>
	/// �w�肵���u���b�N�𓞒B�\�u���b�N���X�g�ɒǉ�����
	/// </summary>
	/// <param name="reachableList">���B�\�u���b�N���X�g</param>
	/// <param name="targetBlock">�Ώۃu���b�N</param>
	/// <returns>�s���~�܂�t���O(�s���~�܂�Ȃ�true���Ԃ�)</returns>
	private bool AddReachableList(List<MapBlock> reachableList, MapBlock targetBlock)
	{
		if (!targetBlock.passable)// �Ώۂ̃u���b�N���ʍs�s�Ȃ炻�����s���~�܂�Ƃ��ďI��
		{
			return true;
		}

		var charaData = GetComponent<CharactersManager>().GetCharacterDataPos(targetBlock.xPos, targetBlock.zPos);
		if (charaData != null)
		{
			return false;
		}
		reachableList.Add(targetBlock);// ���B�\�u���b�N���X�g�ɒǉ�����
		return false;
	}

	/// <summary>
	/// �n���ꂽ�ʒu����L�����N�^�[���U���ł���ꏊ�̃}�b�v�u���b�N�����X�g�ɂ��ĕԂ�
	/// </summary>
	/// <param name="xPos">��_x�ʒu</param>
	/// <param name="zPos">��_z�ʒu</param>
	/// <returns>�����𖞂����}�b�v�u���b�N�̃��X�g</returns>
	public List<MapBlock> SearchAttackableBlocks(int xPos, int zPos)
	{
		var results = new List<MapBlock>();
		int baseX = -1;
		int baseZ = -1;
		for (int i = 0; i < _mapWidth; i++)
		{
			for (int j = 0; j < _mapHeight; j++)
			{
				if ((_mapBlocks[i, j].xPos == xPos) && (_mapBlocks[i, j].zPos == zPos))
				{
					baseX = i;
					baseZ = j;
					break;
				}
			}

			if (baseX != -1)// ���ɔ����ς݂Ȃ�1�ڂ̃��[�v�𔲂���
			{
				break;
			}
		}

		// 4������1�}�X�i�񂾈ʒu�̃u���b�N�����ꂼ��Z�b�g
		// (�c�E��4�}�X)
		AddAttackableList(results, baseX + 1, baseZ);	// X+����
		AddAttackableList(results, baseX - 1, baseZ);	// X-����
		AddAttackableList(results, baseX, baseZ + 1);	// Z+����
		AddAttackableList(results, baseX, baseZ - 1);	// Z-����
		// (�΂�4�}�X)
		AddAttackableList(results, baseX + 1, baseZ + 1);	// X+Z+����
		AddAttackableList(results, baseX - 1, baseZ + 1);	// X-Z+����
		AddAttackableList(results, baseX + 1, baseZ - 1);	// X+Z-����
		AddAttackableList(results, baseX - 1, baseZ - 1);	// X-Z-����

		return results;
	}

	/// <summary>
	/// �}�b�v�f�[�^�̎w�肳�ꂽ�z����ԍ��ɑΉ�����u���b�N���U���\�u���b�N���X�g�ɒǉ�����
	/// </summary>
	/// <param name="attackableList">�U���\�u���b�N���X�g</param>
	/// <param name="indexX">X�����̔z����ԍ�</param>
	/// <param name="indexZ">Z�����̔z����ԍ�</param>
	private void AddAttackableList(List<MapBlock> attackableList, int indexX, int indexZ)
	{
		if (indexX < 0 || indexX >= _mapWidth || indexZ < 0 || indexZ >= _mapHeight)// �w�肳�ꂽ�ԍ����z��̊O�ɏo�Ă�����ǉ������I��
		{
			return;
		}
		attackableList.Add(_mapBlocks[indexX, indexZ]);// ���B�\�u���b�N���X�g�ɒǉ�����
	}

	/// <summary>
	/// �}�b�v�f�[�^�z������X�g�ɂ��ĕԂ�
	/// </summary>
	/// <returns>�}�b�v�f�[�^�̃��X�g</returns>
	public List<MapBlock> MapBlocksToList()
	{
		var results = new List<MapBlock>();
		for (int i = 0; i < _mapWidth; i++)		// �}�b�v�f�[�^�z��̒��g�����ԂɃ��X�g�Ɋi�[
		{
			for (int j = 0; j < _mapHeight; j++)
			{
				results.Add(_mapBlocks[i, j]);
			}
		}
		return results;
	}
}
