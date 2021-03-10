using Microsoft.AspNetCore.Http;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class Helper
    {
        public class UserProfile
        {
            public long UserId { get; set; }
            public long ProfileId { get; set; }
        }
        public UserProfile GetProfileId(HttpContext httpcontext, CandidatesContext db)
        {
            var userId = this.GetCurrentUser(httpcontext);
            long UserId, ProfileId;
            if (userId <= 0)
            {
                UserId = 0;
            }
            else
            {
                UserId = userId;
            }

            var user = db.Users.Where(x => x.Id == userId).SingleOrDefault();
            if (user.ProfileRuningId <= 0 || user.ProfileRuningId == null)
            {
                ProfileId = 0;
            }else
            {
                ProfileId = (long)user.ProfileRuningId;
            }

            UserProfile UP = new UserProfile()
            {
                ProfileId = ProfileId,
                UserId = UserId
            };

            return UP;
        }

        public long GetCurrentUser(HttpContext HttpUser)
        {
            try
            {
                var user = HttpUser.User;
                if (user == null || user.Claims == null)
                {
                    return 0;
                }

                var claims = user.Claims.ToList();

                if (claims.Count == 0)
                {
                    //return 0;
                    return 1;
                }
                string userIdClaim = "";
                if (claims.Count > 1)
                {
                    userIdClaim = claims.Where(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/id").ToList()[0].Value;
                }
                else
                {
                    userIdClaim = claims.Where(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/id").SingleOrDefault().Value;
                }


                long userId = Convert.ToInt64(userIdClaim);


                return Convert.ToInt64(userId);
            }
            catch (Exception)
            {
                return -999;
            }
        }




    }
}
