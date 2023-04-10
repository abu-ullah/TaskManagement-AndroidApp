using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementProject.Models
{
    public class TaskModel
    {
        public string Description { get; set; }

        public string AssignedToName { get; set; }

        public string AssignedToUid { get; set; }

        public string CreatedByName { get; set; }

        public string CreatedByUid { get; set; }

        public string TaskUid { get; set; }

        public bool  Done { get; set; }

        public TaskModel(string assignedToName, string assignedToUid, string createdByName, string createdByUid, string description, bool done, string taskUid)
        {
            Description = description;
            AssignedToName = assignedToName;
            AssignedToUid = assignedToUid;
            CreatedByName = createdByName;
            CreatedByUid = createdByUid;
            TaskUid = taskUid;
            Done = done;
        }

        public TaskModel()
        {

        }
    }
}
