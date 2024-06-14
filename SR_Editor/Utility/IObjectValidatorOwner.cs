using System;

namespace SR_Editor.Core.Utility
{
	public interface IObjectValidatorOwner
	{
		bool IsValidating
		{
			get;
		}

		void ClearError(object obj);

		void ClearErrors();

		void SetError(object obj, string errorText);

		void SetError(object obj, string errorText, bool focus);
	}
}