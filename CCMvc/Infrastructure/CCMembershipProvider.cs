using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CCData;
using Helpers;

namespace CCMvc.Infrastructure
{
    public class CCMembershipProvider: MembershipProvider
    {
        #region GetUser
        public CCMembershipUser GetUser(string username, string password)
        {
            using (var db = new CCEntities())
            {
                var user = db.Humen.SingleOrDefault(u => u.Username == username);
                if (user == null)
                    throw new InvalidCredentialsException();
                else
                {
                    if (!PasswordHash.ValidatePassword(password, user.Password))
                        throw new InvalidCredentialsException();
                    else
                        return Initialize(user);
                }
            }
        }
        #endregion

        #region Initialize
        public CCMembershipUser Initialize(Human user)
        {
            return new CCMembershipUser(
                "CCMembershipProvider",
                user.Username,
                user.HumanId,
                user.Email,
                "",
                false,
                false,
                Convert.ToDateTime(user.CreatedOn),
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                user.OrganizationId);
        }
        #endregion

        #region #region Overrides of MembershipProvider that throw NotImplementedException
        public override bool ValidateUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        protected override byte[] DecryptPassword(byte[] encodedPassword)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}