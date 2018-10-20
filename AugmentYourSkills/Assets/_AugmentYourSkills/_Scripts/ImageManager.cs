using AYS.Camera;
using UnityEngine;

namespace AYS {
	public class ImageManager : MonoBehaviour {

		public Texture currentImage = null;

		public void saveImage() {
			currentImage = DeviceCamera.instance.getCurrentFrameImage();
			ImageHolder.image = currentImage;
		}

	}
}

