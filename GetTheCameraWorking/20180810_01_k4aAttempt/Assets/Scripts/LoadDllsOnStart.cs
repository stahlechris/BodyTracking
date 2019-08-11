using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class LoadDllsOnStart : MonoBehaviour
{
	[DllImport("DllHelloWorld")]
	public static extern int DisplayHelloFromDLL();
	[DllImport("k4a")]
	public static extern uint k4a_device_get_installed_count();
	
	void Start()
    {
		Debug.Log("Time is " + DateTime.Now.ToString("h:mm:ss tt"));
		Debug.Log(DisplayHelloFromDLL());
		Debug.Log(k4a_device_get_installed_count());
		
		
		Debug.Log("Exit");
	}

}
