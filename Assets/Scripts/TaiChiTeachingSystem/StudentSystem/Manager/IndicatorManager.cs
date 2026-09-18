using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class IndicatorManager : MonoBehaviour
        {
            [SerializeField] private Indicators[] _play_originIndicatorList;
            [SerializeField] private Indicators[] _play_modifyIndicatorList;
            [SerializeField] private GameObject _play_originAvatars;
            [SerializeField] private GameObject _play_modifyAvatars;

            [SerializeField] private Material _originMaterial;
            [SerializeField] private Material _modifyMaterial;
            private HumanBodyBones[] _humanBodyBones = {
                HumanBodyBones.Hips,
                HumanBodyBones.Spine,
                HumanBodyBones.Chest,
                HumanBodyBones.UpperChest,
                HumanBodyBones.Neck,
                HumanBodyBones.RightShoulder,
                HumanBodyBones.RightUpperArm,
                HumanBodyBones.RightLowerArm,
                HumanBodyBones.RightHand,
                HumanBodyBones.LeftShoulder,
                HumanBodyBones.LeftUpperArm,
                HumanBodyBones.LeftLowerArm,
                HumanBodyBones.LeftHand,
                HumanBodyBones.RightUpperLeg,
                HumanBodyBones.RightLowerLeg,
                HumanBodyBones.RightFoot,
                HumanBodyBones.LeftUpperLeg,
                HumanBodyBones.LeftLowerLeg,
                HumanBodyBones.LeftFoot
            };

            /////////////////////////////////////////////////////////////////
            ////////// Map the indicator to each body part///////////////////
            /////////////////////////////////////////////////////////////////
            public void SetIndicatorsBodyPart(){
                // play origin indicators
                for(int i=0 ; i<_play_originIndicatorList.Length ; i++){
                    Animator animator = _play_originAvatars.transform.GetChild(i).GetChild(0).GetComponent<Animator>();
                    Transform[] bodyParts = new Transform[_humanBodyBones.Length];
                    for(int j=0 ; j<_humanBodyBones.Length ; j++)
                        bodyParts[j] = animator.GetBoneTransform(_humanBodyBones[j]);
                    _play_originIndicatorList[i].SetIndicatorBodyPart(bodyParts);
                }

                // play modify indicators
                for(int i=0 ; i<_play_modifyIndicatorList.Length ; i++){
                    Animator animator = _play_modifyAvatars.transform.GetChild(i).GetChild(0).GetComponent<Animator>();
                    Transform[] bodyParts = new Transform[_humanBodyBones.Length];
                    for(int j=0 ; j<_humanBodyBones.Length ; j++)
                        bodyParts[j] = animator.GetBoneTransform(_humanBodyBones[j]);
                    _play_modifyIndicatorList[i].SetIndicatorBodyPart(bodyParts);
                }
            }

            //////////////////////////////////////////////////////////////////////////////////////////
            ////////// Set the indicator to the position of corresponding body part///////////////////
            //////////////////////////////////////////////////////////////////////////////////////////
            public void SetIndicatorTransform(){
                for(int i=0 ; i<_play_originIndicatorList.Length ; i++){
                    _play_originIndicatorList[i].SetIndicatorTransform();
                }
                for(int i=0 ; i<_play_modifyIndicatorList.Length ; i++){
                    _play_modifyIndicatorList[i].SetIndicatorTransform();
                }
            }

            //////////////////////////////////////////////////////////////////////////////////////////
            //////////////////  Activate or deactivate indicators  ///////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////
            public void SetIndicatorActive(bool p_active, bool p_singleAvatarMode, bool p_singleAvatarOriginPose){
                if(p_singleAvatarMode){
                    for(int i=0 ; i<_play_originIndicatorList.Length ; i++){
                        _play_originIndicatorList[i].gameObject.SetActive(p_active && p_singleAvatarOriginPose);
                    }
                    for(int i=0 ; i<_play_modifyIndicatorList.Length ; i++){
                        _play_modifyIndicatorList[i].gameObject.SetActive(p_active && !p_singleAvatarOriginPose);
                    }
                }
                else{
                    for(int i=0 ; i<_play_originIndicatorList.Length ; i++){
                        _play_originIndicatorList[i].gameObject.SetActive(p_active);
                    }
                    for(int i=0 ; i<_play_modifyIndicatorList.Length ; i++){
                        _play_modifyIndicatorList[i].gameObject.SetActive(p_active);
                    }
                }
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////// Set the material of each indiators based on the corresponding body part is modified or not //////////////////
            ////////// Original: Green,         Modified: Orange                                                  //////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public void SetIndicatorMaterial(List<AvatarBodyPartList> p_avatarBodyPartList){
                for(int i=0 ; i<_play_originIndicatorList.Length ; i++){
                    _play_originIndicatorList[i].SetIndicatorMaterial(p_avatarBodyPartList, _originMaterial, _modifyMaterial);
                }
                for(int i=0 ; i<_play_modifyIndicatorList.Length ; i++){
                    _play_modifyIndicatorList[i].SetIndicatorMaterial(p_avatarBodyPartList, _originMaterial, _modifyMaterial);
                }
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////// Reset the material of indicator to original(green)  ////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public void ResetIndicatorMaterial(){
                for(int i=0 ; i<_play_originIndicatorList.Length ; i++){
                    _play_originIndicatorList[i].ResetIndicatorMaterial( _originMaterial);
                }
                for(int i=0 ; i<_play_modifyIndicatorList.Length ; i++){
                    _play_modifyIndicatorList[i].ResetIndicatorMaterial(_originMaterial);
                }
            }

        }
    }
}