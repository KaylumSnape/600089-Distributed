namespace DistSysAcwClient.Class
{
    // The JSON object to change a users role.
    // In it's own class to decouple.
    internal class ChangeRole
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public ChangeRole(string userName, string role)
        {
            Username = userName;
            Role = role;
        }
    }
}