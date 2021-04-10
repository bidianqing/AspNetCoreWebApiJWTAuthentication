namespace AspNetCoreWebApiJWTAuthentication.Models
{
    public class LoginReponseModel
    {
        public string Token { get; set; }

        // Test long to string 
        public long? Id { get; set; }
    }
}
