using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.EditorApplication
{

    public class RoyaleSupportPermissions
    {
        public const string GroupName = "RoyaleSupport";

        public class SqlQuery
        {
            public const string Default = GroupName + ".SqlQuery";

            public const string Admin = Default + ".SqlQueryAdmin";
        }

        public class Account
        {
            public const string Default = GroupName + ".Account";

            public const string CharacterPasswordView = Default + ".CharacterPasswordView";

            public const string CharacterPasswordChange = Default + ".CharacterPasswordChange";
            public const string ChatPunishment = Default + ".ChatPunishment";
            public const string AccountPunishment = Default + ".AccountPunishment";
        }
    }

    public static class PermissionExtensions
    {
        public static bool IsGranted(string permission)
        {
            var config = EditorApplication.Configuration;
            if (config == null)
                return false;

            var auth = config.Auth;
            if (auth == null)
                return false;

            var policies = auth.GrantedPolicies;
            if (policies == null || policies.Count == 0)
                return false;

            if (policies.TryGetValue(permission, out var IsGranted))
                return IsGranted != null && IsGranted.Value;

            return false;
        }
    }
}
