IMPORTANT TO READ FOR CTAA NXT V2, PLEASE ALSO READ THE Documentation Update Even if you had a Previous version of CTAA

Thank you for purchasing the premiere Temporal Anti-Aliasing Solution for Unity

SET UNITY TO LINEAR COLOR SPACE from Build Settings -> Player Settings -> Other Settings

If you like to use CTAA for VR please download and install SteamVR and Oculus Integration packages from the Unity Asset Store. These are NOT however required to get CTAA working so please look at the Demos for each platform.

VR SPECIFIC - IMPORTANT PLEASE READ THE FOLLOWING!

For Best Quality and Performance In general we STRONGLY recommend the use of the Dedicated version per HMD, such as the Oculus version for Oculus and VIVE version for VIVE. If you like to support both, then use the VIVE version, which is OPENVR and supports all VR Devices. The SPS (Single Pass Stereo) version should only be used if you must use a single camera, The SPS version Quality is slightly lower due to Precision Issues.

So, our recommendation for VR will be to use the Open VR version together with 2xMSAA when in forward render mode. The SuperSampling option in CTAA NXT V2 is not available in VR due to extreme  high performance requirements, It is also not necessary as CTAA in VR is more then sufficient to achieving great results.

DYNAMIC MOVING OBJECTS IN VR:::

Since Unity does not provide native motion vectors in VR (Required for CTAA) we render our own custum motion vectors. A script needs to be attached to each dynamic objec when using CTAA in VR.

Attach the following scripts depening on the CTAA version used

CTAA SPS ->                 DynamicObjectCTAASPS script

CTAA VR OCULUS ->     CTAA_Dynamic_Tag

CTAA VR VIVE ->          DynamicObjectTag


Contact us if you like to use and obtain CTAA for any Console Development

****CTAA UNITY PACKAGE STRUCTURE****

There are Four different variations of CTAA for each platform PC, OCULUS ( Oculus SDK ), HTC VIVE ( OpenVR which supports both Oculus and VIVE, WMR ) and as of V1.6 CTAA SPS (SINGLE PASS STEREO, which can be used either with Oculus SDK or OpenVR/ STEAM VR SDK with Single Camera Setups)

DO NOT TOUCH THE ' DemoResources' Folder or move it and DO NOT RUN the Scenes in this folder.

CTAA Demos for each platform are found in their respective folders for that platform.



---  Folders  ---

DemoResources :: This includes all the shared resources, do not move it or chage it and dont run the demos in this folder

LIVENDA CTAA SPS :: Unified Single Pass Stereo and Single Camera version for Oculus and Open VR (STEAM VR SDK)

LIVENDA CTAA VR OCULUS :: Use this for ALL Oculus SDK projects (Multi Pass) , PLEASE NOTE:  CTAA SPS Script can be used in all camera rigs if you only require a single Camera, make sure to set XR settings to Single Pass

LIVENDA CTAA VR VIVE :: Use this for ALL  OpenVR /Steam VR SDK Projects (Multi-Pass), PLEASE NOTE:  CTAA SPS Script can be used in all camera rigs if you only require a single Camera, make sure to set XR settings to Single Pass

LIVENDA_CTAA_PC :: Use this for ALL PC Projects

VRTK_INTEGRATION ::  VRTK is an opensource Free VR Toolkit for rapidly building VR solutions which supports SteamVR and Oculus SDK. Multi Pass Only, Please use CTAA SPS for Single Camera Integration with VRTK 