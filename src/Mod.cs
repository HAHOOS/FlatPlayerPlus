using System.Reflection;

using BoneLib;
using BoneLib.BoneMenu;

using FlatPlayer;

using FlatPlayerPlus.MonoBehaviours;

using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Bonelab.SaveData;
using Il2CppSLZ.Marrow;

using MelonLoader;

using UnityEngine;

using AmmoInventory = Il2CppSLZ.Marrow.AmmoInventory;
using Avatar = Il2CppSLZ.VRMK.Avatar;
using Object = UnityEngine.Object;
using Page = BoneLib.BoneMenu.Page;

[assembly: MelonInfo(typeof(FlatPlayerPlus.Mod), "FlatPlayerPlus", "2.0.1", "HL2H0", "https://thunderstore.io/c/bonelab/p/HL2H0/FlatPlayerPlus/")]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]
[assembly: MelonOptionalDependencies("RagdollPlayer")]

namespace FlatPlayerPlus
{
    public class Mod : MelonMod
    {
        public static Page MainPage { get; private set; }
        public static Page GraphicsPage { get; private set; }
        public static Page TogglesPage { get; private set; }
        public static Page UIPage { get; private set; }
        public static Page FlatPlayerPage { get; private set; }

        public static HandLocation ReloadHand => (!DataManager.ActiveSave?.PlayerSettings?.BeltLocationRight ?? true) ? HandLocation.Right : HandLocation.Left;

        public static EnumElement FullscreenModeElement { get; private set; }
        public static BoolElement VSyncElement { get; private set; }
        public static FloatElement FOVElement { get; private set; }
        public static IntElement FPSLimit { get; private set; }
        public static BoolElement ToggleRightGripElement { get; private set; }
        public static BoolElement ToggleLeftGripElement { get; private set; }
        public static EnumElement HealthBarPositionElement { get; private set; }
        public static EnumElement AmmoPositionElement { get; private set; }
        public static FloatElement HandsExtendSensitivity { get; private set; }
        public static FloatElement CameraSmoothness { get; private set; }

        public static bool RightGripToggled { get; private set; }
        public static bool LeftGripToggled { get; private set; }

        private static GameObject _uiPrefab;

        private static GameObject UI { get; set; }
        public static FPP_UI_Handler UIHandler { get; set; }

        public static bool HasRagdollPlayer => FindMelon("Ragdoll Player", "Lakatrazz") != null;

        public enum HandLocation
        {
            Right,
            Left
        }

        private static void ReloadGun()
        {
            var gun = Player.GetComponentInHand<Gun>(GetHand(false));
            var mag = Player.GetComponentInHand<Magazine>(GetHand(true));
            if (gun && mag && !gun.HasMagazine())
            {
                mag.Despawn();

                gun.InstantLoadAsync();
                gun.CompleteSlidePull();
                gun.CompleteSlideReturn();
            }
        }

        private static Hand GetHand(bool inverse = false)
            => (ReloadHand == HandLocation.Right && !inverse) ? Player.RightHand : Player.LeftHand;

        private static void SetupBoneMenu()
        {
            FPSLimit = new IntElement("FPS Limit", Color.white, 60, 1, 0, int.MaxValue, v =>
            {
                Application.targetFrameRate = v;
                ModPreferences.SavePreferences();
            });

            MainPage = Page.Root.CreatePage("FlatPlayerPlus", Color.green);
            TogglesPage = MainPage.CreatePage("Toggles", Color.yellow);
            ToggleRightGripElement = TogglesPage.CreateBool("Toggle Right Grip", Color.white, false, _ => ModPreferences.SavePreferences());
            ToggleLeftGripElement = TogglesPage.CreateBool("Toggle Left Grip", Color.white, false, null);

            UIPage = MainPage.CreatePage("UI", Color.magenta);
            HealthBarPositionElement = UIPage.CreateEnum("Health Bar Position", Color.white, FPP_UI_Handler.HealthBarUIPosition,
                v => UIHandler.UpdateUIPosition((FPP_UI_Handler.UIPosition)v, (FPP_UI_Handler.UIPosition)AmmoPositionElement.Value));
            AmmoPositionElement = UIPage.CreateEnum("Ammo Position", Color.white, FPP_UI_Handler.AmmoUIPosition, v => UIHandler.UpdateUIPosition((FPP_UI_Handler.UIPosition)HealthBarPositionElement.Value, (FPP_UI_Handler.UIPosition)v));

            FlatPlayerPage = MainPage.CreatePage("FlatPlayer Settings", Color.yellow);
            HandsExtendSensitivity = FlatPlayerPage.CreateFloat("Hand Extend Sensitivity", Color.white, 0.1f, 0.1f, 0, int.MaxValue, _ =>
            {
                ModPreferences.SavePreferences();
                FlatBooter.Instance.ReloadConfig();
            });
            CameraSmoothness = FlatPlayerPage.CreateFloat("Camera Smoothness", Color.white, 0.3f, 0.1f, 0, 1, _ =>
            {
                ModPreferences.SavePreferences();
                FlatBooter.Instance.ReloadConfig();
            });
            GraphicsPage = MainPage.CreatePage("Graphics", Color.green);
            FOVElement = GraphicsPage.CreateFloat("FOV", Color.white, 90, 1, 0, int.MaxValue, v =>
            {
                FlatBooter.MainCamera.fieldOfView = v;
                ModPreferences.SavePreferences();
            });
            FullscreenModeElement = GraphicsPage.CreateEnum("Full Screen Mode", Color.white, Screen.fullScreenMode,
                v =>
                {
                    Screen.fullScreenMode = (FullScreenMode)v;
                    ModPreferences.SavePreferences();
                });
            VSyncElement = GraphicsPage.CreateBool("V-Sync", Color.white, false, v =>
            {
                QualitySettings.vSyncCount = v ? 1 : 0;
                if (v)
                {
                    MainPage.Remove(FPSLimit);
                }
                else
                {
                    MainPage.Add(FPSLimit);
                    Application.targetFrameRate = FPSLimit.Value;
                }
                ModPreferences.SavePreferences();
            });
        }

        private static void InitializeBundles()
        {
            FieldInjector.SerialisationHandler.Inject<FPP_UI_Handler>();
            const string bundlePath = "FlatPlayerPlus.Resources.flatplayerplus.pack";
            var bundle = HelperMethods.LoadEmbeddedAssetBundle(Assembly.GetExecutingAssembly(), bundlePath);
            _uiPrefab = bundle.LoadPersistentAsset<GameObject>("FP+ UI");
        }

        public override void OnInitializeMelon()
        {
            if (FindMelon("FlatPlayer", "LlamasHere") == null)
            {
                LoggerInstance.Warning("FlatPlayer not found! Deinitializing...");
                this.Unregister("FlatPlayer is required for FlatPlayerPlus to work!", true);
                return;
            }

            SetupBoneMenu();
            InitializeBundles();
            ModPreferences.CreatePreferences();
            Hooking.OnUIRigCreated += HookingOnOnUIRigCreated;
            Hooking.OnSwitchAvatarPostfix += OnAvatarSwitch;
            LoggerInstance.Msg("FlatPlayer+ 2.0.0 Initialized.");
        }

        private static void OnAvatarSwitch(Avatar obj)
            => UIHandler.MaxHealth = Player.RigManager.health.max_Health;

        private static void HookingOnOnUIRigCreated()
        {
            if (_uiPrefab != null)
            {
                UI = Object.Instantiate(_uiPrefab);
                UIHandler = UI.GetComponent<FPP_UI_Handler>();
                UIHandler.MaxHealth = Player.RigManager.health.max_Health;
                ModPreferences.LoadPreferences();
            }
        }

        public override void OnUpdate()
        {
            base.OnLateUpdate();
            if (!FlatBooter.IsReady) return;

            UIHandler.CurrHealth = Player.RigManager.health.curr_Health;

            var lightAmmo = AmmoInventory.Instance._groupCounts["light"];
            var mediumAmmo = AmmoInventory.Instance._groupCounts["medium"];
            var heavyAmmo = AmmoInventory.Instance._groupCounts["heavy"];
            UIHandler.UpdateAmmoText(lightAmmo, mediumAmmo, heavyAmmo);

            //Reload
            if (Input.GetKeyDown(ModPreferences.ReloadKey.Value))
                ReloadGun();

            //DriveForwards
            if (Input.GetKey(ModPreferences.DriveForwardKey.Value) && Player.RigManager.activeSeat)
            {
                Player.RightController._thumbstickAxis = new Vector2(0, 1);
                Player.LeftController._thumbstickAxis = new Vector2(0, -1);
            }

            //Ragdoll
            if (Input.GetKeyDown(ModPreferences.RagdollKey.Value))
            {
                // This could be done without RagdollPlayer honestly
                if (HasRagdollPlayer)
                    Ragdoll();
                else
                    LoggerInstance.Warning("RagdollPlayer not found! It is required for the ragdoll functionality to work");
            }
        }

        private static void Ragdoll()
        {
            RigManager rigManager = Player.RigManager;
            if (!rigManager?.activeSeat && !UIRig.Instance.popUpMenu.m_IsCursorShown)
            {
                PhysicsRig physicsRig = Player.PhysicsRig;
                if (!physicsRig.torso.shutdown && physicsRig.ballLocoEnabled)
                    RagdollPlayer.RagdollPlayerMod.RagdollRig(rigManager);
                else
                    RagdollPlayer.RagdollPlayerMod.UnragdollRig(rigManager);
            }
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            if (!FlatBooter.IsReady) return;

            if (Input.GetMouseButtonDown(0))
                LeftGripToggled = !LeftGripToggled;

            if (Input.GetMouseButtonDown(1))
                RightGripToggled = !RightGripToggled;

            var rightGrip = RightGripToggled ? 1 : 0;
            var leftGrip = LeftGripToggled ? 1 : 0;

            if (ToggleRightGripElement.Value)
                FlatBooter.RightController.Grip = rightGrip;
            if (ToggleLeftGripElement.Value)
                FlatBooter.LeftController.Grip = leftGrip;
        }
    }
}