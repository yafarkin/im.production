namespace IM.Production.Services
{
    public class User
    {
        public User(string login, string role, string token)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new System.ArgumentException("Login can not be empty or contain only white spaces.", nameof(login));
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                throw new System.ArgumentException("Role can not be empty or contain only white spaces.", nameof(role));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new System.ArgumentException("Token can not be empty or contain only white spaces.", nameof(token));
            }

            Login = login;
            Role = role;
            Token = token;
        }

        public string Login { get; }

        public string Role { get; }

        public string Token { get; }
    }
}
