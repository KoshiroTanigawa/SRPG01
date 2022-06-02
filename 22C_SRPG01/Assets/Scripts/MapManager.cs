using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform blockParent; // �}�b�v�u���b�N�̐e�I�u�W�F�N�g��Transform
    public GameObject blockPrefab_MossStone; // �ې΃u���b�N
	public GameObject blockPrefab_HardStone; // �d���΃u���b�N
	public GameObject blockPrefab_DeepWater; // ���u���b�N

	// �萔���`
	public const int MAP_WIDTH = 15; // �}�b�v�̉���
	public const int MAP_HEIGHT = 15; // �}�b�v�̏c��
	public const int  genarateMossStone = 80; // �ې΃u���b�N�����������m��
	public const int generateHardStone = 10; // �d���΃u���b�N�����������m��

	void Start()
    {
        Vector3 defaultPos = DefaultPos();
        Generateblock(defaultPos);
    }

    private static Vector3 DefaultPos()
    {
        // �u���b�N�����ʒu�̊�_�ƂȂ���W��ݒ�
        Vector3 defaultPos = new Vector3(0f, 0f, 0f);
        defaultPos.x = -(MAP_WIDTH / 2); // x���W�̊�_
        defaultPos.z = -(MAP_HEIGHT / 2); // z���W�̊�_
        return defaultPos;
    }

    private void Generateblock(Vector3 defaultPos)
    {
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

                // �I�u�W�F�N�g�𐶐�
                GameObject obj; // ��������I�u�W�F�N�g�̎Q��

                if (rand < genarateMossStone && rand > generateHardStone)
                {
                    
                    obj = Instantiate(blockPrefab_MossStone, blockParent); // blockParent�̎q�ɑ��u���b�N�𐶐�
                }

                else if ( rand <generateHardStone )
                {
                    obj = Instantiate(blockPrefab_HardStone, blockParent); // blockParent�̎q�ɍd���΃u���b�N�𐶐�
                }

                else
                {
                    obj = Instantiate(blockPrefab_DeepWater, blockParent); // blockParent�̎q�ɍd���΃u���b�N�𐶐�
                }

                obj.transform.position = pos; // �I�u�W�F�N�g�̍��W��K�p
            }
        }
    }
}
