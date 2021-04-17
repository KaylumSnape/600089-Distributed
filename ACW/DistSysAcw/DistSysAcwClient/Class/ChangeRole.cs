namespace DistSysAcwClient.Class
{
    // The JSON object to change a users role.
    // In it's own class to decouple.
    internal class ChangeRole
    {
        public ChangeRole(string userName, string role)
        {
            username = userName;
            this.role = role;
        }

        public string username { get; set; }
        public string role { get; set; }
    }
}