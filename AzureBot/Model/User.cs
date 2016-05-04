namespace AzureBot.Model
{
    public class User
    {
        private User(string id)
        {
            Id = id;
        }

        public static User GetOrCreate(string id)
        {
            var user = UserRegistry.GetSingleton().GetUser(id);

            if (user == null)
            {
                user = new User(id);
            }

            return user;
        }

        public string Token { get; set; }
        public string Id { get; }
        public string Name { get; set; }
    }
}