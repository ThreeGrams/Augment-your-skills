using AYS.Camera;
using UnityEngine;

namespace AYS {
	public class ImageManager : MonoBehaviour {

		public Color32[] currentImage = null;

		public void saveImage() {
			DeviceCamera instance = DeviceCamera.instance;
			currentImage = instance.getCurrentFrameImagePixels();
			ImageHolder.image = currentImage;
			ImageHolder.width = instance.getCurrentImageWidth();
			ImageHolder.height = instance.getCurrentImageHeight();
		}

	}
}

