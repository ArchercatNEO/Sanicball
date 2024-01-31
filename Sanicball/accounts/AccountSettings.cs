using System.Collections.Generic;

namespace Sanicball.Data;

public class AccountSettings
{
    public static List<AccountSettings> All { get; set; } = new();
    public static AccountSettings Active { get; set; } = new();

    public string name = "player";
}