using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	void Start()
	{

	}

	void Update()
	{
		// タップ検出処理
		if (Input.GetMouseButtonDown(0))
		{
			GetMapBlockByTapPos();
		}
	}

	private void GetMapBlockByTapPos()
	{
		GameObject targetObject = null; // タップ対象のオブジェクト

		// タップした方向にカメラからRayを飛ばす
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit))
		{// Rayに当たる位置に存在するオブジェクトを取得
			targetObject = hit.collider.gameObject;
		}

		// 対象オブジェクト(マップブロック)が存在する場合の処理
		if (targetObject != null)
		{
			// ブロック選択時処理
			SelectBlock(targetObject.GetComponent<FloorBlock>());
		}
	}

	private void SelectBlock(FloorBlock targetBlock)
	{
		Debug.Log("ブロックがタップされました。\nブロックの座標：" + targetBlock.transform.position);
	}
}