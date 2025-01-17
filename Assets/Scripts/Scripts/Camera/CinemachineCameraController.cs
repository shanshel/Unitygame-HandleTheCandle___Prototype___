﻿using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using Cinemachine;

namespace MoreMountains.TopDownEngine
{
    public enum MMCameraEventTypes { SetTargetCharacter, SetConfiner, StartFollowing, StopFollowing }

    public struct MMCameraEvent
    {
        public MMCameraEventTypes EventType;
        public GameObject TargetCharacter;
        public Collider Bounds;

        public MMCameraEvent(MMCameraEventTypes eventType, GameObject targetCharacter = null, Collider bounds = null)
        {
            EventType = eventType;
            TargetCharacter = targetCharacter;
            Bounds = bounds;
        }

        static MMCameraEvent e;
        public static void Trigger(MMCameraEventTypes eventType, GameObject targetCharacter = null, Collider bounds = null)
        {
            e.EventType = eventType;
            e.Bounds = bounds;
            e.TargetCharacter = targetCharacter;
            MMEventManager.TriggerEvent(e);
        }
    }

    /// <summary>
    /// A class that handles camera follow for Cinemachine powered cameras
    /// </summary>
    public class CinemachineCameraController : MonoBehaviour, MMEventListener<MMCameraEvent>
    {
        /// True if the camera should follow the player
        public bool FollowsPlayer { get; set; }

        public bool FollowsAPlayer = true;
        public bool ConfineCameraToLevelBounds = true;
        [ReadOnly]
        public GameObject TargetCharacter;

        protected CinemachineVirtualCamera _virtualCamera;
        protected CinemachineConfiner _confiner;

        /// <summary>
        /// On Awake we grab our components
        /// </summary>
        protected virtual void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _confiner = GetComponent<CinemachineConfiner>();
        }

        /// <summary>
        /// On Start we assign our bounding volume
        /// </summary>
        protected virtual void Start()
        {
         
        }

        public virtual void SetTarget(GameObject character)
        {
            TargetCharacter = character;
        }

        /// <summary>
        /// Starts following the LevelManager's main player
        /// </summary>
        public virtual void StartFollowing()
        {
            if (!FollowsAPlayer) { return; }
            FollowsPlayer = true;
           // _virtualCamera.Follow = TargetCharacter.CameraTarget.transform;
        }

        /// <summary>
        /// Stops following any target
        /// </summary>
        public virtual void StopFollowing()
        {
            if (!FollowsAPlayer) { return; }
            FollowsPlayer = false;
            _virtualCamera.Follow = null;
        }

        public virtual void OnMMEvent(MMCameraEvent cameraEvent)
        {
            switch (cameraEvent.EventType)
            {
                case MMCameraEventTypes.SetTargetCharacter:
                    SetTarget(cameraEvent.TargetCharacter);
                    break;
                case MMCameraEventTypes.SetConfiner:                    
                    if (_confiner != null)
                    {
                        _confiner.m_BoundingVolume = cameraEvent.Bounds;
                    }
                    break;
                case MMCameraEventTypes.StartFollowing:
                    if (cameraEvent.TargetCharacter != null)
                    {
                        if (cameraEvent.TargetCharacter != TargetCharacter)
                        {
                            return;
                        }
                    }
                    StartFollowing();
                    break;

                case MMCameraEventTypes.StopFollowing:
                    if (cameraEvent.TargetCharacter != null)
                    {
                        if (cameraEvent.TargetCharacter != TargetCharacter)
                        {
                            return;
                        }
                    }
                    StopFollowing();
                    break;
            }
        }

        protected virtual void OnEnable()
        {
            this.MMEventStartListening<MMCameraEvent>();
        }

        protected virtual void OnDisable()
        {
            this.MMEventStopListening<MMCameraEvent>();
        }
    }
}
