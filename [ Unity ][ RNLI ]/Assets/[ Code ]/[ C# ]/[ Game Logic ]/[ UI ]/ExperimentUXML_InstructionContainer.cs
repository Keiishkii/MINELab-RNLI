using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public partial class ExperimentUXML
{
    public InstructionContainer instructionContainer;
    [Serializable] public class InstructionContainer : IContainer
    {
        #region [ Unserialised Fields ]

        private const string _instructionsContainerID = "InstructionContainer";
        private const string _contentContainerID = "ContentContainer";
        private const string _footerContainerID = "FooterContainer";
        private const string _title = "TitleLabel";
        private const string _content = "ContentLabel";
        private const string _footer = "FooterLabel";

        private readonly VisualElement _contentContainerVisualElement;
        private readonly VisualElement _footerContainerVisualElement;
        private readonly Label _titleLabel;
        private readonly Label _contentLabel;
        private readonly Label _footerLabel;
        #endregion

        public InstructionContainer(ExperimentUXML experimentUxml, VisualElement root) : base(experimentUxml, root, _instructionsContainerID)
        {
            _contentContainerVisualElement = _containerVisualElement.Q<VisualElement>(_contentContainerID);
            _footerContainerVisualElement = _containerVisualElement.Q<VisualElement>(_footerContainerID);
            
            _titleLabel = _contentContainerVisualElement.Q<Label>(_title);
            _contentLabel = _contentContainerVisualElement.Q<Label>(_content);
            _footerLabel = _footerContainerVisualElement.Q<Label>(_footer);
        }

        public void SetContent(InstructionUIContentScriptableObject instructionUIContent)
        {
            _contentContainerVisualElement.style.display = (string.IsNullOrEmpty(instructionUIContent.title) && string.IsNullOrEmpty(instructionUIContent.content)) ? DisplayStyle.None : DisplayStyle.Flex;
            _footerContainerVisualElement.style.display = (string.IsNullOrEmpty(instructionUIContent.footer)) ? DisplayStyle.None : DisplayStyle.Flex;
            
            _titleLabel.text = instructionUIContent.title.ToUpper();
            _contentLabel.text = instructionUIContent.content;
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
