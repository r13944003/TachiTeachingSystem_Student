using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class AvatarManager : MonoBehaviour
        {
            [Header("===Avatar Options")]   // avatar造型選擇
            [SerializeField] private GameObject[] _avatarOptions;

            [Header("===Record Mode Avatar===")]
            [SerializeField] private GameObject _recordAvatars;
            [SerializeField] private Transform[] _recordPos;
            [SerializeField] private GameObject _recordNameTag;
            private List<Animator> _recordAnimators;
            private List<HumanPoseHandler> _recordPoseHandlers;
            private HumanPose _recordPose;
            
            
            // This part is for single student(user) avatar mode that make the avatar follows the camera(MR helmet)
            [SerializeField] private Transform _cameraTransform;
            public float radius = 4.03f;
            public float movementDuration = 0.8f;
            public float rightOffset = 0.135f;
            public float directionThresh = 0.1f;
            private float _height = 0f;
            
            private Vector3 _targetPosition;
            private Vector3 _prevDirection;
            private Vector3 _currentVelocity = Vector3.zero;

            // End of this part
            
            


            [Header("===Play Mode: Origin Avatar===")]
            [SerializeField] private GameObject _play_originAvatars;
            [SerializeField] private Transform[] _play_originPos;
            [SerializeField] private GameObject _play_originNameTag;
            private List<Animator> _play_originAnimators;
            private List<HumanPoseHandler> _play_originPoseHandlers;
            private HumanPose _play_originPose;

            [Header("===Play Mode: Modify Avatar===")]
            [SerializeField] private GameObject _play_modifyAvatars;
            [SerializeField] private Transform[] _play_modifyPos;
            [SerializeField] private GameObject _play_modifyNameTag;
            private List<Animator> _play_modifyAnimators;
            private List<HumanPoseHandler> _play_modifyPoseHandlers;
            private HumanPose _play_modifyPose;

            /////////////////////////////////////////////////////////////////
            /////////////////////   Common   ////////////////////////////////
            /////////////////////////////////////////////////////////////////
            private void _SetAvatarsPosition(GameObject p_avatars, float p_shiftX1, float p_shiftX2, float p_shiftX3){
                // Set front and back avatar
                Vector3 pos = p_avatars.transform.GetChild(0).localPosition;
                p_avatars.transform.GetChild(0).localPosition = new Vector3(p_shiftX1, pos.y, pos.z);
                pos = p_avatars.transform.GetChild(3).localPosition;
                p_avatars.transform.GetChild(3).localPosition = new Vector3(p_shiftX1, pos.y, pos.z);

                // Set left and right avatar
                pos = p_avatars.transform.GetChild(1).localPosition;
                p_avatars.transform.GetChild(1).localPosition = new Vector3(-p_shiftX3, pos.y, pos.z);
                pos = p_avatars.transform.GetChild(2).localPosition;
                p_avatars.transform.GetChild(2).localPosition = new Vector3(p_shiftX3, pos.y, pos.z);

                // Set frontLeft and backLeft avatar
                pos = p_avatars.transform.GetChild(4).localPosition;
                p_avatars.transform.GetChild(4).localPosition = new Vector3(-p_shiftX2, pos.y, pos.z);
                pos = p_avatars.transform.GetChild(6).localPosition;
                p_avatars.transform.GetChild(6).localPosition = new Vector3(-p_shiftX2, pos.y, pos.z);

                // Set frontRight and backRight avatar
                pos = p_avatars.transform.GetChild(5).localPosition;
                p_avatars.transform.GetChild(5).localPosition = new Vector3(p_shiftX2, pos.y, pos.z);
                pos = p_avatars.transform.GetChild(7).localPosition;
                p_avatars.transform.GetChild(7).localPosition = new Vector3(p_shiftX2, pos.y, pos.z);
            }
            
            /////////////////////////////////////////////////////////////////
            ////////////   Record Mode Avatars   ////////////////////////////////
            /////////////////////////////////////////////////////////////////
            
            // Create student(user) avatar  in runtime
            public void InstantiateRecordModeAvatars(){
                int avatarId = PlayerPrefs.GetInt("AvatarId");
                // Record Avatars
                for(int i=0 ; i<8 ; i++){
                    GameObject newAvatar = Instantiate(_avatarOptions[avatarId]);
                    newAvatar.SetActive(true);
                    newAvatar.transform.SetParent(_recordAvatars.transform);
                    newAvatar.transform.position = _recordPos[i].position;
                    newAvatar.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    // Add Name Tag
                    GameObject nameTag = Instantiate(_recordNameTag);
                    nameTag.transform.SetParent(newAvatar.transform.GetChild(0).GetChild(0).GetChild(0));
                    nameTag.transform.localPosition = new Vector3(0, 1.0f, 0);
                }
            }
            
            // For getting the front student avatar in record mode (since mocopi data is sent to the front avatar)
            public GameObject GetFirstRecordAvatar(){
                return _recordAvatars.transform.GetChild(0).GetChild(0).gameObject;
            }

            // Initialize animators and poseHandlers for each student avatar
            private void _PrepareRecordAvatars(){
                // For record Avatars
                _recordAnimators = new List<Animator>();
                _recordPoseHandlers = new List<HumanPoseHandler>();
                for(int ID = 0 ; ID< _recordAvatars.transform.childCount ; ID++){
                    _recordAnimators.Add(_recordAvatars.transform.GetChild(ID).GetChild(0).GetComponent<Animator>());
                    _recordPoseHandlers.Add(new HumanPoseHandler(_recordAnimators[ID].avatar, _recordAnimators[ID].transform));
                }
                _recordPose = new HumanPose();
            }
                
            public void GetRecordMuscleValue(ref MuscleValues tmpValue){
                tmpValue.muscleValues = new float[_recordPose.muscles.Length];
                _recordPoseHandlers[0].GetHumanPose(ref _recordPose);
                for (int i = 0; i < _recordPose.muscles.Length; ++i)
                    tmpValue.muscleValues[i] = _recordPose.muscles[i];

                // position & rotation
                tmpValue.position = _recordPose.bodyPosition;
                tmpValue.rotation = _recordPose.bodyRotation;
            }
            
            // Initialization when entering record mode, include data preparation, set position, and activate or deactivate objects
            public void EnterRecordMode(){
                _PrepareRecordAvatars();
                _SetAvatarsPosition(_recordAvatars, 1f, 4.5f, 3.5f);
                _recordAvatars.SetActive(true);
                _play_originAvatars.SetActive(false);
                _play_modifyAvatars.SetActive(false);
                _prevDirection = Vector3.forward;
            }

            public void SetRecordAvatarsPose(){
                // Mocopi motion data are sent to RecordAvatar_Front
                _recordPoseHandlers[0].GetHumanPose(ref _recordPose);

                // Copy the pose of RecordAvatar_Front to other RecordAvatars
                for(int ID = 1 ; ID< _recordAvatars.transform.childCount ; ID++){
                    _recordPoseHandlers[ID].SetHumanPose(ref _recordPose);
                }
            }
            
            // Make the avatar follow MR helmet in single student avatar mode
            public void MakeRecordAvatarFollowCamera(){
                _UpdateTargetPosition();                

                Transform avatarTransform = _recordAvatars.transform.GetChild(0);
                avatarTransform.position = Vector3.SmoothDamp(
                    avatarTransform.position,
                    _targetPosition,
                    ref _currentVelocity,
                    movementDuration
                );

                // 確保高度固定
                avatarTransform.position = new Vector3(
                    avatarTransform.position.x,
                    _height,
                    avatarTransform.position.z
                );
            }
            // Update targetPosition in MakeRecordAvatarFollowCamera()
            private void _UpdateTargetPosition(){
                // 計算頭盔的右前方向
                Vector3 cameraRight = _cameraTransform.right;   // 頭盔的右方
                Vector3 cameraForward = _cameraTransform.forward; // 頭盔的前方
                //無視頭盔抬頭
                cameraRight.y = 0;
                cameraForward.y = 0;
                
                Vector3 direction = (cameraRight.normalized*rightOffset + cameraForward.normalized).normalized; // 頭盔的右前方方向
                if(Vector3.Distance(direction, _prevDirection) < directionThresh)
                    return;
                _prevDirection = direction;

                // 計算方向在圓周上的投影
                _targetPosition = direction * radius; // 目標位置位於圓周上
                _targetPosition.y = _height; //高度不變
            }

            // Set Student Avatar Number in record mode (0, 1, 8)
            public void SetRecordAvatarNumber(float p_studentAvatarMode){
                // 0 record avatar
                if(p_studentAvatarMode == 0){
                    for(int i=0 ; i<_recordAvatars.transform.childCount ; i++)
                        _recordAvatars.transform.GetChild(i).gameObject.SetActive(false);
                }
                // 1 record avatar
                else if(p_studentAvatarMode == 1){
                    _recordAvatars.transform.GetChild(0).gameObject.SetActive(true);
                    for(int i=1 ; i<_recordAvatars.transform.childCount ; i++)
                        _recordAvatars.transform.GetChild(i).gameObject.SetActive(false);
                }
                // 8 record avatar
                else{
                    _recordAvatars.transform.GetChild(0).position = _recordPos[0].position;
                    for(int i=0 ; i<_recordAvatars.transform.childCount ; i++)
                        _recordAvatars.transform.GetChild(i).gameObject.SetActive(true);
                }
            }


            /////////////////////////////////////////////////////////////////
            ////////////   Play Mode Avatars   ////////////////////////////////
            /////////////////////////////////////////////////////////////////
            
            // create user avatar in runtime
            public void InstantiatePlayModeAvatars(){
                int avatarId = PlayerPrefs.GetInt("AvatarId");

                // Play Origin Avatars
                for(int i=0 ; i<8 ; i++){
                    GameObject newAvatar = Instantiate(_avatarOptions[avatarId]);
                    newAvatar.SetActive(true);
                    newAvatar.transform.SetParent(_play_originAvatars.transform);
                    newAvatar.transform.position = _play_originPos[i].position;
                    newAvatar.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    // Add Name Tag
                    GameObject nameTag = Instantiate(_play_originNameTag);
                    nameTag.transform.SetParent(newAvatar.transform.GetChild(0).GetChild(0).GetChild(0));
                    nameTag.transform.localPosition = new Vector3(0, 1.0f, 0);
                }

                // Play Modify Avatars
                for(int i=0 ; i<8 ; i++){
                    GameObject newAvatar = Instantiate(_avatarOptions[avatarId]);
                    newAvatar.SetActive(true);
                    newAvatar.transform.SetParent(_play_modifyAvatars.transform);
                    newAvatar.transform.position = _play_modifyPos[i].position;
                    newAvatar.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    // Add Name Tag
                    GameObject nameTag = Instantiate(_play_modifyNameTag);
                    nameTag.transform.SetParent(newAvatar.transform.GetChild(0).GetChild(0).GetChild(0));
                    nameTag.transform.localPosition = new Vector3(0, 1.0f, 0);
                }
            }

            // Prepare animators and poseHandler for each student avatar
            public void _PreparePlayAvatars(){
                // For origin Avatars
                _play_originAnimators = new List<Animator>();
                _play_originPoseHandlers = new List<HumanPoseHandler>();
                for(int ID = 0 ; ID< _play_originAvatars.transform.childCount ; ID++){
                    _play_originAnimators.Add(_play_originAvatars.transform.GetChild(ID).GetChild(0).GetComponent<Animator>());
                    _play_originPoseHandlers.Add(new HumanPoseHandler(_play_originAnimators[ID].avatar, _play_originAnimators[ID].transform));
                }
                _play_originPose = new HumanPose();
                _play_originPose.muscles = new float[95];

                // For modify Avatars
                _play_modifyAnimators = new List<Animator>();
                _play_modifyPoseHandlers = new List<HumanPoseHandler>();
                for(int ID = 0 ; ID< _play_modifyAvatars.transform.childCount ; ID++){
                    _play_modifyAnimators.Add(_play_modifyAvatars.transform.GetChild(ID).GetChild(0).GetComponent<Animator>());
                    _play_modifyPoseHandlers.Add(new HumanPoseHandler(_play_modifyAnimators[ID].avatar, _play_modifyAnimators[ID].transform));
                }
                _play_modifyPose = new HumanPose();
                _play_modifyPose.muscles = new float[95];
            }
                

            // Set student avatar pose according to motionData
            public void SetPlayModeAvatarPose(MuscleValues p_muscleValue, ModifyValues p_modifyValue){
                //For origin avatars
                for (int i = 0; i < _play_originPose.muscles.Length; ++i)
                    _play_originPose.muscles[i] = p_muscleValue.muscleValues[i];

                _play_originPose.bodyPosition = p_muscleValue.position;
                _play_originPose.bodyRotation = p_muscleValue.rotation;

                for(int ID = 0 ; ID<_play_originAvatars.transform.childCount ; ID++){
                    _play_originPoseHandlers[ID].SetHumanPose(ref _play_originPose);
                }

                //For modify avatars
                if(p_modifyValue == null){
                    for (int i = 0; i < _play_modifyPose.muscles.Length; ++i)
                        _play_modifyPose.muscles[i] = p_muscleValue.muscleValues[i];

                    _play_modifyPose.bodyPosition = p_muscleValue.position;
                    _play_modifyPose.bodyRotation = p_muscleValue.rotation;

                    for(int ID = 0 ; ID<_play_modifyAvatars.transform.childCount ; ID++)
                        _play_modifyPoseHandlers[ID].SetHumanPose(ref _play_modifyPose);
                }
                else{
                    for (int i = 0; i < _play_modifyPose.muscles.Length; ++i)
                        _play_modifyPose.muscles[i] = p_modifyValue.muscleValues[i];

                    _play_modifyPose.bodyPosition = p_modifyValue.position;
                    _play_modifyPose.bodyRotation = p_modifyValue.rotation;

                    for(int ID = 0 ; ID<_play_modifyAvatars.transform.childCount ; ID++)
                        _play_modifyPoseHandlers[ID].SetHumanPose(ref _play_modifyPose);
                }
            }

            
            // In single student avatar mode of play mode, switch the avatar pose between original pose and modified pose
            public void SetSingleAvatarPose(bool p_singleAvatarOriginPose){
                _play_originAvatars.SetActive(p_singleAvatarOriginPose);
                _play_modifyAvatars.SetActive(!p_singleAvatarOriginPose);
            }

            // Initialization when entering play mode, include data preparation, set position, and activate or deactivate objects
            public void EnterPlayMode(bool p_singleAvatarMode, bool p_singleAvatarOriginPose){
                _recordAvatars.SetActive(false);
                if(p_singleAvatarMode){
                    SetSingleAvatarPose(p_singleAvatarOriginPose);
                    _SetAvatarsPosition(_play_originAvatars, 1f, 3.5f, 4f);
                    _SetAvatarsPosition(_play_modifyAvatars, 1f, 3.5f, 4f);
                    
                }
                else{
                    _play_originAvatars.SetActive(true);
                    _play_modifyAvatars.SetActive(true);
                    _SetAvatarsPosition(_play_originAvatars, 0f, 4.5f, 5f);
                    _SetAvatarsPosition(_play_modifyAvatars, 1.5f, 3.0f, 3.5f);
                    
                }
                _PreparePlayAvatars();
            }
        }
    }    
}