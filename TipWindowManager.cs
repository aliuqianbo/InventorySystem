using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TipWindowManager : MonoBehaviour
{
    public TextMeshProUGUI tipText_name;
    public TextMeshProUGUI tipText_type;
    public TextMeshProUGUI tipText_description;
    public TextMeshProUGUI tipText_formula;
    public Image item_icon;
    public GameObject tipWindow;

    public static Action<string, string, string, string, Vector2, Sprite,ItemRecordProgressManager> OnMouseHover;
    public static Action OnMouseLoseFocus;


    private bool isShown;
    private ItemRecordProgressManager cachedPManager;
    private string cachedItemName;

	private void OnEnable()
	{
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;

	}

	private void OnDisable()
	{
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }


	// Start is called before the first frame update
	void Start()
    {
        HideTip();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
        if (cachedPManager != null)
        {
            float _progress = (float)Math.Floor(cachedPManager.recordProgress);
            tipText_name.text = cachedItemName + "(" + _progress.ToString() + "%)";
        }
    }

    private void ShowTip(string name, string type, string description, string formula, Vector2 mousePos, Sprite icon, ItemRecordProgressManager progressManager)
    {
        if (type == "Record Item")  //string type, not actual type
        {
            cachedPManager = progressManager;
            cachedItemName = name;
            float _progress = (float)Math.Floor(progressManager.recordProgress);
            tipText_name.text = name +"("+ _progress.ToString()+"%)";
        }
        else
        {
            tipText_name.text = name;
        }

        tipText_type.text = type;

        tipText_description.text = description;
        tipText_formula.text = formula;
        item_icon.sprite = icon;
        tipWindow.SetActive(true);
        tipWindow.transform.position = mousePos;


    }

    private void HideTip()
    {
        tipText_name.text = default;
        tipText_type.text = default;
        tipText_description.text = default;
        tipText_formula.text = default;
        tipWindow.SetActive(false);

        cachedPManager = null;
        cachedItemName = default;
    }

}
