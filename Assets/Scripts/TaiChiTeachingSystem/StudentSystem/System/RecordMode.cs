using System;
using System.Collections;
using System.Collections.Generic;
using Mocopi.Receiver;
using UnityEditor;
using UnityEngine;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class RecordMode : MonoBehaviour
        {
            private float _FPS = 30;

            [SerializeField] private AvatarManager _avatarManager;
            [SerializeField] private FramePanelManager _framePanelManager;
            [SerializeField] private RecordPanelManager _recordPanelManager;
            [SerializeField] private StudentPanelManager _studentPanelManager;
            [SerializeField] private CoachManager _coachManager;
            [SerializeField] private MocopiSimpleReceiver _mocopiSimpleReceiver;

            private bool _onRecording = false;
            private bool _inProcess = false;
            private int _frameID;
            private int _studentAvatarMode;

            private MotionData _motionData;
            
            private float _fpsDeltaTime = 0f;
            private float _updateTimer = 0f;


            void Awake(){
                _avatarManager.InstantiateRecordModeAvatars();

                // Set the first record avatar as the mocopi receiver
                GameObject avatar = _avatarManager.GetFirstRecordAvatar();
                avatar.AddComponent<MocopiAvatar>();
                _mocopiSimpleReceiver.AvatarSettings[0].MocopiAvatar = avatar.GetComponent<MocopiAvatar>();
                _mocopiSimpleReceiver.enabled = true;
            }
            //////////////////////////////////////////////////////
            ////////////   For StudentTaichiSystem  //////////////
            /////////////////////////////////////////////////////
            
            public void Enter(){
                _onRecording = false;
                _frameID = 0;
                SetFPS();

                // Avatars
                _avatarManager.EnterRecordMode();
                // UI Panels
                _recordPanelManager.SetPanelActive(true);
                //Coach
                _coachManager.EnterRecordMode();
                //Set default StudentAvatarMode to "Eight Avatar"
                _studentAvatarMode = 2;
                _studentPanelManager.SetAvatarModeButtonInteractable(true);

            }
            public void Run(){
                // Do nothing if in no student avatar mode
                if(_studentAvatarMode == 0)
                    return;

                // Make avatar follow helmet if in single student avatar mode
                if(_studentAvatarMode == 1)
                    _avatarManager.MakeRecordAvatarFollowCamera();

                // Copy avatar pose to other 7 non-mocopi avatars if in eight student avatar mode
                if(_studentAvatarMode == 2)
                    _avatarManager.SetRecordAvatarsPose();

                if(OVRInput.GetDown(OVRInput.Button.One)){
                    ChangeRecordingState();
                }
                if(!_onRecording)
                    return;

                _updateTimer -= Time.deltaTime;
                if (_updateTimer > 0)
                    return;
                _updateTimer += _fpsDeltaTime;

                _framePanelManager.SetFrameNumberText(_frameID);

                _DoRecording();
            }
            public void Leave(){
                _recordPanelManager.SetPanelActive(false);
                //StudentAvatarMode
                _studentPanelManager.SetAvatarModeButtonInteractable(false);

            }

            private void _DoRecording()
            {
                // muscle value
                MuscleValues tmpValue = new MuscleValues();
                _avatarManager.GetRecordMuscleValue(ref tmpValue);

                // add a frames
                _motionData.motionFrames.Add(tmpValue);

                // update recordFrameIndex
                _frameID += 1;
            }

            ////////////////////////////////////////////////////////////
            //////////////   Set FPS(Speed of move)  //////////////////
            ///////////////////////////////////////////////////////////
            public void SetFPS(){
                _FPS = _framePanelManager.GetFPS();
                _fpsDeltaTime = 1/_FPS;
                _coachManager.SetCoachSpeed();
            }


            ////////////////////////////////////////////////////////////////////////
            //////////////   Set number of student Avatar(8, 1, 0)  ////////////////
            ////////////////////////////////////////////////////////////////////////            
            public void SetStudentAvatarMode(int p_modeChange){
                _studentAvatarMode = (_studentAvatarMode + p_modeChange + 3)%3;
                _studentPanelManager.SetAvatarModeText(_studentAvatarMode);
                _avatarManager.SetRecordAvatarNumber(_studentAvatarMode);
                _coachManager.SetCoachAvatarsPosition(_studentAvatarMode);
            }


            //////////////////////////////////////////////////////
            ////////////////   Record Button  //////////////////
            /////////////////////////////////////////////////////
            public void ChangeRecordingState(){

                // Start Recording
                if(!_onRecording && !_inProcess){
                    _inProcess=true;
                    StartCoroutine(_StartRecording());
                }
                //Stop Recording
                else if(!_inProcess){
                    _recordPanelManager.SetRecordDataBufferActive(true, _motionData.motionFrames.Count);
                    _framePanelManager.SetFpsSliderInteractable(true);
                    _onRecording = !_onRecording;
                    _recordPanelManager.SetRecordingButton(_onRecording);
                }
                
            }
            IEnumerator _StartRecording(){
                _motionData = new MotionData
                {
                    motionFrames = new List<MuscleValues>(),
                    modifiedFrames = new List<ModifyValues>(),
                };
                _updateTimer = 0;
                _frameID = 0;
                _coachManager.RestartCoachMove();
                _coachManager.SetCoachSpeed(0);
                _recordPanelManager.SetRecordDataBufferActive(false, 0);
                _framePanelManager.SetFpsSliderInteractable(false);

                yield return StartCoroutine(_StartRecordingCountdown());
                _coachManager.SetCoachSpeed();
                _recordPanelManager.SetRecordingButton(_onRecording);
            }
            IEnumerator _StartRecordingCountdown(){
                _recordPanelManager.SetRecordCountdownPanelActive(true);

                _recordPanelManager.SetRecordCountdownText(3);
                yield return new WaitForSeconds(1);
                _recordPanelManager.SetRecordCountdownText(2);
                yield return new WaitForSeconds(1);
                _recordPanelManager.SetRecordCountdownText(1);
                yield return new WaitForSeconds(1);
                _recordPanelManager.SetRecordCountdownText(0);
                yield return new WaitForSeconds(1);

                _recordPanelManager.SetRecordCountdownPanelActive(false);
                _onRecording = true;
                _inProcess = false;

            }

            //////////////////////////////////////////////////////
            ////////////////   Upload Button  //////////////////
            /////////////////////////////////////////////////////

            IEnumerator _SetCoachIdDropdown(){
                string accessToken = PlayerPrefs.GetString("AccessToken");
                yield return StartCoroutine(HttpService.Get_CoachAll(accessToken));

                List<CoachData> coachDataList = HttpService.GetCoachDataList();
                if(coachDataList != null)
                    _recordPanelManager.SetCoachIdDropdown(coachDataList);
                else
                    Debug.LogError("Get CoachData failed");
            }
            public void OpenUploadRecordDataPanel(){
                _recordPanelManager.SetUploadRecordDataPanelActive(true);
                StartCoroutine(_SetCoachIdDropdown());
            }
            IEnumerator _UploadHandler(MotionData p_motionData, string p_accessToken){
                yield return StartCoroutine(HttpService.Post_MotionDataNew(p_motionData, p_accessToken));
                bool uploadSuccess = HttpService.GetUploadSuccess();
                _recordPanelManager.SetUploadRecordDataPanelActive(false);
                _recordPanelManager.SetUploadResultPanelActive(true);
                _recordPanelManager.SetUploadResultText(uploadSuccess);
            }
            public void UploadRecordData(){
                string accessToken = PlayerPrefs.GetString("AccessToken");
                _motionData.coachId = _recordPanelManager.GetCoachId();
                _motionData.userId = PlayerPrefs.GetInt("UserId");
                // Change float precision for file size
                for(int i=0 ; i<_motionData.motionFrames.Count ; i++){
                    for(int j=0 ; j<_motionData.motionFrames[i].muscleValues.Length ; j++){
                        float value = _motionData.motionFrames[i].muscleValues[j];
                        value = Mathf.Floor(value * 10000)/10000;
                        _motionData.motionFrames[i].muscleValues[j] = value;
                    }
                }
                StartCoroutine(_UploadHandler(_motionData, accessToken));
            }
        }
    }
}
