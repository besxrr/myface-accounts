namespace MyFace.Models.Response
{
    public class JWTLoginResponse
    {
        public JWTLoginResponse(string token)
        {
            Token = token;
        }
        public string Token { get; }
    }

}