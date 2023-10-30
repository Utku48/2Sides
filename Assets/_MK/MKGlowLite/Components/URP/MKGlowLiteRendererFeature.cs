//////////////////////////////////////////////////////
// MK Glow URP Renderer Feature	    		        //
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MK.Glow.URP
{
    public class MKGlowLiteRendererFeature : ScriptableRendererFeature, MK.Glow.ISettings
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        // Renderer Feature Only Properties
        /////////////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR
        public bool showEditorMainBehavior = true;
		public bool showEditorBloomBehavior;
		public bool showEditorLensSurfaceBehavior;
        #endif

        public enum Workmode
        {
            [UnityEngine.InspectorName("Post-Process Volumes")]
            PostProcessVolumes = 0,
            [UnityEngine.InspectorName("Global")]
            Global = 1
        };
        public Workmode workmode = Workmode.PostProcessVolumes;

        ///Main
        public DebugView debugView = MK.Glow.DebugView.None;
        public Workflow workflow = MK.Glow.Workflow.Threshold;
        public LayerMask selectiveRenderLayerMask = -1;
        [Range(-1f, 1f)]
        public float anamorphicRatio = 0f;

        //Bloom
        [MK.Glow.MinMaxRange(0, 10)]
        public MinMaxRange bloomThreshold = new MinMaxRange(1.25f, 10f);
        [Range(1f, 10f)]
		public float bloomScattering = 7f;
		public float bloomIntensity = 1f;

        //LensSurface
        public bool allowLensSurface = false;
		public Texture2D lensSurfaceDirtTexture;
		public float lensSurfaceDirtIntensity = 2.5f;
		public Texture2D lensSurfaceDiffractionTexture;
		public float lensSurfaceDiffractionIntensity = 2.0f;

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
			return debugView;
		}
        public MK.Glow.Workflow GetWorkflow()
        { 
			return workflow;
		}
        public LayerMask GetSelectiveRenderLayerMask()
        { 
			return selectiveRenderLayerMask;
		}
        public float GetAnamorphicRatio()
        { 
			return anamorphicRatio;
		}

        //Bloom
		public MK.Glow.MinMaxRange GetBloomThreshold()
		{ 
			return bloomThreshold;
		}
		public float GetBloomScattering()
		{ 
			return bloomScattering;
		}
		public float GetBloomIntensity()
		{ 
			return bloomIntensity;
		}

        //LensSurface
		public bool GetAllowLensSurface()
		{ 
			return allowLensSurface;
		}
		public Texture2D GetLensSurfaceDirtTexture()
		{ 
			return lensSurfaceDirtTexture;
		}
		public float GetLensSurfaceDirtIntensity()
		{ 
			return lensSurfaceDirtIntensity;
		}
		public Texture2D GetLensSurfaceDiffractionTexture()
		{ 
			return lensSurfaceDiffractionTexture;
		}
		public float GetLensSurfaceDiffractionIntensity()
		{ 
			return lensSurfaceDiffractionIntensity;
		}

        class MKGlowLiteRenderPass : ScriptableRenderPass, MK.Glow.ICameraData
        {
            private MK.Glow.URP.MKGlowLite _mKGlowVolumeComponent;
            private MK.Glow.URP.MKGlowLite mKGlowVolumeComponent
            {
                get
                {
                    _mKGlowVolumeComponent = VolumeManager.instance.stack.GetComponent<MK.Glow.URP.MKGlowLite>();
                    return _mKGlowVolumeComponent;
                }
            }

            internal Effect effect = new Effect();
            internal ScriptableRenderer scriptableRenderer;
            internal MK.Glow.ISettings settingsRendererFeature = null;
            internal Workmode workmode = Workmode.PostProcessVolumes;
            private RenderTarget sourceRenderTarget, destinationRenderTarget;
            private RenderingData _renderingData;
            private MK.Glow.ISettings _settingsPostProcessingVolume = null;
            private MK.Glow.ISettings _activeSettings = null;
            private RenderTextureDescriptor _sourceDescriptor;
            private readonly int _rendererBufferID = Shader.PropertyToID("_MKGlowLiteScriptableRendererOutput");
            private readonly string _profilerName = "MKGlowLite";
            private Material _copyMaterial;
            private readonly Shader _copyShader = Shader.Find("Hidden/MK/Glow/URP/Copy");
            private readonly int _mainTexID = Shader.PropertyToID("_MainTex");

            public MKGlowLiteRenderPass()
            {
                this.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
                this._copyMaterial = new Material(_copyShader) { hideFlags = HideFlags.HideAndDontSave };
            }

            ~MKGlowLiteRenderPass()
            {
                DestroyImmediate(this._copyMaterial);
            }

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                _sourceDescriptor = cameraTextureDescriptor;
            }

            #if UNITY_2021_3_OR_NEWER
            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData) 
            {
                base.OnCameraSetup(cmd, ref renderingData);
                #if UNITY_2022_1_OR_NEWER
                    sourceRenderTarget.renderTargetIdentifier = scriptableRenderer.cameraColorTargetHandle;
                #else
                    sourceRenderTarget.renderTargetIdentifier = scriptableRenderer.cameraColorTarget;
                #endif
            }
            #endif

            private static Mesh _fullScreenMesh;
            public static Mesh fullscreenMesh
            {
                get
                {
                    if (_fullScreenMesh != null)
                        return _fullScreenMesh;

                    float topV = 1.0f;
                    float bottomV = 0.0f;

                    _fullScreenMesh = new Mesh { name = "Fullscreen Quad" };
                    _fullScreenMesh.SetVertices(new List<Vector3>
                    {
                        new Vector3(-1.0f, -1.0f, 0.0f),
                        new Vector3(-1.0f,  1.0f, 0.0f),
                        new Vector3(1.0f, -1.0f, 0.0f),
                        new Vector3(1.0f,  1.0f, 0.0f)
                    });

                    _fullScreenMesh.SetUVs(0, new List<Vector2>
                    {
                        new Vector2(0.0f, bottomV),
                        new Vector2(0.0f, topV),
                        new Vector2(1.0f, bottomV),
                        new Vector2(1.0f, topV)
                    });

                    _fullScreenMesh.SetIndices(new[] { 0, 1, 2, 2, 1, 3 }, MeshTopology.Triangles, 0, false);
                    _fullScreenMesh.UploadMeshData(true);
                    return _fullScreenMesh;
                }
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if(workmode == Workmode.PostProcessVolumes)
                {
                    if(mKGlowVolumeComponent == null || renderingData.cameraData.camera == null)
                        return;
                    
                    if(!mKGlowVolumeComponent.IsActive())
                        return;
                    }
                else
                {
                    if(renderingData.cameraData.camera == null)
                        return;
                }

                _settingsPostProcessingVolume = mKGlowVolumeComponent;
                _activeSettings = workmode == Workmode.PostProcessVolumes ? _settingsPostProcessingVolume : settingsRendererFeature;

                CommandBuffer cmd = CommandBufferPool.Get(_profilerName);
                _renderingData = renderingData;

                #if !UNITY_2021_3_OR_NEWER
                    sourceRenderTarget.renderTargetIdentifier = scriptableRenderer.cameraColorTarget;
                #endif
                
                destinationRenderTarget.identifier = _rendererBufferID;

                #if UNITY_2018_2_OR_NEWER
                destinationRenderTarget.renderTargetIdentifier = new RenderTargetIdentifier(destinationRenderTarget.identifier, 0, CubemapFace.Unknown, -1);
                #else
                destinationRenderTarget.renderTargetIdentifier = new RenderTargetIdentifier(destinationRenderTarget.identifier);
                #endif

                cmd.GetTemporaryRT(destinationRenderTarget.identifier, _sourceDescriptor, FilterMode.Bilinear);
                cmd.SetGlobalTexture(_mainTexID, sourceRenderTarget.renderTargetIdentifier);
                cmd.SetRenderTarget(destinationRenderTarget.renderTargetIdentifier, 0, CubemapFace.Unknown, -1);
                cmd.DrawMesh(fullscreenMesh, Matrix4x4.identity, _copyMaterial, 0, 0);
                effect.Build(destinationRenderTarget, sourceRenderTarget, _activeSettings, cmd, this, renderingData.cameraData.camera);
                cmd.ReleaseTemporaryRT(destinationRenderTarget.identifier);

                context.ExecuteCommandBuffer(cmd);

                cmd.Clear();
                CommandBufferPool.Release(cmd);
            }

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Camera Data
            /////////////////////////////////////////////////////////////////////////////////////////////
            public int GetCameraWidth()
            {
                return _renderingData.cameraData.cameraTargetDescriptor.width;
            }
            public int GetCameraHeight()
            {
                return _renderingData.cameraData.cameraTargetDescriptor.height;
            }
            public bool GetStereoEnabled()
            {
                #if UNITY_2020_2_OR_NEWER
                    return _renderingData.cameraData.xrRendering;
                #else
                    return _renderingData.cameraData.isStereoEnabled;
                #endif
            }
            public float GetAspect()
            {
                return _renderingData.cameraData.camera.aspect;
            }
            public Matrix4x4 GetWorldToCameraMatrix()
            {
                return _renderingData.cameraData.camera.worldToCameraMatrix;
            }
            public bool GetOverwriteDescriptor()
            {
                return false;
            }
            public UnityEngine.Rendering.TextureDimension GetOverwriteDimension()
            {
                return UnityEngine.Rendering.TextureDimension.Tex2D;
            }
            public int GetOverwriteVolumeDepth()
            {
                return 1;
            }
            public bool GetTargetTexture()
            {
                return _renderingData.cameraData.camera.targetTexture != null ? true : false;
            }
        }

        private MKGlowLiteRenderPass _mkGlowRenderPass;
        private readonly string _componentName = "MKGlowLite";

        public override void Create()
        {
            _mkGlowRenderPass = new MKGlowLiteRenderPass();
            _mkGlowRenderPass.effect.Enable(RenderPipeline.SRP);
            _mkGlowRenderPass.settingsRendererFeature = this;
        }

        private void OnDisable()
        {
            _mkGlowRenderPass.effect.Disable();
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            name = _componentName;

            _mkGlowRenderPass.workmode = workmode;
            _mkGlowRenderPass.ConfigureInput(ScriptableRenderPassInput.Color);

            if(workmode == Workmode.Global)
            {
                _mkGlowRenderPass.scriptableRenderer = renderer;
                renderer.EnqueuePass(_mkGlowRenderPass);
            }
            else
            {
                if(renderingData.cameraData.postProcessEnabled)
                {
                    _mkGlowRenderPass.scriptableRenderer = renderer;
                    renderer.EnqueuePass(_mkGlowRenderPass);
                }
            }
        }
    }
}


