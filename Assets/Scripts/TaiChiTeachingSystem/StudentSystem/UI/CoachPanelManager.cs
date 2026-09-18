using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class CoachPanelManager : MonoBehaviour
        {
            [SerializeField] private Toggle _coachActiveToggle;
            [SerializeField] private TextMeshProUGUI _taichiMoveClassText;
            [SerializeField] private TextMeshProUGUI _taichiStartMoveText;
            [SerializeField] private TextMeshProUGUI _taichiEndMoveText;
            [SerializeField] private Button _taichiMoveClassPrevButton;
            [SerializeField] private Button _taichiMoveClassNextButton;
            [SerializeField] private Button _taichiStartMovePrevButton;
            [SerializeField] private Button _taichiStartMoveNextButton;
            [SerializeField] private Button _taichiEndMovePrevButton;
            [SerializeField] private Button _taichiEndMoveNextButton;


            public void SetTaichiMoveClassText(string p_className){
                _taichiMoveClassText.text = p_className;
            }
            public void SetTaichiStartMoveText(string p_moveName){
                _taichiStartMoveText.text = p_moveName;
            }
            public void SetTaichiEndMoveText(string p_moveName){
                _taichiEndMoveText.text = p_moveName;
            }
            public bool GetCoachActive(){
                return _coachActiveToggle.isOn;
            }
            public void SetTaichiMoveButtonInteractable(bool p_interactable){
                _taichiMoveClassPrevButton.interactable = p_interactable;
                _taichiMoveClassNextButton.interactable = p_interactable;
                _taichiStartMovePrevButton.interactable = p_interactable;
                _taichiStartMoveNextButton.interactable = p_interactable;
                _taichiEndMovePrevButton.interactable = p_interactable;
                _taichiEndMoveNextButton.interactable = p_interactable;
            }
        }
    }
}