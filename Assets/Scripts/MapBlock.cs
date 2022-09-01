using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlock : MonoBehaviour
{
	// �����\���I�u�W�F�N�g
	private GameObject selectionBlockObj;

	// �����\���}�e���A��
	[Header("�����\���}�e���A���F�I����")]
	public Material selMat_Select; // �I����
	[Header("�����\���}�e���A���F���B�\")]
	public Material selMat_Reachable; // �L�����N�^�[�����B�\
	[Header("�����\���}�e���A���F�U���\")]
	public Material selMat_Attackable; // �L�����N�^�[���U���\
									   // �u���b�N�̋����\�����[�h���`����(�񋓌^)
	public enum Highlight
	{
		Off, // �I�t
		Select, // �I����
		Reachable, // �L�����N�^�[�����B�\
		Attackable, // �L�����N�^�[���U���\
	}

	// �u���b�N�f�[�^
	[HideInInspector] // �C���X�y�N�^��Ŕ�\���ɂ��鑮��
	public int xPos; // X�����̈ʒu
	[HideInInspector]
	public int zPos; // Z�����̈ʒu
	[Header("�ʍs�\�t���O(true�Ȃ�ʍs�\�ł���)")]
	public bool passable; // �ʍs�\�t���O

	void Start()
	{
		// �����\���I�u�W�F�N�g���擾
		selectionBlockObj = transform.GetChild(0).gameObject; // �q�̂P�Ԗڂɂ���I�u�W�F�N�g

		// ������Ԃł͋����\�������Ȃ�
		SetSelectionMode(Highlight.Off);
	}

	/// <summary>
	/// �I����ԕ\���I�u�W�F�N�g�̕\���E��\����ݒ肷��
	/// </summary>
	/// <param name="mode">�����\�����[�h</param>
	public void SetSelectionMode(Highlight mode)
	{
		switch (mode)
		{
			// �����\���Ȃ�
			case Highlight.Off:
				selectionBlockObj.SetActive(false);
				break;
			// �I����
			case Highlight.Select:
				selectionBlockObj.GetComponent<Renderer>().material = selMat_Select;
				selectionBlockObj.SetActive(true);
				break;
			// �L�����N�^�[�����B�\
			case Highlight.Reachable:
				selectionBlockObj.GetComponent<Renderer>().material = selMat_Reachable;
				selectionBlockObj.SetActive(true);
				break;
			// �L�����N�^�[���U���\
			case Highlight.Attackable:
				selectionBlockObj.GetComponent<Renderer>().material = selMat_Attackable;
				selectionBlockObj.SetActive(true);
				break;
		}
	}

}