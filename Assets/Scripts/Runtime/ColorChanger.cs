using System;
using Sirenix.OdinInspector;
using strange.extensions.mediation.impl;
using UnityEngine;
using ValueObject;

namespace UnityObject
{
    public class ColorChanger : Mediator
    {
        [ShowInInspector] private int Tag;

        [ShowInInspector] private Material _material;

        [ShowInInspector] private CD_ColorData _colorData;

        private ColorData FirstColorData
        {
            get
            {
                if (_colorData != null && _colorData.Colors.Count > Tag)
                {
                    return _colorData.Colors[Tag];
                }
                return null;
            }
        }

        public void ChangeColor()
        {
            if (FirstColorData != null && _material != null)
            {
                Color originalColor = FirstColorData.Color;
                _material.color = originalColor;
            }
        }
    }
}