﻿using System;
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

        private readonly VisualElement _topProgressBarVisualElement;
        private readonly VisualElement _bottomProgressBarVisualElement;
        #endregion

        public TimerContainer(ExperimentUXML experimentUxml, VisualElement root) : base(experimentUxml, root, _instructionsContainerID)
        {
            _topProgressBarVisualElement = _containerVisualElement.Q<VisualElement>(_topProgressBarID);
            _bottomProgressBarVisualElement = _containerVisualElement.Q<VisualElement>(_bottomProgressBarID);
        }
        
        public IEnumerator StartTimer(float duration)
        {
            Display();
            for (float timeElapsed = 0; timeElapsed < duration; timeElapsed += Time.deltaTime)
            {
                float weight = Mathf.InverseLerp(0, duration, timeElapsed);
                float percentage = (1 - weight) * 100f;
                
                _topProgressBarVisualElement.style.right = new StyleLength(new Length(percentage, LengthUnit.Percent));
                _bottomProgressBarVisualElement.style.right = new StyleLength(new Length(percentage, LengthUnit.Percent));
                yield return null;
            }
            Hide();
        }
    }
}