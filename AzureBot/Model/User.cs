namespace AzureBot.Model
{
    public class User
    {
        public User(string id)
        {
            Id = id;
        }

        public string Token { get; set; }
        public string Id { get; }
        public string Name { get; set; }
    }
}