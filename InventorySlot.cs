using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour,IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;

	private void Awake()
	{
        Deselect();
        //temp: stop using the select slot function
        selectedColor = notSelectedColor;
	}

	public void Select()
	{
        image.color = selectedColor;
	}

    public void Deselect()
    {
        image.color = notSelectedColor;
    }

	public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        InventoryItem draggableItem = dropped.GetComponent<InventoryItem>();


        if (transform.childCount == 0)
        {

            draggableItem.parentAfterDrag = transform;
        }
        else
        {
            Transform _target = transform.GetChild(0);
            InventoryItem _itemInSlot = _target.GetComponent<InventoryItem>();

            //same item add count
            if (_itemInSlot.item.name == draggableItem.item.name && _itemInSlot.count + draggableItem.count < _itemInSlot.item.stackCount)
            {
                _itemInSlot.count += draggableItem.count;
                _itemInSlot.RefreshCount();
                Destroy(draggableItem.gameObject);
            }
            //different item switch position
            else if(_itemInSlot.item.name != draggableItem.item.name)
            {
                if (draggableItem.parentAfterDrag.childCount == 0)
                {
                    _itemInSlot.transform.parent = draggableItem.parentAfterDrag;
                    draggableItem.parentAfterDrag = transform;
                }

            }

            
        }

    }

}
