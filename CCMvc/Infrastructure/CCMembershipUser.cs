using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CCMvc.Infrastructure
{
    public class CCMembershipUser: MembershipUser
    {
        private long organizationId;
        public long OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }

        public CCMembershipUser(
            string providername,
            string username,
            object providerUserKey,
            string email,
            string comment,
            bool isApproved,
            bool isLockedOut,
            DateTime creationDate,
            DateTime lastLoginDate,
            DateTime lastActivityDate,
            DateTime lastPasswordChangedDate,
            DateTime lastLockedOutDate,
            long organizationId
            ) :
            base(providername,
                username,
                providerUserKey,
                email,
                "",
                comment,
                isApproved,
                isLockedOut,
                creationDate,
                lastLoginDate,
                lastActivityDate,
                lastPasswordChangedDate,
                lastLockedOutDate)
        {
            this.organizationId = organizationId;
        }
    }
}