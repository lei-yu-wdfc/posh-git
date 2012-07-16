using System;
using System.IO;

namespace Wonga.QA.Generators.Core
{
	public class EntityGenerator
	{
		private const string OriginalNamespaceRemovablePattern = "Wonga.";


		private readonly DirectoryInfo _entityRootDirectory;

		private readonly string _targetFrameworkProject;

		private readonly string _targetFrameworkFolder;

		public EntityGenerator(DirectoryInfo entityRootDirectory, string targetFrameworkProject, string targetFrameworkFolder)
		{
			_entityRootDirectory = entityRootDirectory;
			_targetFrameworkProject = targetFrameworkProject;
			_targetFrameworkFolder = targetFrameworkFolder;
		}
		
		public GeneratedEntityDefinition GenerateEntityDefinition(Type entityType)
		{
			return GenerateEntityDefinition(entityType.Namespace);
		}

		public GeneratedEntityDefinition GenerateEntityDefinition(string entityNamespace)
		{
			string namespaceRelativePath =
						string.IsNullOrEmpty(entityNamespace)
							? string.Empty
							: entityNamespace.Replace(OriginalNamespaceRemovablePattern, string.Empty);

			string subfolderName = namespaceRelativePath.Replace(".", new string(Path.DirectorySeparatorChar, 1));

			string generatedNamespace =
				string.IsNullOrEmpty(namespaceRelativePath)
					? string.Format("{0}.{1}", _targetFrameworkProject, _targetFrameworkFolder)
					: string.Format("{0}.{1}.{2}", _targetFrameworkProject, _targetFrameworkFolder, namespaceRelativePath);

			DirectoryInfo entityDirectory = Repo.Directory(subfolderName, _entityRootDirectory);

			return new GeneratedEntityDefinition(generatedNamespace, entityDirectory);
		}

	}
}
