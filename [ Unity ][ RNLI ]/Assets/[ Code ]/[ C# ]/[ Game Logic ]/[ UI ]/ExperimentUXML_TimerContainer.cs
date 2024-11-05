using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public partial class ExperimentUXML
{
    public TimerContainer timerContainer;
    [Serializable] public class TimerContainer : IContainer
    {
        #region [ Unserialised Fields ]
        private const string _instructionsContainerID = "TimerContainer";
        private const string _topProgressBarID = "TopProgressBar";
        private const string _bottomProgressBarID = "BottomProgressBar";
        private const string _bottomProgressBarContainerID = "BottomProgressBarContainer";

        private readonly VisualElement _topProgressBarVisualElement;
        private readonly VisualElement _bottomProgressBarVisualElement;
        private readonly VisualElement _bottomProgressBarContainerVisualElement;

        private IEnumerator _displayCoroutine;
        #endregion

        public TimerContainer(ExperimentUXML experimentUxml, VisualElement root) : base(experimentUxml, root, _instructionsContainerID)
        {
            _topProgressBarVisualElement = _containerVisualElement.Q<VisualElement>(_topProgressBarID);
            _bottomProgressBarVisualElement = _containerVisualElement.Q<VisualElement>(_bottomProgressBarID);
            _bottomProgressBarContainerVisualElement = _containerVisualElement.Q<VisualElement>(_bottomProgressBarContainerID);
        }
        
        public IEnumerator StartTimer(float duration, bool showBottomTimeBar)
        {
            _bottomProgressBarContainerVisualElement.style.display = showBottomTimeBar ? DisplayStyle.Flex : DisplayStyle.None;
            if (!ReferenceEquals(_displayCoroutine, null)) _experimentUxml.StopCoroutine(_displayCoroutine);
            _experimentUxml.StartCoroutine(_displayCoroutine = DisplayCoroutine());
            
            for (float timeElapsed = 0; timeElapsed < duration; timeElapsed += Time.deltaTime)
            {
                float weight = Mathf.InverseLerp(0, duration, timeElapsed);
                float percentage = (1 - weight) * 0.5f * 100f;
                
                _topProgressBarVisualElement.style.right = new StyleLength(new Length(percentage, LengthUnit.Percent));
                _topProgressBarVisualElement.style.left = new StyleLength(new Length(percentage, LengthUnit.Percent));

                if (showBottomTimeBar)
                {
                    _bottomProgressBarVisualElement.style.right = new StyleLength(new Length(percentage, LengthUnit.Percent));
                    _bottomProgressBarVisualElement.style.left = new StyleLength(new Length(percentage, LengthUnit.Percent));   
                }
                yield return null;
            }
            
            if (!ReferenceEquals(_displayCoroutine, null)) _experimentUxml.StopCoroutine(_displayCoroutine);
            _experimentUxml.StartCoroutine(_displayCoroutine = HideCoroutine());
        }
    }
}
