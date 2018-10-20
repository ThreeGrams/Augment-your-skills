using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {

	// Update is called once per frame
	void Update() {

	}

	public void loadMenuScene() {
		SceneManager.LoadSceneAsync("Menu");
	}

	public void loadRequestListScene() {
		SceneManager.LoadSceneAsync("RequestList");
	}

	public void loadRequestDetailScene() {
		SceneManager.LoadSceneAsync("RequestDetail");
	}

	public void loadNeedRequestScene() {
		SceneManager.LoadSceneAsync("NeedRequest");
	}

	public void loadCameForPhotoSceneScene() {
		SceneManager.LoadSceneAsync("CameraForPhotoScene");
	}

	public void loadARScene() {
		SceneManager.LoadSceneAsync("ARScene");
	}
}

