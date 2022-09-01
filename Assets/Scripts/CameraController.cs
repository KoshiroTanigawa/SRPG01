using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	// �J�����ړ��p�ϐ�
	private bool isCameraRotate; // �J������]���t���O
	private bool isMirror; // ��]�������]�t���O

	// �萔��`
	const float SPEED = 30.0f; // ��]���x

	void Update()
	{
		// �J������]����
		if (isCameraRotate)
		{
			// ��]���x���v�Z����
			float speed = SPEED * Time.deltaTime;
			// ��]�������]�t���O�������Ă���Ȃ瑬�x���]
			if (isMirror)
				speed *= -1.0f;

			// ��_�̈ʒu�𒆐S�ɃJ��������]�ړ�������
			transform.RotateAround(
				Vector3.zero, // ��_�̈ʒu(0, 0, 0)
				Vector3.up, // ��]��
				speed // ��]���x
			);
		}
	}

	/// <summary>
	/// �J�����ړ��{�^���������n�߂�ꂽ���ɌĂяo����鏈��
	/// </summary>
	/// <param name="rightMode">�E�����t���O(�E�ړ��{�^������Ă΂ꂽ��true�ɂȂ��Ă���)</param>
	public void CameraRotate_Start(bool rightMode)
	{
		// �J������]���t���O��ON
		isCameraRotate = true;
		// ��]�������]�t���O��K�p����
		isMirror = rightMode;
	}
	/// <summary>
	/// �J�����ړ��{�^����������Ȃ��Ȃ������ɌĂяo����鏈��
	/// </summary>
	public void CameraRotate_End()
	{
		// �J������]���t���O��OFF
		isCameraRotate = false;
	}
}