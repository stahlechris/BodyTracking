// Copyright(c) 2019 HoloLab Inc.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php

using HoloLab.AzureKinect.NativeMethod;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoloLab.AzureKinect
{
    public class AzureKinectException : Exception
    {
        k4a_result_t result;
        k4a_wait_result_t wait_result;

        public AzureKinectException(k4a_result_t result)
        {
            this.result = result;
        }

		public AzureKinectException(k4a_wait_result_t wait_result)
        {
            this.wait_result = wait_result;
        }

        public AzureKinectException(string message)
            :base(message)
        {
        }

        public override string Message { get { return result.ToString(); } }
	}
}
