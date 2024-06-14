using System;
using System.DirectoryServices.AccountManagement;

namespace SR_Editor.Core.Utility
{
	public static class UtilActiveDirectory
	{
		public static bool IsKullaniciVarmi(string pDomainName, string pKullaniciAdi)
		{
			bool flag;
			try
			{
				PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, pDomainName);
				try
				{
					UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, pKullaniciAdi);
					try
					{
						flag = userPrincipal != null;
					}
					finally
					{
						if (userPrincipal != null)
						{
							((IDisposable)userPrincipal).Dispose();
						}
					}
				}
				finally
				{
					if (principalContext != null)
					{
						((IDisposable)principalContext).Dispose();
					}
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		public static bool IsSifreDogrumu(string pDomainName, string pKullaniciAdi, string pSifre)
		{
			bool flag;
			try
			{
				PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, pDomainName);
				try
				{
					flag = principalContext.ValidateCredentials(pKullaniciAdi, pSifre);
				}
				finally
				{
					if (principalContext != null)
					{
						((IDisposable)principalContext).Dispose();
					}
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}
	}
}