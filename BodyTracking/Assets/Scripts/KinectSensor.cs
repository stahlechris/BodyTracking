// Copyright(c) 2019 HoloLab Inc.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

using HoloLab.AzureKinect.NativeMethod;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace HoloLab.AzureKinect
{
    public class KinectSensor : IDisposable
    {
        internal IntPtr device_handle = IntPtr.Zero;
        string serialNumber = string.Empty;
		
		public static uint SensorCount { get { return K4A.k4a_device_get_installed_count(); } }
		public bool IsOpened { get { return device_handle != IntPtr.Zero; } }

		/// <summary>
		/// 最初のKinectセンサーを開く
		/// </summary>
		public void Open()
        {
            Open(0);
        }

        /// <summary>
		/// Open the Kinect sensor at the specified index
        /// 指定されたインデックスのKinectセンサーを開く
        /// </summary>
        /// <param name="index"></param>
        public void Open(UInt32 index)
        {
            Close();
			Debug.Log("device_handle:" + device_handle);
            var ret = K4A.k4a_device_open(index, out device_handle);
			Debug.Log("open ret:" + ret.ToString());
            if (ret != k4a_result_t.K4A_RESULT_SUCCEEDED)
            {
                throw new AzureKinectException(ret);
            }
        }

        /// <summary>
		/// Close the Kinect Sensor
        /// Kinectセンサーを閉じる
        /// </summary>
        public void Close()
        {
            if (device_handle != IntPtr.Zero)
            {
                K4A.k4a_device_close(device_handle);
                device_handle = IntPtr.Zero;
                serialNumber = string.Empty;
            }
        }

        /// <summary>
        /// シリアルナンバーを取得する
        /// </summary>
        /// <returns></returns>
        public string GetSerialNumber()
        {
            // 取得済みであれば、そのまま返す
            if (!string.IsNullOrEmpty(serialNumber))
            {
                return serialNumber;
            }

			// If the second argument buffer is NULL (0), the serial number buffer size is returned
			// 第二引数のバッファをNULL(0)にすると、シリアルナンバーのバッファサイズが返る
			UInt64 size = 0;
            var ret = K4A.k4a_device_get_serialnum(device_handle, IntPtr.Zero, ref size);
            if (ret != k4a_buffer_result_t.K4A_BUFFER_RESULT_TOO_SMALL)
            {
                throw new AzureKinectException(ret.ToString());
            }

			// Actually get the serial number
			// 実際にシリアルナンバーを取得する
			var buffer = Marshal.AllocHGlobal((int)size);
            try
            {
                ret = K4A.k4a_device_get_serialnum(device_handle, buffer, ref size);
                if (ret != k4a_buffer_result_t.K4A_BUFFER_RESULT_SUCCEEDED)
                {
                    throw new AzureKinectException(ret.ToString());
                }

                var serial = new byte[size];
                Marshal.Copy(buffer, serial, 0, (int)size);

                // '\0'は抜く
                serialNumber = Encoding.ASCII.GetString(serial, 0, (int)size - 1);
                return serialNumber;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        /// カメラを起動する Start the camera
        /// </summary>
        public void StartCamera(DeviceConfiguration config)
        {
			Debug.Log("device handle" + device_handle);
            var ret = K4A.k4a_device_start_cameras(device_handle, ref config);
			if (ret != k4a_result_t.K4A_RESULT_SUCCEEDED)
			{
				Debug.Log("Start camera result:" + ret.ToString());
				throw new AzureKinectException(ret); // when start cameras failed, ret.toString printed 'K4A_RESULT_SUCCEEDED' instead of failed
													 // removing the toString prints the correct error 'K4A_RESULT_FAILED'
            }
        }

        /// <summary>
        /// カメラを停止する Stop the camera
        /// </summary>
        public void StopCamera()
        {
            K4A.k4a_device_stop_cameras(device_handle);
        }

        public Capture GetNextCapture(Int32 timeout = -1)
        {
            return new Capture(this, timeout);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。
                Close();

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~KinectSensor()
        // {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
