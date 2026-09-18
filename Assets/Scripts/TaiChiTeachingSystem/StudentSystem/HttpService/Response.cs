using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaichiTeachingSystem{
    namespace StudentSystem{

        [Serializable]
        public class Token_response
        {
            public string access_token;
            public string refresh_token;
        }
        [Serializable]
        public class MeUser_response
        {
            public int id;
            public string username;
            public string email;
            public string created_at;
            public string updated_at;
        }

        [Serializable]
        public class CoachData
        {
            public int coachId;     // 
            public int userId;      // 對應到userTable的ID
            public string skill;
            public string createTime;
            public string updateTime;
        }
            
        [Serializable]
        public class CoachAll_response
        {
            public int total;
            public List<CoachData> data;
        }

        [Serializable]
        public class MotionRecord
        {
            public int id;
            public int userId;      
            public int coachId;
            public int coachUpdate;
            public string fileName;
            public string createTime;
            public string updateTime;
        }
            
        [Serializable]
        public class MotionDataAll_response
        {
            public int total;
            public List<MotionRecord> data;
        }
    }
}
