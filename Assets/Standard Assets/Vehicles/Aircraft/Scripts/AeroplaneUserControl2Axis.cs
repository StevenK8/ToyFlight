using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.XR;
using System.Collections.Generic;


namespace UnityStandardAssets.Vehicles.Aeroplane
{
    [RequireComponent(typeof (AeroplaneController))]    
    public class AeroplaneUserControl2Axis : MonoBehaviour
    {
        // these max angles are only used on mobile, due to the way pitch and roll input are handled
        public float maxRollAngle = 80;
        public float maxPitchAngle = 80;

        // reference to the aeroplane that we're controlling
        private AeroplaneController m_Aeroplane;


        private void Awake()
        {
            // Set up the reference to the aeroplane controller.
            m_Aeroplane = GetComponent<AeroplaneController>();
        }


        private void FixedUpdate()
        {
            // Read input for the pitch, yaw, roll and throttle of the aeroplane.
            // set roll as the roll axis of the XR controller
            // if SystemInfo.deviceModel contains "Oculus" then use the Oculus XR controller
            float yaw = 0, roll = 0, pitch = 0;
            bool boost = false;
            if(SystemInfo.deviceModel != null &&  SystemInfo.deviceModel.Contains("Pico") ||  SystemInfo.deviceModel.Contains("Valve") || SystemInfo.deviceModel.Contains("Oculus"))
            {
                yaw = -InputTracking.GetLocalRotation(XRNode.LeftHand).y;
                pitch = -InputTracking.GetLocalRotation(XRNode.LeftHand).x;
                roll = InputTracking.GetLocalRotation(XRNode.LeftHand).z;
            }else{
                // read input from keyboard
                roll = Input.GetAxisRaw("Horizontal");
                pitch = Input.GetAxisRaw("Vertical");
                // Boost on spacebar press
                boost = Input.GetKey(KeyCode.Space);
            }
            // 
            // bool airBrakes = InputTracking.GetComponent(XRNode.LeftHand).GetPress(triggerButton);

            bool airBrakes = false;
            var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);

            if(leftHandDevices.Count == 1)
            {
                UnityEngine.XR.InputDevice device = leftHandDevices[0];

                bool triggerValue;
                if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out triggerValue) && triggerValue)
                {
                    boost = true;
                }else
                {
                    boost = false;
                }


                // bool menuButton;
                // if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out menuButton) && menuButton)
                // {
                //     m_Aeroplane.Shoot();
                // }

            }
            
            float throttle = 0;
            if (airBrakes){
                throttle = -1;
            }else{
                if(boost){
                    throttle = 1;
                }else{
                    throttle = 0.2f;
                }
            }

            // Debug.Log("throttle: " + throttle);

            // auto throttle up, or down if braking.
            // float throttle = airBrakes ? -1 : 0.1f;
#if MOBILE_INPUT
            // AdjustInputForMobileControls(ref roll, ref pitch, ref throttle);
#endif
            // Pass the input to the aeroplane
            m_Aeroplane.Move(roll, pitch, yaw, throttle, airBrakes);
        }


        private void AdjustInputForMobileControls(ref float roll, ref float pitch, ref float throttle)
        {
            // because mobile tilt is used for roll and pitch, we help out by
            // assuming that a centered level device means the user
            // wants to fly straight and level!

            // this means on mobile, the input represents the *desired* roll angle of the aeroplane,
            // and the roll input is calculated to achieve that.
            // whereas on non-mobile, the input directly controls the roll of the aeroplane.

            float intendedRollAngle = roll*maxRollAngle*Mathf.Deg2Rad;
            float intendedPitchAngle = pitch*maxPitchAngle*Mathf.Deg2Rad;
            roll = Mathf.Clamp((intendedRollAngle - m_Aeroplane.RollAngle), -1, 1);
            pitch = Mathf.Clamp((intendedPitchAngle - m_Aeroplane.PitchAngle), -1, 1);

            // similarly, the throttle axis input is considered to be the desired absolute value, not a relative change to current throttle.
            float intendedThrottle = throttle*0.5f + 0.5f;
            throttle = Mathf.Clamp(intendedThrottle - m_Aeroplane.Throttle, -1, 1);
        }
    }
}
