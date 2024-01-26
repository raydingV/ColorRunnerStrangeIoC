using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Views.Player
{
    public class CollectStack : View
    {
        private readonly string _player = "Player";

        [SerializeField] private CapsuleCollider _collider;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_player))
            {
                StackObject();
            }
        }

        private void StackObject()
        {
            _collider.enabled = false;
        }
    }
}

