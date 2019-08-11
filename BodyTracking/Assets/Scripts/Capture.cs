using HoloLab.AzureKinect.NativeMethod;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HoloLab.AzureKinect
{
    public class Capture : IDisposable
    {
        IntPtr capture_handle = IntPtr.Zero;

		public Capture(KinectSensor sensor, Int32 timeout)
		{
			var ret = Open(sensor, timeout);
			if (!ret)
			{
				Debug.Log("open w/in capture:" + ret.ToString());
				throw new AzureKinectException(k4a_wait_result_t.K4A_WAIT_RESULT_TIMEOUT.ToString());
			}
		}

		public bool Open(KinectSensor sensor, Int32 timeout)
		{
			var ret = K4A.k4a_device_get_capture(sensor.device_handle, out capture_handle, timeout);
			Debug.Log("open ret:" + ret.ToString());
			Debug.Log(
				k4a_wait_result_t.K4A_WAIT_RESULT_FAILED.ToString()
				.Equals(ret.ToString(), StringComparison.Ordinal));


			if (ret == k4a_wait_result_t.K4A_WAIT_RESULT_TIMEOUT)
			{
				Debug.Log("open w/in open timeout:" + ret.ToString());
				return false;
			}
			else if (ret == k4a_wait_result_t.K4A_WAIT_RESULT_FAILED)
			{
				Debug.Log("open w/in open failed:" + ret.ToString());
				throw new AzureKinectException(ret.ToString());
			}
			
			Debug.Log("open w/in open good:" + ret.ToString());
			return true;
		}

		public void Close()
        {
            if (capture_handle != IntPtr.Zero)
            {
                K4A.k4a_capture_release(capture_handle);
                capture_handle = IntPtr.Zero;
            }
        }

        public Image GetColorImage()
        {
            return new Image(K4A.k4a_capture_get_color_image(capture_handle));
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
        // ~Capture()
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
