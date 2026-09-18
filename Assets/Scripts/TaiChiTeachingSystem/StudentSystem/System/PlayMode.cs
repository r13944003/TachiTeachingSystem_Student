using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mocopi.Receiver;
using UnityEditor;
using UnityEngine;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class PlayMode : MonoBehaviour
        {
            private float _FPS;

            [SerializeField] private AvatarManager _avatarManager;
            [SerializeField] private IndicatorManager _indicatorManager;
            [SerializeField] private FramePanelManager _framePanelManager;
            [SerializeField] private PlayPanelManager _playPanelManager;
            [SerializeField] private CoachManager _coachManager;

            private bool _onPlay = false;
            private bool _singleAvatarMode = true;
            private bool _indicatorActive = false;
            private int _frameID = 0;
            private int _modifyDataIndex;
            private MuscleValues[] _originData;
            private ModifyValues[] _modifyData;
            

            private MotionData _motionData;
            
            private float _fpsDeltaTime = 0f;
            private float _updateTimer = 0f;

            
            void Awake(){
                _avatarManager.InstantiatePlayModeAvatars();
                _indicatorManager.SetIndicatorsBodyPart();
            }

            //////////////////////////////////////////////////////
            ////////////   For StudentTaichiSystem  //////////////
            /////////////////////////////////////////////////////
            public void Enter(){
                _updateTimer = 0f;
                if(_onPlay)
                    SwitchOnPlay();
                _singleAvatarMode = true;

                // Indicator
                SetIndicatorActive();

                // frameID
                _frameID = 0;
                _framePanelManager.SetFrameNumberText(_frameID);

                //FPS
                SetFPS();

                //Avatars
                _avatarManager.EnterPlayMode(_singleAvatarMode, _playPanelManager.GetOriginPoseToggleIsOn());

                // UI
                _playPanelManager.SetPanelActive(true);
                _playPanelManager.SetPlayButtonInteractible(false);
                _playPanelManager.SetSingleAvatarPoseToggleInteractable(_singleAvatarMode);

                //Coach
                _coachManager.EnterPlayMode(_singleAvatarMode);
            
            }

            public void Run(){
                if(!_onPlay)
                    return;

                _updateTimer -= Time.deltaTime;
                if (_updateTimer > 0)
                    return;
                _updateTimer += _fpsDeltaTime;

                _framePanelManager.SetFrameNumberText(_frameID);

                _DoPlay();
                
            }
            public void Leave(){
                _playPanelManager.SetPanelActive(false);
                _indicatorManager.SetIndicatorActive(false, _singleAvatarMode, _playPanelManager.GetOriginPoseToggleIsOn());

            }



            private void _DoPlay()
            {
                // Restart coach avatar if user avatar restart 
                if(_frameID == 0){
                    _coachManager.RestartCoachMove();
                }
                // if this frame has modified Data
                if (_modifyDataIndex>=0 && _modifyData[_modifyDataIndex].frameID == _frameID){            
                    // Avatar Pose
                    _avatarManager.SetPlayModeAvatarPose(_originData[_frameID], _modifyData[_modifyDataIndex]);

                    // Indicator
                    if(_indicatorActive){
                        _indicatorManager.SetIndicatorTransform();                    
                    }
                    _indicatorManager.SetIndicatorMaterial(_modifyData[_modifyDataIndex].modifiedBodyParts);             
                    SwitchOnPlay();
                
                }
                else{
                    _avatarManager.SetPlayModeAvatarPose(_originData[_frameID], null);
                    if(_indicatorActive){
                        _indicatorManager.SetIndicatorTransform();
                        _indicatorManager.ResetIndicatorMaterial();
                    }
                    //next frame
                    _frameID = (_frameID + 1)%_originData.Length;
                }

            }

            ////////////////////////////////////////////////////////////
            //////////////   Set FPS(Speed of move)  //////////////////
            ///////////////////////////////////////////////////////////
                

            public void SetFPS(){
                _FPS = _framePanelManager.GetFPS();
                _fpsDeltaTime = 1/_FPS;
                if(_onPlay)
                    _coachManager.SetCoachSpeed();
            }


            /////////////////////////////////////////////////////////////////
            //////////////   Load motion data from server  //////////////////
            ////////////////////////////////////////////////////////////////
            IEnumerator _SetModifiedFileDropdown(){
                string accessToken = PlayerPrefs.GetString("AccessToken");
                int userId = PlayerPrefs.GetInt("UserId");
                yield return StartCoroutine(HttpService.Get_MotionDataByUserId(userId, accessToken));
                List<MotionRecord> motionRecordList = HttpService.GetMotionRecordList();
                _playPanelManager.SetModifiedDataFileDropdown(motionRecordList);

            }
            public void OpenLoadModifiedDataPanel(){
                _playPanelManager.SetLoadModifiedDataPanelActive(true);
                StartCoroutine(_SetModifiedFileDropdown());
            }
            IEnumerator _LoadModifiedData(){
                string filename = _playPanelManager.GetModifiedDataFilename();
                string accessToken = PlayerPrefs.GetString("AccessToken");
                yield return StartCoroutine(HttpService.Get_MotionDataLoad(filename, accessToken));
                _motionData = HttpService.GetMotionData();
                if(_motionData == null){
                    Debug.LogError("Load motion data failed");
                    yield break;
                }
                _originData = _motionData.motionFrames.ToArray();
                if(_motionData.modifiedFrames.Count == 0)
                    _modifyDataIndex = -1;
                else{
                    _motionData.modifiedFrames.Sort((x, y)=>x.frameID.CompareTo(y.frameID));
                    _modifyDataIndex = 0;
                }
                _modifyData = _motionData.modifiedFrames.ToArray();
                
                Debug.Log($"Motion frame count:{_originData.Length} modify frame count:{_modifyData.Length}");
                _frameID = 0;
                if(_originData.Length > 0){
                    _playPanelManager.SetPlayButtonInteractible(true);
                }
                else{
                    _playPanelManager.SetPlayButtonInteractible(false);
                }
            }

            public void LoadModifiedData(){
                StartCoroutine(_LoadModifiedData());
                _playPanelManager.SetLoadModifiedDataPanelActive(false);
                
            }

            /////////////////////////////////////////////////////////////////
            //////////////////    Play / Stop    ///////////////////////////
            ////////////////////////////////////////////////////////////////
            public void SwitchOnPlay(){
                _onPlay = !_onPlay;
                if(_onPlay){
                    if(_modifyDataIndex >= 0 && _modifyData[_modifyDataIndex].frameID == _frameID){
                        _modifyDataIndex += 1;
                        if(_modifyDataIndex >= _modifyData.Length)
                            _modifyDataIndex = 0;
                    }
                    _frameID = (_frameID + 1)%_originData.Length;
                }

                // Set Play Button
                _playPanelManager.SetPlayButton(_onPlay);
                
                // Set coach speed
                if(_onPlay)
                    _coachManager.Play();
                else
                    _coachManager.Stop();
                
            }

            ////////////////////////////////////////////////////////////////////////////////////////
            ////    Single Avatar / Duo Avatar                                             /////////
            ////    Single Avatar: Show one move at a time, original move or modified move /////////
            ////    Duo Avatar: Show both move                                             ////////
            ////////////////////////////////////////////////////////////////////////////////////////
            public void SwitchSingleAvatarMode(){
                _singleAvatarMode = !_singleAvatarMode;
                _avatarManager.EnterPlayMode(_singleAvatarMode, _playPanelManager.GetOriginPoseToggleIsOn());
                _coachManager.EnterPlayMode(_singleAvatarMode);
                _indicatorManager.SetIndicatorActive(_indicatorActive, _singleAvatarMode, _playPanelManager.GetOriginPoseToggleIsOn());
                _indicatorManager.SetIndicatorTransform();
                _playPanelManager.SetSingleAvatarModeButton(_singleAvatarMode);
                _playPanelManager.SetSingleAvatarPoseToggleInteractable(_singleAvatarMode);
            }
            
            ////////////////////////////////////////////////////////////////////////
            //////////////////    Show/Hide indicator    ///////////////////////////
            ///////////////////////////////////////////////////////////////////////
            public void SetIndicatorActive(){
                _indicatorActive = _playPanelManager.GetIndicatorToggleIsOn();
                _indicatorManager.SetIndicatorActive(_indicatorActive, _singleAvatarMode, _playPanelManager.GetOriginPoseToggleIsOn());
                _indicatorManager.SetIndicatorTransform();
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            ///////   Switch between original move and modified move in single avatar mode   //////////
            ///////////////////////////////////////////////////////////////////////////////////////////
            public void SetSingleAvatarPose(){
                _avatarManager.SetSingleAvatarPose( _playPanelManager.GetOriginPoseToggleIsOn());
                _indicatorManager.SetIndicatorActive(_indicatorActive, _singleAvatarMode, _playPanelManager.GetOriginPoseToggleIsOn());
            }

        }
    }
}