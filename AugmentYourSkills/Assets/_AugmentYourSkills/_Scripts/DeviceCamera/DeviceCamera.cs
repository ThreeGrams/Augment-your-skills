using UnityEngine;
using UnityEngine.UI;

namespace AYS.Camera {
	public class DeviceCamera : MonoBehaviour {

		private bool _cameraIsAvailable;
		private WebCamTexture _backCamera;
		private Texture _defaultBackground;

		public RawImage background;
		public AspectRatioFitter fitter;

		public static DeviceCamera instance = null;

		private void OnDestroy() {
			instance = null;
		}

		private void Awake() {
			instance = this;
		}

		// Start is called before the first frame update
		void Start() {
			_defaultBackground = background.texture;
			WebCamDevice[] devices = WebCamTexture.devices;

			if (devices.Length == 0) {
				Debug.LogWarning("No camera was detected!");
				_cameraIsAvailable = false;
				return;
			}

			for (int i = 0; i < devices.Length; i++) {
				if (!devices[i].isFrontFacing) {
					_backCamera = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
				}
			}

			if (_backCamera == null) {
				Debug.Log("No back camera was detected! Trying to use front camera...");
				_backCamera = new WebCamTexture(devices[0].name, Screen.width, Screen.height);
			}

			_backCamera.Play();
			background.texture = _backCamera;

			_cameraIsAvailable = true;
		}

		// Update is called once per frame
		void Update() {
			if (!_cameraIsAvailable) {
				return;
			}

			float ratio = _backCamera.width / _backCamera.height;
			fitter.aspectRatio = ratio;

			float scaleY = _backCamera.videoVerticallyMirrored ? -1f : 1f;
			background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

			float orientation = -_backCamera.videoRotationAngle;
			background.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);

		}
	}
}

