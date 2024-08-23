using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public partial class ExperimentUXML
{
    public CalibrationContainer calibrationContainer;
    [Serializable] public class CalibrationContainer : IContainer
   {
        #region [ Unserialised Fields ]
        private const string _calibrationContainerID = "CalibrationContainer";
        private const string _expressionID = "Expression";
        private const string _contentContainerID = "ContentContainer";
        private const string _footerContainerID = "FooterContainer";
        private const string _title = "TitleLabel";
        private const string _content = "ContentLabel";
        private const string _footer = "FooterLabel";
        
        private readonly VisualElement _expressionVisualElement;
        private readonly VisualElement _contentContainerVisualElement;
        private readonly VisualElement _footerContainerVisualElement;
        private readonly Label _titleLabel;
        private readonly Label _contentLabel;
        private readonly Label _footerLabel;

        private readonly InputManager _inputManager;
        #endregion

        public CalibrationContainer(ExperimentUXML experimentUxml, VisualElement root) : base(experimentUxml, root, _calibrationContainerID)
        {
            _expressionVisualElement = _containerVisualElement.Q<VisualElement>(_expressionID);
            _contentContainerVisualElement = _containerVisualElement.Q<VisualElement>(_contentContainerID);
            _footerContainerVisualElement = _containerVisualElement.Q<VisualElement>(_footerContainerID);
            
            _titleLabel = _contentContainerVisualElement.Q<Label>(_title);
            _contentLabel = _contentContainerVisualElement.Q<Label>(_content);
            _footerLabel = _footerContainerVisualElement.Q<Label>(_footer);
            
            _inputManager = FindObjectOfType<InputManager>();
        }
        
        private void SetTextContent(InstructionUIContentScriptableObject instructionUIContent)
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
        
        private void SetExpressionSprite(ExpressionDataScriptableObject expressionData)
        {
            _expressionVisualElement.style.backgroundImage = new StyleBackground(expressionData.expressionSprite);
        }
        
        public IEnumerator DisplayExpressionCoroutine(InstructionUIContentScriptableObject instructionUIContent, ExpressionDataScriptableObject expressionData, IEnumerator enumerator)
        {
            SetTextContent(instructionUIContent);
            SetExpressionSprite(expressionData);
        
            yield return DisplayCoroutine();
            yield return enumerator;
            yield return HideCoroutine();
        }
    }
}
