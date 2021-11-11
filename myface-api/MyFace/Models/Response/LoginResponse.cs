using MyFace.Models.Database;

namespace MyFace.Models.Response
{
    public class GetLoginResponse
    {
        public GetLoginResponse(RoleType roleType)
        {
            RoleType = roleType;
        }
        public RoleType RoleType { get; }
    }

}