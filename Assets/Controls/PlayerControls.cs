// GENERATED AUTOMATICALLY FROM 'Assets/Controls/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""GunGuy"",
            ""id"": ""9e957278-b574-4870-ba6d-947ecea83f4a"",
            ""actions"": [
                {
                    ""name"": ""MoveLeft"",
                    ""type"": ""Button"",
                    ""id"": ""370d996c-3029-4272-bee8-ae8b8378f0bc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveRight"",
                    ""type"": ""Button"",
                    ""id"": ""9f101fc5-96cd-43b9-b992-b32cf7de1fa0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ArmAimBack"",
                    ""type"": ""Value"",
                    ""id"": ""6af3625c-23d2-4c25-84ec-2469e2baeaac"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ArmAimFront"",
                    ""type"": ""Value"",
                    ""id"": ""37e6271b-76bf-4e56-88cf-9cb84d41d7ff"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShootBack"",
                    ""type"": ""Button"",
                    ""id"": ""c86c6232-8d11-46de-b5d5-d76414d9eb5b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShootFront"",
                    ""type"": ""Button"",
                    ""id"": ""e0e5dccc-613d-4017-851e-3a25688347e4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""BackArm1"",
                    ""type"": ""Button"",
                    ""id"": ""f7dd9dc6-9aed-4142-9a42-e2eda1c35dcd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""BackArm2"",
                    ""type"": ""Button"",
                    ""id"": ""627ca8ea-defc-47d2-862d-672bcfb9e314"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""BackArm3"",
                    ""type"": ""Button"",
                    ""id"": ""8684786b-b415-4c13-bae4-2cbd2236c616"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""BackArm4"",
                    ""type"": ""Button"",
                    ""id"": ""ac606eef-5e2c-477c-b14c-19717f4985d4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FrontArm1"",
                    ""type"": ""Button"",
                    ""id"": ""cbf1d193-344f-4b52-be34-a23eaca7a461"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FrontArm2"",
                    ""type"": ""Button"",
                    ""id"": ""c69acdda-ec51-45f2-8a04-4c171e58da18"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FrontArm3"",
                    ""type"": ""Button"",
                    ""id"": ""d306685d-d354-4eb2-b20f-eb47a8c455ed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FrontArm4"",
                    ""type"": ""Button"",
                    ""id"": ""fb1b32e9-97dc-43d6-8629-0d3d53a9244c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0ac22792-f90b-48ee-a4ba-fe68ce57e5bf"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ArmAimBack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c76e128-c9e8-441a-9472-a3079f528d4a"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ArmAimFront"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7601c7ce-5f62-43be-818e-07e2c3aaebf0"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShootBack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ea921c02-a48a-4f69-aacb-b1398cd2b5c9"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShootFront"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""abe70862-ecd7-4d43-8ff9-53be59246018"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BackArm1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""baf01e3e-2f52-4c8f-a1f5-b2150233d481"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BackArm2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b12a0503-df6a-4b8b-9514-54feb5de28b5"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BackArm3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bdf9d5b1-cd76-452d-a885-ba51da0d3d41"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BackArm4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""26f60892-7215-44bc-a6eb-bfebd47a3025"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FrontArm1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1f0626de-f5e4-4e19-a7d7-fcfc1c4c399c"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FrontArm2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""89c792f4-eb39-4da5-ae2b-80ee16602ed3"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FrontArm3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb93a388-6e52-436b-9c1a-6256d0168d99"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FrontArm4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1db75b8b-ccbf-45f2-8afd-ed1e3fe61407"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7b57fb56-85a7-448a-9e56-2fad0170b393"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GunGuy
        m_GunGuy = asset.FindActionMap("GunGuy", throwIfNotFound: true);
        m_GunGuy_MoveLeft = m_GunGuy.FindAction("MoveLeft", throwIfNotFound: true);
        m_GunGuy_MoveRight = m_GunGuy.FindAction("MoveRight", throwIfNotFound: true);
        m_GunGuy_ArmAimBack = m_GunGuy.FindAction("ArmAimBack", throwIfNotFound: true);
        m_GunGuy_ArmAimFront = m_GunGuy.FindAction("ArmAimFront", throwIfNotFound: true);
        m_GunGuy_ShootBack = m_GunGuy.FindAction("ShootBack", throwIfNotFound: true);
        m_GunGuy_ShootFront = m_GunGuy.FindAction("ShootFront", throwIfNotFound: true);
        m_GunGuy_BackArm1 = m_GunGuy.FindAction("BackArm1", throwIfNotFound: true);
        m_GunGuy_BackArm2 = m_GunGuy.FindAction("BackArm2", throwIfNotFound: true);
        m_GunGuy_BackArm3 = m_GunGuy.FindAction("BackArm3", throwIfNotFound: true);
        m_GunGuy_BackArm4 = m_GunGuy.FindAction("BackArm4", throwIfNotFound: true);
        m_GunGuy_FrontArm1 = m_GunGuy.FindAction("FrontArm1", throwIfNotFound: true);
        m_GunGuy_FrontArm2 = m_GunGuy.FindAction("FrontArm2", throwIfNotFound: true);
        m_GunGuy_FrontArm3 = m_GunGuy.FindAction("FrontArm3", throwIfNotFound: true);
        m_GunGuy_FrontArm4 = m_GunGuy.FindAction("FrontArm4", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // GunGuy
    private readonly InputActionMap m_GunGuy;
    private IGunGuyActions m_GunGuyActionsCallbackInterface;
    private readonly InputAction m_GunGuy_MoveLeft;
    private readonly InputAction m_GunGuy_MoveRight;
    private readonly InputAction m_GunGuy_ArmAimBack;
    private readonly InputAction m_GunGuy_ArmAimFront;
    private readonly InputAction m_GunGuy_ShootBack;
    private readonly InputAction m_GunGuy_ShootFront;
    private readonly InputAction m_GunGuy_BackArm1;
    private readonly InputAction m_GunGuy_BackArm2;
    private readonly InputAction m_GunGuy_BackArm3;
    private readonly InputAction m_GunGuy_BackArm4;
    private readonly InputAction m_GunGuy_FrontArm1;
    private readonly InputAction m_GunGuy_FrontArm2;
    private readonly InputAction m_GunGuy_FrontArm3;
    private readonly InputAction m_GunGuy_FrontArm4;
    public struct GunGuyActions
    {
        private @PlayerControls m_Wrapper;
        public GunGuyActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveLeft => m_Wrapper.m_GunGuy_MoveLeft;
        public InputAction @MoveRight => m_Wrapper.m_GunGuy_MoveRight;
        public InputAction @ArmAimBack => m_Wrapper.m_GunGuy_ArmAimBack;
        public InputAction @ArmAimFront => m_Wrapper.m_GunGuy_ArmAimFront;
        public InputAction @ShootBack => m_Wrapper.m_GunGuy_ShootBack;
        public InputAction @ShootFront => m_Wrapper.m_GunGuy_ShootFront;
        public InputAction @BackArm1 => m_Wrapper.m_GunGuy_BackArm1;
        public InputAction @BackArm2 => m_Wrapper.m_GunGuy_BackArm2;
        public InputAction @BackArm3 => m_Wrapper.m_GunGuy_BackArm3;
        public InputAction @BackArm4 => m_Wrapper.m_GunGuy_BackArm4;
        public InputAction @FrontArm1 => m_Wrapper.m_GunGuy_FrontArm1;
        public InputAction @FrontArm2 => m_Wrapper.m_GunGuy_FrontArm2;
        public InputAction @FrontArm3 => m_Wrapper.m_GunGuy_FrontArm3;
        public InputAction @FrontArm4 => m_Wrapper.m_GunGuy_FrontArm4;
        public InputActionMap Get() { return m_Wrapper.m_GunGuy; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GunGuyActions set) { return set.Get(); }
        public void SetCallbacks(IGunGuyActions instance)
        {
            if (m_Wrapper.m_GunGuyActionsCallbackInterface != null)
            {
                @MoveLeft.started -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnMoveLeft;
                @MoveLeft.performed -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnMoveLeft;
                @MoveLeft.canceled -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnMoveLeft;
                @MoveRight.started -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnMoveRight;
                @MoveRight.performed -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnMoveRight;
                @MoveRight.canceled -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnMoveRight;
                @ArmAimBack.started -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnArmAimBack;
                @ArmAimBack.performed -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnArmAimBack;
                @ArmAimBack.canceled -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnArmAimBack;
                @ArmAimFront.started -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnArmAimFront;
                @ArmAimFront.performed -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnArmAimFront;
                @ArmAimFront.canceled -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnArmAimFront;
                @ShootBack.started -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnShootBack;
                @ShootBack.performed -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnShootBack;
                @ShootBack.canceled -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnShootBack;
                @ShootFront.started -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnShootFront;
                @ShootFront.performed -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnShootFront;
                @ShootFront.canceled -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnShootFront;
                @BackArm1.started -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnBackArm1;
                @BackArm1.performed -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnBackArm1;
                @BackArm1.canceled -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnBackArm1;
                @BackArm2.started -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnBackArm2;
                @BackArm2.performed -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnBackArm2;
                @BackArm2.canceled -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnBackArm2;
                @BackArm3.started -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnBackArm3;
                @BackArm3.performed -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnBackArm3;
                @BackArm3.canceled -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnBackArm3;
                @BackArm4.started -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnBackArm4;
                @BackArm4.performed -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnBackArm4;
                @BackArm4.canceled -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnBackArm4;
                @FrontArm1.started -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnFrontArm1;
                @FrontArm1.performed -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnFrontArm1;
                @FrontArm1.canceled -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnFrontArm1;
                @FrontArm2.started -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnFrontArm2;
                @FrontArm2.performed -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnFrontArm2;
                @FrontArm2.canceled -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnFrontArm2;
                @FrontArm3.started -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnFrontArm3;
                @FrontArm3.performed -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnFrontArm3;
                @FrontArm3.canceled -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnFrontArm3;
                @FrontArm4.started -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnFrontArm4;
                @FrontArm4.performed -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnFrontArm4;
                @FrontArm4.canceled -= m_Wrapper.m_GunGuyActionsCallbackInterface.OnFrontArm4;
            }
            m_Wrapper.m_GunGuyActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveLeft.started += instance.OnMoveLeft;
                @MoveLeft.performed += instance.OnMoveLeft;
                @MoveLeft.canceled += instance.OnMoveLeft;
                @MoveRight.started += instance.OnMoveRight;
                @MoveRight.performed += instance.OnMoveRight;
                @MoveRight.canceled += instance.OnMoveRight;
                @ArmAimBack.started += instance.OnArmAimBack;
                @ArmAimBack.performed += instance.OnArmAimBack;
                @ArmAimBack.canceled += instance.OnArmAimBack;
                @ArmAimFront.started += instance.OnArmAimFront;
                @ArmAimFront.performed += instance.OnArmAimFront;
                @ArmAimFront.canceled += instance.OnArmAimFront;
                @ShootBack.started += instance.OnShootBack;
                @ShootBack.performed += instance.OnShootBack;
                @ShootBack.canceled += instance.OnShootBack;
                @ShootFront.started += instance.OnShootFront;
                @ShootFront.performed += instance.OnShootFront;
                @ShootFront.canceled += instance.OnShootFront;
                @BackArm1.started += instance.OnBackArm1;
                @BackArm1.performed += instance.OnBackArm1;
                @BackArm1.canceled += instance.OnBackArm1;
                @BackArm2.started += instance.OnBackArm2;
                @BackArm2.performed += instance.OnBackArm2;
                @BackArm2.canceled += instance.OnBackArm2;
                @BackArm3.started += instance.OnBackArm3;
                @BackArm3.performed += instance.OnBackArm3;
                @BackArm3.canceled += instance.OnBackArm3;
                @BackArm4.started += instance.OnBackArm4;
                @BackArm4.performed += instance.OnBackArm4;
                @BackArm4.canceled += instance.OnBackArm4;
                @FrontArm1.started += instance.OnFrontArm1;
                @FrontArm1.performed += instance.OnFrontArm1;
                @FrontArm1.canceled += instance.OnFrontArm1;
                @FrontArm2.started += instance.OnFrontArm2;
                @FrontArm2.performed += instance.OnFrontArm2;
                @FrontArm2.canceled += instance.OnFrontArm2;
                @FrontArm3.started += instance.OnFrontArm3;
                @FrontArm3.performed += instance.OnFrontArm3;
                @FrontArm3.canceled += instance.OnFrontArm3;
                @FrontArm4.started += instance.OnFrontArm4;
                @FrontArm4.performed += instance.OnFrontArm4;
                @FrontArm4.canceled += instance.OnFrontArm4;
            }
        }
    }
    public GunGuyActions @GunGuy => new GunGuyActions(this);
    public interface IGunGuyActions
    {
        void OnMoveLeft(InputAction.CallbackContext context);
        void OnMoveRight(InputAction.CallbackContext context);
        void OnArmAimBack(InputAction.CallbackContext context);
        void OnArmAimFront(InputAction.CallbackContext context);
        void OnShootBack(InputAction.CallbackContext context);
        void OnShootFront(InputAction.CallbackContext context);
        void OnBackArm1(InputAction.CallbackContext context);
        void OnBackArm2(InputAction.CallbackContext context);
        void OnBackArm3(InputAction.CallbackContext context);
        void OnBackArm4(InputAction.CallbackContext context);
        void OnFrontArm1(InputAction.CallbackContext context);
        void OnFrontArm2(InputAction.CallbackContext context);
        void OnFrontArm3(InputAction.CallbackContext context);
        void OnFrontArm4(InputAction.CallbackContext context);
    }
}
