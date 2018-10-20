using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailRequestUI : MonoBehaviour
{
	// Start is called before the first frame update

	[SerializeField]
	private TMP_Text _title;

	[SerializeField]
	private InputField _description;

	[SerializeField]
	private Image _image;

	void Start()
    {
		loadData();
    }

	private void loadData() {
		if (ImageHolder.image != null) {
			Texture2D tex2D = new Texture2D(ImageHolder.width, ImageHolder.height);
			tex2D.SetPixels32(ImageHolder.image);
			_image.sprite = Sprite.Create(tex2D, _image.rectTransform.rect, _image.rectTransform.pivot);
		}
		if (_title != null) {
			_title.text = RequestDataHolder.title;
		}
		if (_description != null) {
			_description.text = RequestDataHolder.description;
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
