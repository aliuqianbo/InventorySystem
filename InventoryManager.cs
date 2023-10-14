using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	public InventorySlot[] inventorySlots;
	public GameObject inventoryItemPrefab;
	int selectedSlot = -1;

	private void Start()
	{
		ChangeSelectedSlot(0);
	}

	private void Update()
	{
		//change item_select
		if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			if ((selectedSlot - 1) >= 0)
			{
				int temp = selectedSlot - 1;
				ChangeSelectedSlot(temp);
			}
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			if ((selectedSlot + 1) <= 3)
			{
				int temp = selectedSlot + 1;
				ChangeSelectedSlot(temp);
			}
		}
	}

	void ChangeSelectedSlot(int newValue) 
	{
		if (selectedSlot >= 0)
		{
			inventorySlots[selectedSlot].Deselect();
		}
		inventorySlots[newValue].Select();
		selectedSlot = newValue;
	}

	public bool AddItem(Item item) 
	{
		for (int i = 0; i < inventorySlots.Length; i++)
		{
			InventorySlot slot = inventorySlots[i];
			InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
			if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < item.stackCount)
			{
				itemInSlot.count++;
				itemInSlot.RefreshCount();
				return true;
			}
		}

		for (int i = 0; i < inventorySlots.Length; i++) 
		{
			InventorySlot slot = inventorySlots[i];
			InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
			if (itemInSlot == null) 
			{
				SpawnNewItem(item, slot);
				return true;
			}
		}

		return false;
	}

	void SpawnNewItem(Item item, InventorySlot slot)
	{
		GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
		InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
		inventoryItem.InitializeItem(item);
	}

	public InventoryItem GetSelectedItem()
	{
		InventorySlot slot = inventorySlots[selectedSlot];
		InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
		if (itemInSlot != null)
		{
			return itemInSlot;
		}
		return null;
	}

}
