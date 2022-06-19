using UnityEngine;

public class CameraZoom : MonoBehaviour
{
	// ���C���J����
	private Camera mainCamera;

	// �萔��`
	const float ZOOM_SPEED = 0.1f; // �Y�[�����x
	const float ZOOM_MIN = 40.0f; // �J�����̍ŏ��̎���
	const float ZOOM_MAX = 60.0f; // �J�����̍ő�̎���

	void Start()
	{
		mainCamera = GetComponent<Camera>(); // �J�����̎Q�Ƃ��擾
	}

	void Update()
	{
		// �}���`�^�b�`(�Q�_�����^�b�`)�łȂ��Ȃ�I��
		if (Input.touchCount != 2)
			return;

		// �Q�_�̃^�b�`�����擾����
		var touchData_0 = Input.GetTouch(0);
		var touchData_1 = Input.GetTouch(1);

		// �P�t���[���O�̂Q�_�Ԃ̋��������߂�
		float oldTouchDistance = Vector2.Distance( // Vector2.Distance�łQ�_�Ԃ̋������擾
			touchData_0.position - touchData_0.deltaPosition, // (deltaPosition�ɂ͂P�t���[���O�̃^�b�`�ʒu�������Ă���)
			touchData_1.position - touchData_1.deltaPosition
			);
		// ���݂̂Q�_�Ԃ̋��������߂�
		float currentTouchDistance = Vector2.Distance(touchData_0.position, touchData_1.position);

		// �Q�_�Ԃ̋����̕ω��ʂɉ����ăY�[������(�J�����̎���̍L����ύX����)
		float distanceMoved = oldTouchDistance - currentTouchDistance;
		mainCamera.fieldOfView += distanceMoved * ZOOM_SPEED;

		// �J�����̎�����w��͈̔͂Ɏ��߂�
		mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, ZOOM_MIN, ZOOM_MAX);
	}
}