namespace Domain;

public enum AccessLevel
{
    NONE = 0,
    VIEWER = 1,
    COLLABORATOR = 2,
    ADMINISTRATOR = 3
}

public enum AccessState
{
    NONE = 0,    
    INVITED = 1,
    ACCEPTED = 2,
    DECLINED= 3
}
