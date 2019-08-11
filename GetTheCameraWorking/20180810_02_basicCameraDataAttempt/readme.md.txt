k4a methods work immediately if they return a uint
-like get_installed_count or displayHelloFromDll

For most other methods that return the k4a_result_t type
copy over the k4atypes.cs file from the Japanese team
It's namespaced as HoloLab.etc
I changed to zeroonetwo
Apply the namespace to the camera script
Enums now work and no errors

Repeat copy and namespace for Japanese team enum folder

Re startCamera confithttps://microsoft.github.io/Azure-Kinect-Sensor-SDK/master/group___definitions_ga4f693ace52a6eeec10cc6ca0350d6601.html

Tried to instantiate default disabled config:
const k4a_device_configuration_t K4A_DEVICE_CONFIG_INIT_DISABLE_ALL
= { K4A_IMAGE_FORMAT_COLOR_MJPG,                                          K4A_COLOR_RESOLUTION_OFF,                                                K4A_DEPTH_MODE_OFF,                                          K4A_FRAMES_PER_SECOND_30,                                                             false,                                                                 0,                                    K4A_WIRED_SYNC_MODE_STANDALONE,                                                                 0,                                                                false }

Kept getting errors about array initialization pattern
Tried k4a_device_configuration_t[] config = new k4a_device_configuration_t[] { ... };
But we have a mixture of data types
Generic class wouldn't work quickly either

Copied from Japanese team:
var config = new DeviceConfiguration();

Seems to work, no errors. But haven't tried pulling data yet
