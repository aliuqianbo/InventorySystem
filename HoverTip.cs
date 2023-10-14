using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private float timeToWait = 0.5f;
	private string item_name;
	private string item_type;
	private string item_description;
	private string item_formula;
	private Sprite item_icon;
	private InventoryItem inventoryItem;
	private ItemRecordProgressManager progressManager;

	public void OnPointerEnter(PointerEventData eventData)
	{
		StopAllCoroutines();
		StartCoroutine(StartTimer());
		inventoryItem = transform.GetComponent<InventoryItem>();
		progressManager = transform.GetComponent<ItemRecordProgressManager>();
		item_name = inventoryItem.item.txt_Name;
		item_description = inventoryItem.item.txt_description;
		item_formula = inventoryItem.item.txt_formula;
		item_type = inventoryItem.item.txt_Type;
		item_icon = inventoryItem.item.image;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		StopAllCoroutines();
		TipWindowManager.OnMouseLoseFocus();
	}
	private void ShowMessage()
	{
		TipWindowManager.OnMouseHover(item_name, item_type, item_description, item_formula, Input.mousePosition, item_icon, progressManager);
	}

	private IEnumerator StartTimer()
	{
		yield return new WaitForSeconds(timeToWait);
		ShowMessage();
	}
}
