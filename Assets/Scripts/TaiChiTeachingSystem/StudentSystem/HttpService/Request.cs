using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        [Serializable]
        public class token_request
        {
            public string email;
            public string password;
        }

        
        [Serializable]
        public class createUser_request
        {
            public string username;
            public string email;
            public string password;
        }
    }
}
