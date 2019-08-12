using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using HoloLab.AzureKinect;
using HoloLab.AzureKinect.NativeMethod;
//using UnityEngine.Assertions;
using System;

public class DllScript : MonoBehaviour
{
	[DllImport("DllHelloWorld")]
	public static extern int DisplayHelloFromDLL();
	[DllImport("k4a")]
	public static extern uint k4a_device_get_installed_count();

	public GameObject textureHolder;
	Texture2D texture;
	public RawImage rawimage;

	HoloLab.AzureKinect.Image image;
	HoloLab.AzureKinect.Image depthImage;
	Capture capture = null;

	// Start is called before the first frame update
	void Start()
    {
		//Debug.Log("This is C# program");
		//Debug.Log(DisplayHelloFromDLL());
		//Debug.Log(k4a_device_get_installed_count());
		Debug.Log(K4A.k4a_device_get_installed_count() + " cameras");
		
		StartTheCamera();
	}
	
	public void GetNumberOfDevices()
	{
		//Assert.AreEqual(1U, KinectSensor.SensorCount);
		Debug.Log("SensorCount:" + KinectSensor.SensorCount);
	}

	public void OpenTheSensor()
	{
		try
		{
			using (var targert = new KinectSensor())
			{
				targert.Open(0);
				Debug.Log("Opened Sensor successfully");
			}
		}
		catch (System.Exception e)
		{
			Debug.Log("Failed to open sensor");
			Debug.Log(e);
		}
	}

	public void ConfirmDispose()
	{
		KinectSensor target = null;
		using (target = new KinectSensor())
		{
			target.Open(0);
			Debug.Log("target.IsOpened:" + target.IsOpened);
			//Assert.IsTrue(target.IsOpened);
		}

		//Assert.IsNotNull(target);
		//Assert.IsFalse(target.IsOpened);
	}

	public void getSerialNumber()
	{
		using (var target = new KinectSensor())
		{
			target.Open();
			Debug.Log("Serial number:" + target.GetSerialNumber());
			//Assert.AreEqual("000327292312", target.GetSerialNumber());
		}
	}

	public void StartTheCamera()
	{
		using (var target = new KinectSensor())
		{
			target.Open();

			Debug.Log("serial number:" + target.GetSerialNumber());
			// body tracker wont run if depth mode  K4A_DEPTH_MODE_OFF or K4A_DEPTH_MODE_PASSIVE_IR.
			// needs either K4A_DEPTH_MODE_NFOV_UNBINNED or K4A_DEPTH_MODE_WFOV_2X2BINNED
			var config = new DeviceConfiguration();
			config.ColorFormat = ImageFormat.ColorBGRA32;
			config.ColorResolution = ColorResolution._720P;
			//config.ColorResolution = ColorResolution.OFF; // per MS doc
			//config.CameraFps = Fps._30;
			config.DepthMode = DepthMode.OFF;
			config.SynchronizedImagesOnly = false;
			//config.SubordinateDelayOffMasterUsec = 0;
			//config.WiredSyncMode = WiredSyncMode.Standalone;
			//config.SubordinateDelayOffMasterUsec = 0;
			//config.DisableStreamingIndicator = false;
			Debug.Log(config.ToString());

			target.StartCamera(config);

			IntPtr image_handle = IntPtr.Zero;
			capture = new Capture(target, 5000);
			image = new HoloLab.AzureKinect.Image(image_handle);
			//image = new HoloLab.AzureKinect.Image(config.ColorFormat, 1920, 1080, 1920);
			//depthImage = new HoloLab.AzureKinect.Image(ImageFormat.Depth16, 1920, 1080, 1920);
			//Image depthImage = new Image(ImageFormat.Depth16, 320, 288, 320); //640 576 640

			texture = new Texture2D(1920, 1080, TextureFormat.BGRA32, false);

			rawimage.texture = texture;

			//target.StopCamera();
		}

	}

}
