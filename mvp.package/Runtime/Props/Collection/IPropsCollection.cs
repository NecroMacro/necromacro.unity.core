using System;
using System.Collections.Generic;

namespace NecroMacro.UI
{
	public interface IPropsCollection
	{
		void InitProps(IEnumerable<IUIProps> props);
		List<IUIProps> GetAllPropsReferences();
		T GetProps<T>() where T : class, IUIProps;
		T GetProps<T>(Type derivedType) where T : class, IUIProps;
		void Reset();
	}
}