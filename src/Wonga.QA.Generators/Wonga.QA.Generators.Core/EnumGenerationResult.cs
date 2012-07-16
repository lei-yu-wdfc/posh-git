
namespace Wonga.QA.Generators.Core
{
	public class EnumGenerationResult
	{
		public EnumGenerationStatus Status { get; private set; }

		public GeneratedEnumDefinition EnumDefinition { get; private set; }

		public EnumGenerationResult(EnumGenerationStatus status, GeneratedEnumDefinition enumDefinition)
		{
			Status = status;
			EnumDefinition = enumDefinition;
		}
	}
}
