using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
// ...
namespace zeroonetwo
{
	public class LoadDllsOnStart : MonoBehaviour
	{
		const string k4a = "k4a.dll";

		[DllImport("DllHelloWorld")]
		public static extern int DisplayHelloFromDLL();

		[DllImport(k4a)]
		public static extern uint k4a_device_get_installed_count();
		
		
		[DllImport(k4a)]
		public extern static k4a_result_t k4a_device_open(UInt32 index, out IntPtr device_handle);
		
		[DllImport(k4a)]
		public extern static void k4a_device_close(IntPtr device_handle);

		
        [DllImport(k4a)]
        public extern static k4a_buffer_result_t k4a_device_get_serialnum(IntPtr device_handle, IntPtr serial_number, ref UInt64 serial_number_size);

        internal IntPtr device_handle = IntPtr.Zero; // 1265321712
        internal IntPtr device_serial_num = IntPtr.Zero; // seems to be generated? keeps changing
        		
        [DllImport(k4a)]
        public extern static k4a_result_t k4a_device_start_cameras(IntPtr device_handle, ref DeviceConfiguration config);

        [DllImport(k4a)]
        public extern static void k4a_device_stop_cameras(IntPtr device_handle);

        [DllImport(k4a)]
        public extern static k4a_wait_result_t k4a_device_get_capture(IntPtr device_handle, out IntPtr capture_handle, Int32 timeout_in_ms);

		internal IntPtr capture_handle = IntPtr.Zero; // 
		Int32 timeout_in_ms = 10000;

		DeviceConfiguration defaultConfigDisableAll;
		
        [DllImport(k4a)]
        public extern static void k4a_capture_release(IntPtr capture_handle);

		void Start()
		{
			Debug.Log("Time is " + DateTime.Now.ToString("h:mm:ss tt") + ". Dll test: " + DisplayHelloFromDLL());
			// Debug.Log("loc:\n" + Assembly.GetExecutingAssembly().Location);
			Debug.Log(k4a_device_get_installed_count() + " device found");
			
			k4a_result_t result = k4a_device_open(0, out device_handle);
			Debug.Log("device_handle: " + device_handle);

			UInt64 size = 0; // these two lines attempt to get the serial num. If too small, size = needed buffer size
			k4a_buffer_result_t buffer_Result_T = k4a_device_get_serialnum(device_handle, device_serial_num, ref size);

			if(buffer_Result_T.Equals(k4a_buffer_result_t.K4A_BUFFER_RESULT_TOO_SMALL))
			{ // size was reset from 0 to needed size by k4a_device_get_serialnum, alloc an IntPtr buffer
				device_serial_num = Marshal.AllocHGlobal((int)size);
				buffer_Result_T = k4a_device_get_serialnum(device_handle, device_serial_num, ref size);
			}
			// Debug.Log(size);

			if(buffer_Result_T.Equals(k4a_buffer_result_t.K4A_BUFFER_RESULT_SUCCEEDED))
			{
				Debug.Log("WORKED!! : " + buffer_Result_T);
			} else {
				Debug.Log("Failed: " + buffer_Result_T);
			}

			Debug.Log("device_serial_num: " + device_serial_num);



			var config = getDefaultConfig();
			// config.camera_fps       = K4A_FRAMES_PER_SECOND_15;
			// config.color_format     = K4A_IMAGE_FORMAT_COLOR_BGRA32;
			// config.color_resolution = K4A_COLOR_RESOLUTION_3072P;

			k4a_device_start_cameras(device_handle, ref config);

			// Camera capture and application specific code would go here
			k4a_wait_result_t result_getCapture = k4a_device_get_capture(device_handle, out capture_handle, timeout_in_ms);
			switch (result_getCapture)
			{
			// case K4A_WAIT_RESULT_SUCCEEDED:
			case k4a_wait_result_t.K4A_WAIT_RESULT_SUCCEEDED:
				break;
			case k4a_wait_result_t.K4A_WAIT_RESULT_TIMEOUT:
				Debug.Log("Timed out waiting for a capture\n");
				break;
			case k4a_wait_result_t.K4A_WAIT_RESULT_FAILED:
				Debug.Log("Failed to read a capture\n");
				break;
			}
			// Shut down the camera when finished with application logic
			k4a_capture_release(capture_handle);
			k4a_device_stop_cameras(device_handle);
			k4a_device_close(device_handle);

			Debug.Log("Exit");
		}

		DeviceConfiguration getDefaultConfig()
		{
			if(defaultConfigDisableAll.Equals(null)) // check if null, singleton
			{
				defaultConfigDisableAll = new DeviceConfiguration(); //k4a_device_configuration_t
			}
			defaultConfigDisableAll.ColorFormat = ImageFormat.ColorMJPG;
			defaultConfigDisableAll.ColorResolution = ColorResolution.OFF;
			defaultConfigDisableAll.DepthMode = DepthMode.OFF;
			defaultConfigDisableAll.CameraFps = Fps._30;
			defaultConfigDisableAll.SynchronizedImagesOnly = false;
			defaultConfigDisableAll.SubordinateDelayOffMasterUsec = 0;
			defaultConfigDisableAll.WiredSyncMode = WiredSyncMode.Standalone;
			defaultConfigDisableAll.SubordinateDelayOffMasterUsec = 0;
			defaultConfigDisableAll.DisableStreamingIndicator = false;
			// else {
				// Debug.Log("This will be an infinite loop if something breaks");
				// defaultConfigDisableAll = new DeviceConfiguration(); //k4a_device_configuration_t
				// getDefaultConfig();
			// }
			return defaultConfigDisableAll;
		}

	}

}

