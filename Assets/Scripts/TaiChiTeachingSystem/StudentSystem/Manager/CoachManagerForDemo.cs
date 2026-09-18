using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class CoachManagerForDemo : MonoBehaviour
        {
            [SerializeField] private CoachPanelManager _coachPanelManager;
            [SerializeField] private GameObject _coachAvatars;
            [SerializeField] private RuntimeAnimatorController _taichi13Controller;
            [SerializeField] private RuntimeAnimatorController _taichi24Controller;
            [SerializeField] private TextMeshProUGUI _playButtonText;
            public List<TaichiMoveClassInfo> _taichiMoveClassInfoList;
            private Animator _coachAnimator;
            private int _taichiMoveClassID;
            private int _taichiStartMoveID;
            private int _taichiEndMoveID;
            private bool _onPlay;
            

            void Start(){
                SetTaichiMoveClass(0);
                SetTaichiStartMove(0);
                SetTaichiEndMove(0);
            }
            public void EnterRecordMode(){
                RestartCoachMove();
                _onPlay = false;
                SetCoachSpeed(0);
                _SetCoachAvatarsPosition(-1f, 2.5f, 5.5f);
            }

            ///////////////////////////////////////////////////////
            ///////////// Set Taichi Move  ////////////////////////
            ///////////////////////////////////////////////////////
            public void SetTaichiMoveClass(int p_indexChange){
                int classNum = _taichiMoveClassInfoList.Count;
                _taichiMoveClassID = (_taichiMoveClassID + p_indexChange + classNum)%classNum; 
                RuntimeAnimatorController controller;
                if(_taichiMoveClassID == 0)
                    controller = _taichi13Controller;
                else
                    controller = _taichi24Controller;

                for(int i=0 ; i<_coachAvatars.transform.childCount ; i++)
                    _coachAvatars.transform.GetChild(i).GetChild(0).GetComponent<Animator>().runtimeAnimatorController = controller;

                // Set Class Name
                _coachPanelManager.SetTaichiMoveClassText(_taichiMoveClassInfoList[_taichiMoveClassID].className);

                // Set  taichi Move ID to 0
                SetTaichiStartMove(-_taichiStartMoveID);
                SetTaichiEndMove(-_taichiEndMoveID);
            }

            public void SetTaichiStartMove(int p_indexChange){
                int moveNum = _taichiMoveClassInfoList[_taichiMoveClassID].moveNum;
                _taichiStartMoveID = (_taichiStartMoveID + p_indexChange + moveNum)%moveNum;

                // Fist, make sure the endMoveID is larger than startMoveID
                if(_taichiStartMoveID > _taichiEndMoveID)
                    SetTaichiEndMove(_taichiStartMoveID - _taichiEndMoveID);

                for(int i=0 ; i<_coachAvatars.transform.childCount ; i++){
                    _coachAnimator = _coachAvatars.transform.GetChild(i).GetChild(0).GetComponent<Animator>();
                    _coachAnimator.SetInteger("TaichiStartMoveID", _taichiStartMoveID);
                    _coachAnimator.SetTrigger("ChangeTaichiStartMove");
                }

                // Set Start Move Name 
                _coachPanelManager.SetTaichiStartMoveText(_taichiMoveClassInfoList[_taichiMoveClassID].moveName[_taichiStartMoveID]);
                
            }
            public void SetTaichiEndMove(int p_indexChange){
                int range = _taichiMoveClassInfoList[_taichiMoveClassID].moveNum - _taichiStartMoveID;
                _taichiEndMoveID = _taichiStartMoveID + (_taichiEndMoveID - _taichiStartMoveID + p_indexChange + range)%range;

                for(int i=0 ; i<_coachAvatars.transform.childCount ; i++){
                    _coachAnimator = _coachAvatars.transform.GetChild(i).GetChild(0).GetComponent<Animator>();
                    _coachAnimator.SetInteger("TaichiEndMoveID", _taichiEndMoveID);
                }

                // Set End Move Name
                _coachPanelManager.SetTaichiEndMoveText(_taichiMoveClassInfoList[_taichiMoveClassID].moveName[_taichiEndMoveID]);
            }

            ///////////////////////////////////////////////////////
            /////////////   Play / Stop   ////////////////////////
            ///////////////////////////////////////////////////////

            public void Play(){
                SetCoachSpeed(1);
                _onPlay = true;
                _playButtonText.text = "Stop";
            }
            public void Stop(){
                SetCoachSpeed(0);
                _onPlay = false;
                _playButtonText.text = "Play";
            }

            public void ChangePlayState(){
                if(!_onPlay){
                    Play();
                }
                else{
                    Stop();
                }
            }

            
            ///////////////////////////////////////////////////////
            /////////////  Set Coach Position /////////////////////
            ///////////////////////////////////////////////////////
            public void SetCoachAvatarsPosition(int p_studentAvatarMode){
                // No student avatar
                if(p_studentAvatarMode == 0){
                    _SetCoachAvatarsPosition(0f, 2.5f, 3.5f);
                }
                // Eight student avatar
                else{
                    _SetCoachAvatarsPosition(-1f, 2.5f, 5.5f);
                }
            }


            ///////////////////////////////////////////////////////
            /////////////   Tools   ///////////////////////////////
            ///////////////////////////////////////////////////////
            public void SetCoachSpeed(float p_speed = -1){ // p_speed = -1 => Get speed from frame Panel
                if(p_speed == -1)
                    return;
                for(int i=0 ; i< _coachAvatars.transform.childCount ; i++){
                    _coachAnimator = _coachAvatars.transform.GetChild(i).GetChild(0).GetComponent<Animator>();
                    _coachAnimator.speed = p_speed;
                }
            }

            public void RestartCoachMove(){
                for(int i=0 ; i< _coachAvatars.transform.childCount ; i++){
                    _coachAnimator = _coachAvatars.transform.GetChild(i).GetChild(0).GetComponent<Animator>();
                    _coachAnimator.Play(0, -1, 0);
                }
            }
            private void _SetCoachAvatarsPosition(float p_shiftX1, float p_shiftX2, float p_shiftX3){
                // Set front and back avatar
                Vector3 pos = _coachAvatars.transform.GetChild(0).localPosition;
                _coachAvatars.transform.GetChild(0).localPosition = new Vector3(p_shiftX1, pos.y, pos.z);
                pos = _coachAvatars.transform.GetChild(3).localPosition;
                _coachAvatars.transform.GetChild(3).localPosition = new Vector3(p_shiftX1, pos.y, pos.z);

                // Set left and right avatar
                pos = _coachAvatars.transform.GetChild(1).localPosition;
                _coachAvatars.transform.GetChild(1).localPosition = new Vector3(-p_shiftX3, pos.y, pos.z);
                pos = _coachAvatars.transform.GetChild(2).localPosition;
                _coachAvatars.transform.GetChild(2).localPosition = new Vector3(p_shiftX3, pos.y, pos.z);

                // Set frontLeft and backLeft avatar
                pos = _coachAvatars.transform.GetChild(4).localPosition;
                _coachAvatars.transform.GetChild(4).localPosition = new Vector3(-p_shiftX2, pos.y, pos.z);
                pos = _coachAvatars.transform.GetChild(6).localPosition;
                _coachAvatars.transform.GetChild(6).localPosition = new Vector3(-p_shiftX2, pos.y, pos.z);

                // Set frontRight and backRight avatar
                pos = _coachAvatars.transform.GetChild(5).localPosition;
                _coachAvatars.transform.GetChild(5).localPosition = new Vector3(p_shiftX2, pos.y, pos.z);
                pos = _coachAvatars.transform.GetChild(7).localPosition;
                _coachAvatars.transform.GetChild(7).localPosition = new Vector3(p_shiftX2, pos.y, pos.z);
            }

        }
    }
}