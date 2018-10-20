using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedRequestUI : MonoBehaviour
{
	// Start is called before the first frame update



	[SerializeField]
	private TMP_InputField _title;

	[SerializeField]
	private InputField _description;

	[SerializeField]
	private Image _buttonImage;
	void Start()
    {
		Color32[] image = ImageHolder.image;
		if (image != null) {
			Texture2D texture2D = new Texture2D(ImageHolder.width, ImageHolder.height);
			texture2D.SetPixels32(image);
			texture2D.Apply();
			_buttonImage.sprite = Sprite.Create(texture2D, _buttonImage.rectTransform.rect, _buttonImage.rectTransform.pivot);
		}
		_title.text = RequestDataHolder.title;
		_description.text = RequestDataHolder.description;
	}

	public void saveTitleToCache() {
		RequestDataHolder.title = _title.text;
	}
	public void saveDescriptionToCache() {
		RequestDataHolder.description = _description.text;
	}
}
