using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CML.Utilities
{
    public class Structs
    {
        public struct EmailNotices
        {
            public const string CreatedRequest = "NewRequest";
            public const string ApprovalRequired = "ApprovalRequired";
            public const string ApprovalNotice = "ApprovalNotice";
            public const string ApprovalCancelled = "ApprovalCancelled";
            public const string RequestComplete = "Completed";
            public const string TestAssignment = "TestAssignment";
            public const string RequestAssignment = "RequestAssignment";
            public const string RequestRejection = "Rejection";
            public const string RequestApproved = "RequestApproved";
            public const string TestComplete = "TestComplete";
            public const string TestCancelled = "TestCancelled";
            public const string RequestCancelled = "RequestCancelled";

        }

        public struct Statuses
        {
            public const int Open = 1;
            public const int AwaitingApproval = 2;
            public const int Approved = 3;
            public const int Testing = 4;
            public const int Complete = 5;
            public const int Cancelled = 6;
            public const int Rejected = 7;
            public const int TestsComplete = 8;
        }

        public struct Players
        {
            public const string LabManager = "LabManager";
        }

        public struct Outcomes
        {
            public const string Approved = "Approved";
            public const string Rejected = "Rejected";
            public const string Cancelled = "Cancelled";
        }

        public struct DashPanelType
        {
            public const string OpenCreated = "Open";
            public const string OpenAssignedRequest = "OpenAssignedRequest";
            public const string OpenAssignedTest = "OpenAssignedTest";
            public const string Approvals = "Approvals";
            public const string Approved = "Approved";
            public const string Rejected = "Rejected";
            public const string Cancelled = "Cancelled";

        }
    }
}