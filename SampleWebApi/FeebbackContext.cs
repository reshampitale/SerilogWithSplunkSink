using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace splunkWebApi
{

    public class Feedback
    {

        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackID { get; set; }
        public string Message { get; set; }


        public List<Feedback> GetAllFeedbacks()
        {
            List<Feedback> listOfFeedbacks = new List<Feedback>

{
    new Feedback { FeedbackID = 1111, Message = "Feebback for Morning"},
    new Feedback { FeedbackID = 1001, Message = "Feebback for Noon"},
    new Feedback { FeedbackID = 1002, Message = "Feebback for Evening" }
};



            return listOfFeedbacks;

        }
    }

        

    }
