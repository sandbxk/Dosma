namespace Domain;

public class User
{
    public int Id { get; set; }
    public string DisplayName { get; set;} = string.Empty;

    public ICollection<UserList> GroceryLists { get; set; }
    public List<GroceryList> SharedLists { get; set; }

    /* The following properties are used for authentication
    *
    */

    public string Username { get; set; } = string.Empty;

    /* 
        HashedPassword will be a base 64 encoded string

        The password will be hashed using SHA512
    */
    public string HashedPassword { get; set; } = string.Empty;

    /*
        Salt will be a base 64 encoded string

        never send the salt to the client
    */
    public string Salt { get; set; } = string.Empty;
}