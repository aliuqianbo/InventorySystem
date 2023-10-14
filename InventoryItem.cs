using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryItem : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler, IPointerExitHandler
{
	[Header("UI")]
	public Image image;
	public Text countText;
	public GameObject selecFrame;


	[HideInInspector]public Item item;
	[HideInInspector]public int count = 1;
	[HideInInspector]public Transform parentAfterDrag;

	private GameObject itemObjectRemained;
	private PlayerController playerController;
	private SoundManager soundManager;

	private void Start()
	{
		var _target = GameObject.FindGameObjectsWithTag("Player");
		playerController = _target[0].GetComponent<PlayerController>();

		var soundManagerArray = FindObjectsOfType<SoundManager>();
		soundManager = soundManagerArray[0];

	}

	public void InitializeItem(Item newItem) {
		item = newItem;
		image.sprite = newItem.image;
		RefreshCount();
	}

	public void RefreshCount() 
	{
		countText.text = count.ToString();
		bool textActive = count > 1;
		countText.gameObject.SetActive(textActive);

	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.pointerCurrentRaycast.gameObject != null)
		{
			selecFrame.SetActive(true);
			Debug.Log("Mouse Over: " + eventData.pointerCurrentRaycast.gameObject.name);
			playerController.pointerOn_Item = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventoryItem>();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		selecFrame.SetActive(false);
		playerController.pointerOn_Item = null;
	}


	public void OnBeginDrag(PointerEventData eventData)
	{
		//check if shit is pressed
		bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;
		Debug.Log(isShiftPressed);
		if (isShiftPressed)
		{
			if (count > 1)
			{
				//spawn new item object at orignal parent, with -1 count.
				itemObjectRemained = Instantiate(transform.gameObject, transform.parent);
				InventoryItem remained_inventoryItem = itemObjectRemained.GetComponent<InventoryItem>();
				remained_inventoryItem.count--;
				remained_inventoryItem.RefreshCount();
			}

			count = 1;
			RefreshCount();

		}

		Debug.Log("Begin drag");
		//play audio
		soundManager.PlaySound(soundManager.pickUpItem, 0.25f);

		//set origin parent
		parentAfterDrag = transform.parent;

		//move icon to the top 
		transform.SetParent(transform.root);
		transform.SetAsLastSibling();
		image.raycastTarget = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		///Debug.Log("Dragging");
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log("End drag");
		if (parentAfterDrag.childCount!= 0)
		{
			InventoryItem remained_inventoryItem = itemObjectRemained.GetComponent<InventoryItem>();
			count = remained_inventoryItem.count + 1;
			RefreshCount();

			Destroy(itemObjectRemained);
			image.raycastTarget = true;
		}

		transform.SetParent(parentAfterDrag);
		image.raycastTarget = true;

		//play audio
		soundManager.PlaySound(soundManager.dropItem, 0.1f);

	}
}
