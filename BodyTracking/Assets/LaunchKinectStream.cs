using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class LaunchKinectStream : MonoBehaviour
{
	public Text text;
	
	[DllImport ("k4aAsset.dll")]
	private static extern uint k4a_device_get_installed_count();

	// Start is called before the first frame update
	void Start()
    {
		text.text = "something dumb";   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void openKinectStream()
	{
		Debug.Log("Opening kinect stream");
		Debug.Log(k4a_device_get_installed_count());
	}


}
