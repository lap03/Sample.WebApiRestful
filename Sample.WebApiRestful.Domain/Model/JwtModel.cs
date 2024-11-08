namespace Sample.WebApiRestful.Model
{
    public class JwtModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
