using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	void Start()
	{

	}

	void Update()
	{
		// �^�b�v���o����
		if (Input.GetMouseButtonDown(0))
		{
			GetMapBlockByTapPos();
		}
	}

	private void GetMapBlockByTapPos()
	{
		GameObject targetObject = null; // �^�b�v�Ώۂ̃I�u�W�F�N�g

		// �^�b�v���������ɃJ��������Ray���΂�
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{// Ray�ɓ�����ʒu�ɑ��݂���I�u�W�F�N�g���擾
			targetObject = hit.collider.gameObject;
		}

		// �ΏۃI�u�W�F�N�g(�}�b�v�u���b�N)�����݂���ꍇ�̏���
		if (targetObject != null)
		{
			// �u���b�N�I��������
			SelectBlock(targetObject.GetComponent<FloorBlock>());
		}
	}

	private void SelectBlock(FloorBlock targetBlock)
	{
		Debug.Log("�u���b�N���^�b�v����܂����B\n�u���b�N�̍��W�F" + targetBlock.transform.position);
	}
}