using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class LoadDllsOnStart : MonoBehaviour
{
	[DllImport("DllHelloWorld")]
	public static extern int DisplayHelloFromDLL();
	
	void Start()
    {
		Debug.Log("Time is " + DateTime.Now.ToString("h:mm:ss tt"));
		Debug.Log(DisplayHelloFromDLL());
		
		
		
		Debug.Log("Exit");
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
