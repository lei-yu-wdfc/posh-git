using System;

namespace Wonga.QA.Generators.Core
{
	[Flags]
	public enum EnumGenerationMode
	{
		Standard = 0x00,
		UseNormalTypeName = 0x01,
		IncludeConstantValues = 0x02,
		IncludeDescription = 0x04,

		//COMBOS
		UseNormalTypeNameWithDescription = UseNormalTypeName | IncludeDescription,
		UseNormalTypeNameWithConstantValues = UseNormalTypeName | IncludeConstantValues,
	}
}
