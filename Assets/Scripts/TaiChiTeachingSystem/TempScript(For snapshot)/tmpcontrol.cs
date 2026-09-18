using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class tmpcontrol : MonoBehaviour
        {
            public StudentTaichiSystem studentTaichiSystem;
            public PlayMode playMode;
            public ShowPanelsButton showPanelsButton;
            public CoachManager coachManager;
            // Start is called before the first frame update
            void Start()
            {
                
            }

            // Update is called once per frame
            void Update()
            {
                if (Input.GetKeyDown(KeyCode.Keypad0))
                {
                    studentTaichiSystem.SetMode(1);
                }
                if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    playMode.OpenLoadModifiedDataPanel();
                }
                if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    playMode.LoadModifiedData();
                }
                if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    playMode.SwitchOnPlay();
                }
                if (Input.GetKeyDown(KeyCode.Keypad4))
                {
                    playMode.SwitchSingleAvatarMode();
                }
                if (Input.GetKeyDown(KeyCode.Keypad5))
                {
                    showPanelsButton.SwitchShowPanels();
                }
                if (Input.GetKeyDown(KeyCode.Keypad6))
                {
                    coachManager.SetTaichiMoveClass(1);
                }
                if (Input.GetKeyDown(KeyCode.Keypad7))
                {
                    coachManager.SetTaichiStartMove(1);
                }

            }
        }
    }
}
