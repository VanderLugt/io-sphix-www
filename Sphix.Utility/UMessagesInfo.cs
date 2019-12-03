using System.Collections.Generic;
namespace Sphix.Utility
{
    public static class UMessagesInfo
    {
        public static Dictionary<string, string> timeZone = new Dictionary<string, string>();
        static UMessagesInfo()
        {
            timeZone.Add("PST", "PST");
            timeZone.Add("CST", "CST");
            timeZone.Add("HST", "HST");
            timeZone.Add("EST", "EST");
        }
        public static string MailFooter = @"<p><h5>Thanks and Regards Sphix Support</h5><br />
                        <a href ='http://ww2.sphix.io/' target='_blank'>ww2.sphix.io</a></p>";

        public static string AWSPublicURL = "https://io-sphix-www.s3-us-west-1.amazonaws.com/";
        public static string WebSiteUrl = "http://beta.app.sphix.io/";
        public static string Error = "Somthing went wrong. Please try later!";
        public static string UserNameExist = "The username {0} is already exists. Please use a different username.";
        public static string RecordExist = "This record is already exist!";
        public static string RecordNotExist = "Information in not exist!";
        public static string RecordSaved = "Record saved successfully.";
        public static string RecordDeleted = "Record deleted successfully!";
        public static string ProfileLinkExist = "This community page URL is already taken. Please try with different name.";

        public static string EmailSent = "mail sent successfully.";
        public static string VerificationEmailSubject = "Welcome to Sphix! Confirm Your Email.";
        public static string NewCommunityGroupEmailSubject = "New Community Group! In-Review";
        public static string CommunityGroupJoinedSuccessfully = "Thanks for joining this community group.";
        public static string ResetPasswordSubject = "Sphix reset password";
        public static string ResetPasswordLinkSentOnMail = "Password reset link has been sent to your email.";
        public static string ResetPasswordSuccessfully = "Your password has been reset successfully!";

        public static string SphixSupport = "Sphix Support";
        public static string SignUpSuccess = "Your account has been made, please verify it by clicking the activation link that has been send to your email.";
        public static string TokenExpired = "Your token has expired!";
        public static string TokenNotExists = "Wrong verification token.";
        public static string TokenAlreadyUsed = "This verification link has already used.";

        public static string MeetingJoinAlready = "You have already joined this meeting.";
        public static string WrongUserName = "You have entered an invalid username or password";
        public static string UserNameNotExist = "That Sphix account doesn't exist. Enter a different account or get a new one";

        public static string JoinCommunityEmailSubject = "Thanks!";
        //Email Invitation 
        public static string GroupEmailInvitationSubject = "Sphix! new group invitation.";

        public static bool CheckTimeZoneValidation(string timeZonneValue)
        {
            return timeZone.ContainsKey(timeZonneValue); 
        }
    }
}
