using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Intro
{
    public class CameraListener : MonoBehaviour
    {
        [SerializeField] Camera _camera;
        [SerializeField] RawImage _rawImage;

        private void Start()
        {
            _rawImage.texture = _camera.targetTexture;
        }


    }
}
