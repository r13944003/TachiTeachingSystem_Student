using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;


namespace TaichiTeachingSystem
{
    namespace StudentSystem{
        public class StudentTaichiSystem : MonoBehaviour
        {
            [SerializeField] private PlayMode _playMode;
            [SerializeField] private RecordMode _recordMode;

            [SerializeField] private ModePanelManager _modePanelManager;
            private int _mode;


            void Start()
            {
                _mode = 0;
                SetMode(0);
            }
            void Update()
            {
                switch (_mode){
                    // Record Mode
                    case 0:
                        _recordMode.Run();
                        break;

                    // Play Mode
                    case 1:
                        _playMode.Run();
                        break;
                }
            }

            ////////////////////////////////////////////////////////////////////////////
            //////////////   Switch between Record Move or Play Mode  //////////////////
            ////////////////////////////////////////////////////////////////////////////
            public void SetMode(int p_modeChange){
                _mode = (_mode + p_modeChange+2)%2;
                _modePanelManager.SetModeText(_mode);
                switch (_mode){
                    // Record Mode
                    case 0:
                        _recordMode.Enter();
                        _playMode.Leave();

                        break;
                    // Play Mode
                    case 1:
                        _playMode.Enter();
                        _recordMode.Leave();
                        break;
                }
            }

            ////////////////////////////////////////////////////////////
            //////////////   Set FPS(Speed of move)  //////////////////
            ///////////////////////////////////////////////////////////
            public void SetFPS(){
                switch (_mode){
                    // Record Mode
                    case 0:
                        _recordMode.SetFPS();
                        break;

                    // Play Mode
                    case 1:
                        _playMode.SetFPS();
                        break;
                }
            }

        }


    }
}