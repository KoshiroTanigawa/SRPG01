using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform blockParent; // �}�b�v�u���b�N�̐e�I�u�W�F�N�g��Transform
    public GameObject blockPrefab_Metal1; 
    public GameObject blockPrefab_Metal2; 

	// �}�b�v�f�[�^
	public MapBlock[,] mapBlocks;


	// �萔��`
	public const int MAP_WIDTH = 9; // �}�b�v�̉���
    public const int MAP_HEIGHT = 9; // �}�b�v�̏c(���s)�̕�
    private const int GENERATE_RATIO_METAL = 90; // �����u���b�N�����������m��

    void Start()
    {
		// �}�b�v�f�[�^��������
		mapBlocks = new MapBlock[MAP_WIDTH, MAP_HEIGHT];

		// �u���b�N�����ʒu�̊�_�ƂȂ���W��ݒ�
		Vector3 defaultPos = new Vector3(0.0f, 0.0f, 0.0f); // x:0.0f y:0.0f z:0.0f ��Vector3�ϐ�defaultPos��錾
        defaultPos.x = -(MAP_WIDTH / 2); // x���W�̊�_
        defaultPos.z = -(MAP_HEIGHT / 2); // z���W�̊�_

		// �u���b�N��������
		for (int i = 0; i < MAP_WIDTH; i++)
		{// �}�b�v�̉������J��Ԃ�����
			for (int j = 0; j < MAP_HEIGHT; j++)
			{// �}�b�v�̏c�����J��Ԃ�����
			 // �u���b�N�̏ꏊ������
				Vector3 pos = defaultPos; // ��_�̍��W�����ɕϐ�pos��錾
				pos.x += i; // 1�ڂ�for���̌J��Ԃ��񐔕�x���W�����炷
				pos.z += j; // 2�ڂ�for���̌J��Ԃ��񐔕�z���W�����炷

				// �u���b�N�̎�ނ�����
				int rand = Random.Range(0, 100); // 0~99�̒�����1�����_���Ȑ������擾
				bool isGrass = false; // ���u���b�N�����t���O(������Ԃ�false)
									  // �����l�����u���b�N�m���l���Ⴏ��Α��u���b�N�𐶐�����
				if (rand < GENERATE_RATIO_METAL)
					isGrass = true;

				// �I�u�W�F�N�g�𐶐�
				GameObject obj; // ��������I�u�W�F�N�g�̎Q��
				if (isGrass)
				{// ���u���b�N�����t���O�FON
					obj = Instantiate(blockPrefab_Metal1, blockParent); // blockParent�̎q�ɑ��u���b�N�𐶐�
				}
				else
				{// ���u���b�N�����t���O�FOFF
					obj = Instantiate(blockPrefab_Metal2, blockParent); // blockParent�̎q�ɐ���u���b�N�𐶐�
				}
				// �I�u�W�F�N�g�̍��W��K�p
				obj.transform.position = pos;

				// �z��mapBlocks�Ƀu���b�N�f�[�^���i�[
				var mapBlock = obj.GetComponent<MapBlock>(); // �I�u�W�F�N�g��MapBlock���擾
				mapBlocks[i, j] = mapBlock;
				// �u���b�N�f�[�^�ݒ�
				mapBlock.xPos = (int)pos.x; // X�ʒu���L�^
				mapBlock.zPos = (int)pos.z; // Z�ʒu���L�^
			}
		}
	}

	/// <summary>
	/// �S�Ẵu���b�N�̑I����Ԃ���������
	/// </summary>
	public void AllSelectionModeClear()
	{
		for (int i = 0; i < MAP_WIDTH; i++)
			for (int j = 0; j < MAP_HEIGHT; j++)
				mapBlocks[i, j].SetSelectionMode(MapBlock.Highlight.Off);
	}

	/// <summary>
	/// �n���ꂽ�ʒu����L�����N�^�[�����B�ł���ꏊ�̃u���b�N�����X�g�ɂ��ĕԂ�
	/// </summary>
	/// <param name="xPos">��_x�ʒu</param>
	/// <param name="zPos">��_z�ʒu</param>
	/// <returns>�����𖞂����u���b�N�̃��X�g</returns>
	public List<MapBlock> SearchReachableBlocks(int xPos, int zPos)
	{
		// �����𖞂����u���b�N�̃��X�g
		var results = new List<MapBlock>();

		// ��_�ƂȂ�u���b�N�̔z����ԍ�(index)������
		int baseX = -1, baseZ = -1; // �z����ԍ�(�����O��-1�������Ă���)
									// ��������
		for (int i = 0; i < MAP_WIDTH; i++)
		{
			for (int j = 0; j < MAP_HEIGHT; j++)
			{
				if ((mapBlocks[i, j].xPos == xPos) &&
					(mapBlocks[i, j].zPos == zPos))
				{// �w�肳�ꂽ���W�Ɉ�v����}�b�v�u���b�N�𔭌�
				 // �z����ԍ����擾���ă��[�v���I��
					baseX = i;
					baseZ = j;
					break; // 2�ڂ̃��[�v�𔲂���
				}
			}
			// ���ɔ����ς݂Ȃ�1�ڂ̃��[�v�𔲂���
			if (baseX != -1)
				break;
		}

		// �ړ�����L�����N�^�[�̈ړ����@���擾
		var moveType = Character.MoveType.Rook; // �ړ����@
		var moveChara = GetComponent<CharactersManager>().GetCharacterDataByPos(xPos, zPos); // �ړ�����L����
		if (moveChara != null)
			moveType = moveChara.moveType; // �L�����N�^�[�f�[�^����ړ����@���擾

		// �L�����N�^�[�̈ړ����@�ɍ��킹�ĈقȂ�����̃u���b�N�f�[�^���擾���Ă���
		// �c�E��
		if (moveType == Character.MoveType.Rook ||
			moveType == Character.MoveType.Queen)
		{
			// X+����
			for (int i = baseX + 1; i < MAP_WIDTH; i++)
				if (AddReachableList(results, mapBlocks[i, baseZ]))
					break;
			// X-����
			for (int i = baseX - 1; i >= 0; i--)
				if (AddReachableList(results, mapBlocks[i, baseZ]))
					break;
			// Z+����
			for (int j = baseZ + 1; j < MAP_HEIGHT; j++)
				if (AddReachableList(results, mapBlocks[baseX, j]))
					break;
			// Z-����
			for (int j = baseZ - 1; j >= 0; j--)
				if (AddReachableList(results, mapBlocks[baseX, j]))
					break;
		}
		// �΂߂ւ̈ړ�
		if (moveType == Character.MoveType.Bishop ||
			moveType == Character.MoveType.Queen)
		{
			// X+Z+����
			for (int i = baseX + 1, j = baseZ + 1;
				i < MAP_WIDTH && j < MAP_HEIGHT;
				i++, j++)
				if (AddReachableList(results, mapBlocks[i, j]))
					break;
			// X-Z+����
			for (int i = baseX - 1, j = baseZ + 1;
				i >= 0 && j < MAP_HEIGHT;
				i--, j++)
				if (AddReachableList(results, mapBlocks[i, j]))
					break;
			// X+Z-����
			for (int i = baseX + 1, j = baseZ - 1;
				i < MAP_WIDTH && j >= 0;
				i++, j--)
				if (AddReachableList(results, mapBlocks[i, j]))
					break;
			// X-Z-����
			for (int i = baseX - 1, j = baseZ - 1;
				i >= 0 && j >= 0;
				i--, j--)
				if (AddReachableList(results, mapBlocks[i, j]))
					break;
		}
		// �����̃u���b�N
		results.Add(mapBlocks[baseX, baseZ]);

		return results;
	}

	/// <summary>
	/// (�L�����N�^�[���B�u���b�N���������p)
	/// �w�肵���u���b�N�𓞒B�\�u���b�N���X�g�ɒǉ�����
	/// </summary>
	/// <param name="reachableList">���B�\�u���b�N���X�g</param>
	/// <param name="targetBlock">�Ώۃu���b�N</param>
	/// <returns>�s���~�܂�t���O(�s���~�܂�Ȃ�true���Ԃ�)</returns>
	private bool AddReachableList(List<MapBlock> reachableList, MapBlock targetBlock)
	{
		// �Ώۂ̃u���b�N���ʍs�s�Ȃ炻�����s���~�܂�Ƃ��ďI��
		if (!targetBlock.passable)
			return true;

		// �Ώۂ̈ʒu�ɑ��̃L���������ɂ���Ȃ瓞�B�s�ɂ��ďI��(�s���~�܂�ɂ͂��Ȃ�)
		var charaData =
			GetComponent<CharactersManager>().GetCharacterDataByPos(targetBlock.xPos, targetBlock.zPos);
		if (charaData != null)
			return false;

		// ���B�\�u���b�N���X�g�ɒǉ�����
		reachableList.Add(targetBlock);
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
		// �����𖞂����}�b�v�u���b�N�̃��X�g
		var results = new List<MapBlock>();

		// ��_�ƂȂ�u���b�N�̔z����ԍ�(index)������
		int baseX = -1, baseZ = -1; // �z����ԍ�(�����O��-1�������Ă���)
									// ��������
		for (int i = 0; i < MAP_WIDTH; i++)
		{
			for (int j = 0; j < MAP_HEIGHT; j++)
			{
				if ((mapBlocks[i, j].xPos == xPos) &&
					(mapBlocks[i, j].zPos == zPos))
				{// �w�肳�ꂽ���W�Ɉ�v����}�b�v�u���b�N�𔭌�
				 // �z����ԍ����擾���ă��[�v���I��
					baseX = i;
					baseZ = j;
					break; // 2�ڂ̃��[�v�𔲂���
				}
			}
			// ���ɔ����ς݂Ȃ�1�ڂ̃��[�v�𔲂���
			if (baseX != -1)
				break;
		}

		// 4������1�}�X�i�񂾈ʒu�̃u���b�N�����ꂼ��Z�b�g
		// (�c�E��4�}�X)
		// X+����
		AddAttackableList(results, baseX + 1, baseZ);
		// X-����
		AddAttackableList(results, baseX - 1, baseZ);
		// Z+����
		AddAttackableList(results, baseX, baseZ + 1);
		// Z-����
		AddAttackableList(results, baseX, baseZ - 1);
		// (�΂�4�}�X)
		// X+Z+����
		AddAttackableList(results, baseX + 1, baseZ + 1);
		// X-Z+����
		AddAttackableList(results, baseX - 1, baseZ + 1);
		// X+Z-����
		AddAttackableList(results, baseX + 1, baseZ - 1);
		// X-Z-����
		AddAttackableList(results, baseX - 1, baseZ - 1);

		return results;
	}

	/// <summary>
	/// (�L�����N�^�[�U���\�u���b�N���������p)
	/// �}�b�v�f�[�^�̎w�肳�ꂽ�z����ԍ��ɑΉ�����u���b�N���U���\�u���b�N���X�g�ɒǉ�����
	/// </summary>
	/// <param name="attackableList">�U���\�u���b�N���X�g</param>
	/// <param name="indexX">X�����̔z����ԍ�</param>
	/// <param name="indexZ">Z�����̔z����ԍ�</param>
	private void AddAttackableList(List<MapBlock> attackableList, int indexX, int indexZ)
	{
		// �w�肳�ꂽ�ԍ����z��̊O�ɏo�Ă�����ǉ������I��
		if (indexX < 0 || indexX >= MAP_WIDTH ||
			indexZ < 0 || indexZ >= MAP_HEIGHT)
			return;

		// ���B�\�u���b�N���X�g�ɒǉ�����
		attackableList.Add(mapBlocks[indexX, indexZ]);
	}

	/// <summary>
	/// �}�b�v�f�[�^�z������X�g�ɂ��ĕԂ�
	/// </summary>
	/// <returns>�}�b�v�f�[�^�̃��X�g</returns>
	public List<MapBlock> MapBlocksToList()
	{
		// ���ʗp���X�g
		var results = new List<MapBlock>();

		// �}�b�v�f�[�^�z��̒��g�����ԂɃ��X�g�Ɋi�[
		for (int i = 0; i < MAP_WIDTH; i++)
		{
			for (int j = 0; j < MAP_HEIGHT; j++)
			{
				results.Add(mapBlocks[i, j]);
			}
		}

		return results;
	}

}
