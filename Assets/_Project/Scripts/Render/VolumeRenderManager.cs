using System;
using UnityEngine;
using Volumes;

namespace Render
{
    public class VolumeRenderManager : MonoBehaviour
    {
        [SerializeField] private VolumeRender volumePrefab;
        
        private VolumeRender volumeRender;

        public VolumeRender VolumeRender => volumeRender;

        public event Action<VolumeRender> OnVolumeLoaded = render => { }; 

        public void LoadVolume(RuntimeVolume volume)
        {
            if (volumeRender)
            {
                Destroy(volumeRender.Material.GetTexture("_Volume"));
                Destroy(volumeRender.gameObject);
            }
        
            volumeRender = Instantiate(volumePrefab);
            volumeRender.Material.SetTexture("_Volume", volume.Texture);
            volumeRender.Material.SetFloat("_Alpha", 0.1f);
            volumeRender.Material.SetFloat("_AlphaThreshold", 0.999f);
            volumeRender.Material.SetFloat("_StepDistance", 0.0002f);
            volumeRender.Material.SetInt("_MaxStepThreshold", 2048);
            OnVolumeLoaded(volumeRender);
        }

        public void Cut(Vector3 position, Vector3 normal)
        {
            var localPos = volumeRender.transform.InverseTransformPoint(position);
            var localPosition = volumeRender.transform.InverseTransformVector(normal);
            
            volumeRender.Material.SetVector("_CutOrigin", localPos);
            volumeRender.Material.SetVector("_CutNormal", localPosition);
        }
    }
}