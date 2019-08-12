using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace HoloLab.AzureKinect
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DeviceConfiguration
    {
        [MarshalAs(UnmanagedType.I4)]
        public ImageFormat ColorFormat;

        [MarshalAs(UnmanagedType.I4)]
        public ColorResolution ColorResolution;

        [MarshalAs(UnmanagedType.I4)]
        public DepthMode DepthMode;

        [MarshalAs(UnmanagedType.I4)]
        public Fps CameraFps;

        [MarshalAs(UnmanagedType.U1)]
        public bool SynchronizedImagesOnly;

        [MarshalAs(UnmanagedType.I4)]
        public Int32 DepthDelayOffColorUsec;

        [MarshalAs(UnmanagedType.I4)]
        public WiredSyncMode WiredSyncMode;

        [MarshalAs(UnmanagedType.I4)]
        public UInt32 SubordinateDelayOffMasterUsec;

        [MarshalAs(UnmanagedType.U1)]
        public bool DisableStreamingIndicator;

		public override string ToString()
		{ // ColorBGRA32_1080PNarrowFOV_2x2_Binned_30True0Standalone0False
			//ColorBGRA32
			//_1080P
			//NarrowFOV_2x2_Binned
			//_30
			//True
			//0
			//Standalone
			//0
			//False
			return ColorFormat.ToString() + "\n" + ColorResolution.ToString() + "\n" + DepthMode.ToString() + "\n" + CameraFps.ToString() + "\n" +
				SynchronizedImagesOnly.ToString() + "\n" + DepthDelayOffColorUsec.ToString() + "\n" + WiredSyncMode.ToString() + "\n" +
				SubordinateDelayOffMasterUsec.ToString() + "\n" + DisableStreamingIndicator.ToString();
		}
	}
}
