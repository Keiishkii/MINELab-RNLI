using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public partial class ExperimentUXML
{
    public TutorialAVScaleContainer tutorialAVScaleContainer;
    [Serializable] public class TutorialAVScaleContainer : IContainer
    {
        #region [ Unserialised Fields ]
        private const string _avScaleContainerID = "TutorialAVScaleContainer";
        private const string _avScalePointerID = "AVScalePointer";
        private const string _avScaleTargetID = "AVScaleTarget";
        private const string _avScaleRingID = "AVScaleRing";
        private const string _contentContainerID = "ContentContainer";
        private const string _footerContainerID = "FooterContainer";
        private const string _title = "TitleLabel";
        private const string _content = "ContentLabel";
        private const string _footer = "FooterLabel";
        
        private readonly VisualElement _avScalePointerVisualElement;
        private readonly VisualElement _avScaleTargetVisualElement;
        private readonly VisualElement _avScaleRingVisualElement;

        private readonly VisualElement _contentContainerVisualElement;
        private readonly VisualElement _footerContainerVisualElement;
        private readonly Label _titleLabel;
        private readonly Label _contentLabel;
        private readonly Label _footerLabel;

        private readonly InputManager _inputManager;
        #endregion

        public TutorialAVScaleContainer(ExperimentUXML experimentUxml, VisualElement root) : base(experimentUxml, root, _avScaleContainerID)
        {
            _avScalePointerVisualElement = _containerVisualElement.Q<VisualElement>(_avScalePointerID);
            _avScaleTargetVisualElement = _containerVisualElement.Q<VisualElement>(_avScaleTargetID);
            _avScaleRingVisualElement = _containerVisualElement.Q<VisualElement>(_avScaleRingID);
            
            _contentContainerVisualElement = _containerVisualElement.Q<VisualElement>(_contentContainerID);
            _footerContainerVisualElement = _containerVisualElement.Q<VisualElement>(_footerContainerID);
            
            _titleLabel = _contentContainerVisualElement.Q<Label>(_title);
            _contentLabel = _contentContainerVisualElement.Q<Label>(_content);
            _footerLabel = _footerContainerVisualElement.Q<Label>(_footer);
            
            _inputManager = FindObjectOfType<InputManager>();

            HidePointer();
            HideTarget();
        }

        #region [ Container Controls ]
        public override IEnumerator DisplayCoroutine()
        {
            _inputManager.participantConformationActionReference.action.performed += OnParticipantConformationPressed;
            _inputManager.participantConformationActionReference.action.canceled += OnParticipantConformationReleased;
            
            _inputManager.avScaleInputActionReference.action.performed += DisplayPointer;
            _inputManager.avScaleInputActionReference.action.canceled += HidePointer;

            _avScaleRingVisualElement.style.display = (_inputManager.participantConformationActionReference.action.ReadValue<float>() > 0.5f) ? DisplayStyle.Flex : DisplayStyle.None;
            
            yield return base.DisplayCoroutine();
        }

        public override IEnumerator HideCoroutine()
        {
            yield return base.HideCoroutine();
            
            _inputManager.participantConformationActionReference.action.performed -= OnParticipantConformationPressed;
            _inputManager.participantConformationActionReference.action.canceled -= OnParticipantConformationReleased;
            
            _inputManager.avScaleInputActionReference.action.performed -= DisplayPointer;
            _inputManager.avScaleInputActionReference.action.canceled -= HidePointer;
        }
        #endregion

        #region [ Target Controls ]
        public void HideTarget() => _avScaleTargetVisualElement.style.display = DisplayStyle.None;
        public void DisplayTarget(Vector2 targetPosition)
        {
            _avScaleTargetVisualElement.style.translate = new Translate(targetPosition.x * 200, targetPosition.y * -200);
            _avScaleTargetVisualElement.style.display = DisplayStyle.Flex;
        }
        #endregion

        #region [ Input Logic - Conformation ]
        private void OnParticipantConformationPressed(InputAction.CallbackContext callbackContext) => _avScaleRingVisualElement.style.display = DisplayStyle.Flex;
        private void OnParticipantConformationReleased(InputAction.CallbackContext callbackContext) => _avScaleRingVisualElement.style.display = DisplayStyle.None;
        #endregion

        #region [ Input Logid - AV Inputs ]
        private void HidePointer(InputAction.CallbackContext callbackContext) => HidePointer();
        private void HidePointer() => _avScalePointerVisualElement.style.display = DisplayStyle.None;
        private void DisplayPointer(InputAction.CallbackContext callbackContext)
        {
            _avScalePointerVisualElement.style.display = DisplayStyle.Flex;
            Vector2 position = callbackContext.ReadValue<Vector2>();

            _avScalePointerVisualElement.style.translate = new Translate(position.x * 200, position.y * -200);
            Debug.Log($"Position: {position.ToString()}");
        }
        #endregion
        

        private void SetContent(InstructionUIContentScriptableObject instructionUIContent)
        {
            _contentContainerVisualElement.style.display = (string.IsNullOrEmpty(instructionUIContent.title) && string.IsNullOrEmpty(instructionUIContent.content)) ? DisplayStyle.None : DisplayStyle.Flex;
            _footerContainerVisualElement.style.display = (string.IsNullOrEmpty(instructionUIContent.footer)) ? DisplayStyle.None : DisplayStyle.Flex;
            
            _titleLabel.style.display = (string.IsNullOrEmpty(instructionUIContent.title)) ? DisplayStyle.None : DisplayStyle.Flex;
            _titleLabel.text = instructionUIContent.title.ToUpper();
            
            _contentLabel.style.display = (string.IsNullOrEmpty(instructionUIContent.content)) ? DisplayStyle.None : DisplayStyle.Flex;
            _contentLabel.text = instructionUIContent.content;
            
            _footerLabel.style.display = (string.IsNullOrEmpty(instructionUIContent.footer)) ? DisplayStyle.None : DisplayStyle.Flex;
            _footerLabel.text = instructionUIContent.footer;
        }
        
        public IEnumerator DisplayTextFieldCoroutine(InstructionUIContentScriptableObject instructionUIContent, IEnumerator enumerator)
        {
            SetContent(instructionUIContent);
        
            yield return DisplayCoroutine();
            yield return enumerator;
            yield return HideCoroutine();
        }
    }
}
