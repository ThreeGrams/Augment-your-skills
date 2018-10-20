using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequestListUI : MonoBehaviour
{

	[SerializeField]
	private Button lastButton = null;
	
	// Start is called before the first frame update
	void Start()
    {
		if (lastButton != null) {
			lastButton.GetComponentInChildren<Text>().text = RequestDataHolder.title;
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
