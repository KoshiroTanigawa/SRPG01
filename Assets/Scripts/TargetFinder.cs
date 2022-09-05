using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetFinder
{
	public class ActionPlan
	{
		public Character charaData; // �s������G�L�����N�^�[
		public MapBlock toMoveBlock; // �ړ���̈ʒu
		public Character toAttackChara; // �U������̃L�����N�^�[
	}

	/// <summary>
	/// �U���\�ȍs���v������S�Č������A���̓��̂P�������_���ɕԂ�����
	/// </summary>
	/// <param name="mapManager">�V�[������MapManager�̎Q��</param>
	/// <param name="charactersManager">�V�[������CharactersManager�̎Q��</param>
	/// <param name="enemyCharas">�G�L�����N�^�[�̃��X�g</param>
	/// <returns></returns>
	public static ActionPlan GetRandomActionPlan(MapManager mapManager, CharactersManager charactersManager, List<Character> enemyCharas)
	{
		// �S�s���v����(�U���\�ȑ��肪������x�ɒǉ������)
		var actionPlans = new List<ActionPlan>();
		// �ړ��͈̓��X�g
		var reachableBlocks = new List<MapBlock>();
		// �U���͈̓��X�g
		var attackableBlocks = new List<MapBlock>();

		// �S�s���v������������
		foreach (Character enemyData in enemyCharas)
		{
			// �ړ��\�ȏꏊ���X�g���擾����
			reachableBlocks =
				mapManager.SearchReachableBlocks(enemyData.xPos, enemyData.zPos);
			// ���ꂼ��̈ړ��\�ȏꏊ���Ƃ̏���
			foreach (MapBlock block in reachableBlocks)
			{
				// �U���\�ȏꏊ���X�g���擾����
				attackableBlocks = mapManager.SearchAttackableBlocks(block.xPos, block.zPos);
				// ���ꂼ��̍U���\�ȏꏊ���Ƃ̏���
				foreach (MapBlock attackBlock in attackableBlocks)
				{
					// �U���ł��鑊��L�����N�^�[(�v���C���[���̃L�����N�^�[)��T��
					Character targetChara =
						charactersManager.GetCharacterDataPos(attackBlock.xPos, attackBlock.zPos);
					if (targetChara != null &&
						!targetChara._isEnemy)
					{// ����L�����N�^�[�����݂���
					 // �s���v������V�K�쐬����
						var newPlan = new ActionPlan();
						newPlan.charaData = enemyData;
						newPlan.toMoveBlock = block;
						newPlan.toAttackChara = targetChara;

						// �S�s���v�������X�g�ɒǉ�
						actionPlans.Add(newPlan);
					}
				}
			}
		}

		// �����I����A�s���v�������P�ł�����Ȃ炻�̓��̂P�������_���ɕԂ�
		if (actionPlans.Count > 0)
			return actionPlans[Random.Range(0, actionPlans.Count)];
		else // �s���v�����������Ȃ�null��Ԃ�
			return null;
	}
}