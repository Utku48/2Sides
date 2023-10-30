//////////////////////////////////////////////////////
// MK Glow URP Volume Component     	            //
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MK.Glow.URP
{
    [ExecuteInEditMode, VolumeComponentMenu("Post-processing/MK/MKGlowLite")]
    public class MKGlowLite : VolumeComponent, IPostProcessComponent, MK.Glow.ISettings
    {
        [System.Serializable]
        public sealed class Texture2DParameter : VolumeParameter<Texture2D>
        {
            public override void Interp(Texture2D from, Texture2D to, float t)
            {
                value = t > 0 ? to : from;
            }
        }

        [System.Serializable]
        public sealed class DebugViewParameter : VolumeParameter<MK.Glow.DebugView>
        {
            public override void Interp(MK.Glow.DebugView from, MK.Glow.DebugView to, float t)
            {
                value = t > 0 ? to : from;
            }
        }

        [System.Serializable]
        public sealed class WorkflowParameter : VolumeParameter<MK.Glow.Workflow>
        {
            public override void Interp(MK.Glow.Workflow from, MK.Glow.Workflow to, float t)
            {
                value = t > 0 ? to : from;
            }
        }

        [System.Serializable]
        public sealed class LayerMaskParameter : VolumeParameter<LayerMask>
        {
            public override void Interp(LayerMask from, LayerMask to, float t)
            {
                value = t > 0 ? to : from;
            }
        }

        [System.Serializable]
        public sealed class MinMaxRangeParameter : VolumeParameter<MK.Glow.MinMaxRange>
        {
            public override void Interp(MK.Glow.MinMaxRange from, MK.Glow.MinMaxRange to, float t)
            {
                m_Value.minValue = Mathf.Lerp(from.minValue, to.minValue, t);
                m_Value.maxValue = Mathf.Lerp(from.maxValue, to.maxValue, t);
            }
        }

        #if UNITY_EDITOR
        public BoolParameter showEditorMainBehavior = new BoolParameter(true);
		public BoolParameter showEditorBloomBehavior = new BoolParameter(false);
		public BoolParameter showEditorLensSurfaceBehavior = new BoolParameter(false);
        /// <summary>
        /// Keep this value always untouched, editor internal only
        /// </summary>
        public BoolParameter isInitialized = new BoolParameter(false, true);
        #endif
        
        //Main
        public DebugViewParameter debugView = new DebugViewParameter() { value = MK.Glow.DebugView.None };
        public WorkflowParameter workflow = new WorkflowParameter() { value = MK.Glow.Workflow.Threshold };
        public LayerMaskParameter selectiveRenderLayerMask = new LayerMaskParameter() { value = -1 };
        [Range(-1f, 1f)]
        public ClampedFloatParameter anamorphicRatio = new ClampedFloatParameter(0, -1, 1);

        //Bloom
        [MK.Glow.MinMaxRange(0, 10)]
        public MinMaxRangeParameter bloomThreshold = new MinMaxRangeParameter() { value = new MinMaxRange(1.25f, 10f) };
        [Range(1f, 10f)]
		public ClampedFloatParameter bloomScattering = new ClampedFloatParameter(7, 1, 10);
		public FloatParameter bloomIntensity = new FloatParameter(0);

        //LensSurface
        public BoolParameter allowLensSurface = new BoolParameter(false, true);
		public Texture2DParameter lensSurfaceDirtTexture = new Texture2DParameter();
		public FloatParameter lensSurfaceDirtIntensity = new FloatParameter(0);
		public Texture2DParameter lensSurfaceDiffractionTexture = new Texture2DParameter();
		public FloatParameter lensSurfaceDiffractionIntensity = new FloatParameter(0);
        
        public bool IsActive() => Compatibility.IsSupported && active && (bloomIntensity.value > 0);

        public bool IsTileCompatible() => true;

        /////////////////////////////////////////////////////////////////////////////////////////////
        // Settings
        /////////////////////////////////////////////////////////////////////////////////////////////
        public bool GetAllowGeometryShaders()
        { 
            return false;
        }
        public bool GetAllowComputeShaders()
        { 
            return false;
        }
        public MK.Glow.DebugView GetDebugView()
        { 
			return debugView.value;
		}
        public MK.Glow.Workflow GetWorkflow()
        { 
			return workflow.value;
		}
        public LayerMask GetSelectiveRenderLayerMask()
        { 
			return selectiveRenderLayerMask.value;
		}
        public float GetAnamorphicRatio()
        { 
			return anamorphicRatio.value;
		}

        //Bloom
		public MK.Glow.MinMaxRange GetBloomThreshold()
		{ 
			return bloomThreshold.value;
		}
		public float GetBloomScattering()
		{ 
			return bloomScattering.value;
		}
		public float GetBloomIntensity()
		{ 
			return bloomIntensity.value;
		}

        //LensSurface
		public bool GetAllowLensSurface()
		{ 
			return allowLensSurface.value;
		}
		public Texture2D GetLensSurfaceDirtTexture()
		{ 
			return lensSurfaceDirtTexture.value;
		}
		public float GetLensSurfaceDirtIntensity()
		{ 
			return lensSurfaceDirtIntensity.value;
		}
		public Texture2D GetLensSurfaceDiffractionTexture()
		{ 
			return lensSurfaceDiffractionTexture.value;
		}
		public float GetLensSurfaceDiffractionIntensity()
		{ 
			return lensSurfaceDiffractionIntensity.value;
		}
    }
}
