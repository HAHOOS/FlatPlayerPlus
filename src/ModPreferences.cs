using MelonLoader;

using UnityEngine;

using FlatPlayer;

using FlatPlayerPlus.MonoBehaviours;

namespace FlatPlayerPlus
{
    public static class ModPreferences
    {
        private static MelonPreferences_Category _category;
        private static MelonPreferences_Category _fpCategory;

        private static MelonPreferences_Entry<float> _fpCameraFOV;
        private static MelonPreferences_Entry<float> _fpHandsExtendSensitivity;
        private static MelonPreferences_Entry<float> _fpCameraSmoothness;

        private static MelonPreferences_Entry<FPP_UI_Handler.UIPosition> _healthBarPosition;
        private static MelonPreferences_Entry<FPP_UI_Handler.UIPosition> _ammoPosition;
        private static MelonPreferences_Entry<bool> _toggleLeftGrip;
        private static MelonPreferences_Entry<bool> _toggleRightGrip;
        private static MelonPreferences_Entry<int> _fpsLimit;
        private static MelonPreferences_Entry<FullScreenMode> _fullscreenMode;
        private static MelonPreferences_Entry<bool> _vSync;
        public static MelonPreferences_Entry<KeyCode> ReloadKey { get; private set; }
        public static MelonPreferences_Entry<KeyCode> DriveForwardKey { get; private set; }
        public static MelonPreferences_Entry<KeyCode> RagdollKey { get; private set; }

        public static void CreatePreferences()
        {
            _category = MelonPreferences.CreateCategory("FlatPlayerPlus");
            _fpCategory = MelonPreferences.GetCategory("FlatPlayer");
            _healthBarPosition = _category.CreateEntry("healthBarPosition", FPP_UI_Handler.UIPosition.Top);
            _ammoPosition = _category.CreateEntry("ammoPosition", FPP_UI_Handler.UIPosition.Bottom);
            _toggleLeftGrip = _category.CreateEntry("toggleLeftGrip", false);
            _toggleRightGrip = _category.CreateEntry("toggleRightGrip", false);
            _fpsLimit = _category.CreateEntry("fpsLimit", 60);
            _fullscreenMode = _category.CreateEntry("fullscreenMode", FullScreenMode.FullScreenWindow);
            _vSync = _category.CreateEntry("vSyncEnabled", true);

            ReloadKey = _category.CreateEntry("ReloadKey", KeyCode.R,
                description:
                "For more info about key names, go to https://docs.unity3d.com/2021.3/Documentation/ScriptReference/KeyCode.html");
            DriveForwardKey = _category.CreateEntry("DriveForward", KeyCode.W);
            RagdollKey = _category.CreateEntry("RagdollKey", KeyCode.LeftControl);

            _fpCameraFOV = _fpCategory.GetEntry<float>("CameraFOV");
            _fpHandsExtendSensitivity = _fpCategory.GetEntry<float>("HandsExtendSensitivity");
            _fpCameraSmoothness = _fpCategory.GetEntry<float>("CameraSmoothness");
        }

        public static void SavePreferences()
        {
            _healthBarPosition.Value = (FPP_UI_Handler.UIPosition)Mod.HealthBarPositionElement.Value;
            _ammoPosition.Value = (FPP_UI_Handler.UIPosition)Mod.AmmoPositionElement.Value;
            _toggleLeftGrip.Value = Mod.ToggleLeftGripElement.Value;
            _toggleRightGrip.Value = Mod.ToggleRightGripElement.Value;
            _fpsLimit.Value = Mod.FPSLimit.Value;
            _fullscreenMode.Value = (FullScreenMode)Mod.FullscreenModeElement.Value;
            _vSync.Value = Mod.VSyncElement.Value;

            _fpCameraFOV.Value = Mod.FOVElement.Value;
            _fpHandsExtendSensitivity.Value = Mod.HandsExtendSensitivity.Value;
            _fpCameraSmoothness.Value = Mod.CameraSmoothness.Value;

            _fpCategory.SaveToFile(false);
            _category.SaveToFile(false);
        }

        public static void LoadPreferences()
        {
            //Loading Elements
            Mod.HealthBarPositionElement.Value = _healthBarPosition.Value;
            Mod.AmmoPositionElement.Value = _ammoPosition.Value;
            Mod.ToggleLeftGripElement.Value = _toggleLeftGrip.Value;
            Mod.ToggleRightGripElement.Value = _toggleRightGrip.Value;
            Mod.FPSLimit.Value = _fpsLimit.Value;
            Mod.FullscreenModeElement.Value = _fullscreenMode.Value;
            Mod.FOVElement.Value = _fpCameraFOV.Value;
            Mod.VSyncElement.Value = _vSync.Value;
            Mod.HandsExtendSensitivity.Value = _fpHandsExtendSensitivity.Value;
            Mod.CameraSmoothness.Value = _fpCameraFOV.Value;

            //Loading Values
            Mod.UIHandler.UpdateUIPosition(_healthBarPosition.Value, _ammoPosition.Value);
            QualitySettings.vSyncCount = _vSync.Value ? 1 : 0;
            if (_vSync.Value)
                Application.targetFrameRate = _fpsLimit.Value;
            Screen.fullScreenMode = _fullscreenMode.Value;
            FlatBooter.MainCamera.fieldOfView = _fpCameraFOV.Value;

            FlatBooter.Instance.ReloadConfig();
        }
    }
}