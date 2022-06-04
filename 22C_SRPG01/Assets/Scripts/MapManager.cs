using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform blockParent; // �}�b�v�u���b�N�̐e�I�u�W�F�N�g��Transform
    public GameObject blockPrefab_MossStone; // �ې΃u���b�N
    public GameObject blockPrefab_HardStone; // �΃u���b�N
    public GameObject blockPrefab_Grass; // ���u���b�N

	// �萔���`
	public const int MAP_WIDTH = 11; // �}�b�v�̉���
	public const int MAP_HEIGHT = 11; // �}�b�v�̏c��
    private int generateHardStone = 20; // �΃u���b�N�����������m��
    private int generateGrass = 20; // ���u���b�N�����������m��


    void Start()
    {
        Vector3 defaultPos = DefaultPos();
        Generateblock( defaultPos );
    }

    private static Vector3 DefaultPos()
    {
        // �u���b�N�����ʒu�̊�_�ƂȂ���W��ݒ�
        Vector3 defaultPos = new Vector3(0f, 0f, 0f);
        defaultPos.x = -( MAP_WIDTH / 2 ); // x���W�̊�_
        defaultPos.z = -( MAP_HEIGHT / 2 ); // z���W�̊�_
        return defaultPos;
    }

    private void Generateblock(Vector3 defaultPos)
    {
        // �u���b�N��������
        for ( int i = 0; i < MAP_WIDTH; i++)// �}�b�v�̉������J��Ԃ�����
        {
            for ( int j = 0; j < MAP_HEIGHT; j++)// �}�b�v�̏c�����J��Ԃ�����
            {
                Vector3 pos = defaultPos; 
                pos.x += i; // 1�ڂ�for���̌J��Ԃ��񐔕�x���W�����炷
                pos.z += j; // 2�ڂ�for���̌J��Ԃ��񐔕�z���W�����炷

                // �u���b�N�̎�ނ����肷��m���̗���
                int rand = Random.Range(0, 100); 

                // �I�u�W�F�N�g�𐶐�
                GameObject obj; 

                if ( rand <generateHardStone )
                {
                    
                    obj = Instantiate(blockPrefab_HardStone, blockParent); // blockParent�̎q�ɐ΃u���b�N�𐶐�����
                }

                else if ( rand > generateHardStone && rand < generateHardStone + generateGrass )
                {
                    obj = Instantiate(blockPrefab_Grass, blockParent); // blockParent�̎q�ɑ��u���b�N�𐶐�����
                }

                else
                {
                    obj = Instantiate(blockPrefab_MossStone, blockParent); // blockParent�̎q�ɑې΃u���b�N�𐶐�����
                }

                obj.transform.position = pos; // �I�u�W�F�N�g�̍��W��K�p
            }
        }
    }
}
