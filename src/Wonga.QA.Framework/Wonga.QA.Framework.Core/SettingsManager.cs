using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Wonga.QA.Framework.Core
{
    internal class SettingsManager
    {
        private const string CONFIG_FOLDER_NAME = @"config";
        private const string RUN_FOLDER_NAME = @"run";
        private const string V3CONFIG_EXTENTION = "v3qaconfig";
        private const string V3QA_APPDATA_FOLDER = "v3qa";

        private DirectoryInfo _configsDirectory;
        private DirectoryInfo _executingDirectory;
        private DirectoryInfo _appDataDirectory;

        public string TestTarget { get; private set; }

        public string ExecutingDirectoryPath
        {
            get
            {
                return _executingDirectory != null ? _executingDirectory.FullName : "";
            }
        }

        public string ConfigDirectoryPath
        {
            get
            {
                return _configsDirectory != null ? _configsDirectory.FullName : "";
            }
        }

        public string AppDataDirectoryPath
        {
            get
            {
                return _appDataDirectory != null ? _appDataDirectory.FullName : "";
            }
        }

        public SettingsManager(string executingDirectoryPath, string configDirectoryPath, string appDataDirectoryPath, string testTarget)
        {
            _configsDirectory = configDirectoryPath != null ? new DirectoryInfo(configDirectoryPath) : GetDefaultConfigsFolder();
            _executingDirectory = executingDirectoryPath != null ? new DirectoryInfo(executingDirectoryPath) : new DirectoryInfo(Environment.CurrentDirectory);
            _appDataDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), V3QA_APPDATA_FOLDER));
            TestTarget = testTarget ?? Get.EnvironmentVariable<string>(null, "QAFTestTarget");
        }

        /// <summary>
        /// Hackish way to find out the config folder.
        /// </summary>
        /// <returns></returns>
        private DirectoryInfo GetDefaultConfigsFolder()
        {
            var runFolder = GetFolderUpwards(new DirectoryInfo(Environment.CurrentDirectory), RUN_FOLDER_NAME);
            return GetFolderUpwards(runFolder, CONFIG_FOLDER_NAME);
        }

        private static DirectoryInfo GetFolderUpwards(DirectoryInfo current, string folder)
        {
            var dirs = current.GetDirectories(folder);
            if (dirs.Length == 1)
                return dirs[0];
            if (current.Parent == null)
                return null;
            return GetFolderUpwards(current.Parent, folder);
        }

        public XDocument ReadSettings()
        {
            string settings = null;
            var proc = new XmlProcessor();
            var wildcard = string.Format("*.{0}", V3CONFIG_EXTENTION);
            var appDataConfigs = _appDataDirectory.Exists ? _appDataDirectory.GetFiles(wildcard) : new FileInfo[0];
            var binFolderConfigs = _executingDirectory.GetFiles(wildcard);
            var configFolderConfigs = _configsDirectory == null ? new FileInfo[0] : _configsDirectory.GetFiles(wildcard, SearchOption.AllDirectories);
            var testTargetConfig = configFolderConfigs.FirstOrDefault(x => x.Name.Equals(string.Format("{0}.{1}", TestTarget, V3CONFIG_EXTENTION), StringComparison.InvariantCultureIgnoreCase));

            //Figure out which settings file to use based on priority.
            if (binFolderConfigs.Length > 0)
                settings = binFolderConfigs[0].FullName;
            else if (testTargetConfig != null)
                settings = testTargetConfig.FullName;
            else if (appDataConfigs.Length > 0)
                settings = appDataConfigs[0].FullName;

            //Merge inheriting configs.
            XDocument doc = XDocument.Load(settings);
            while (doc.XPathSelectElement("//Include") != null)
            {
                var include = doc.XPathSelectElement("//Include");
                include.Remove();
                XDocument parentDoc =
                    XDocument.Load(
                        configFolderConfigs.First(x => x.FullName.EndsWith(string.Format("{0}.{1}", include.Value, V3CONFIG_EXTENTION))).FullName);
                var nodesToAddOrReplace = doc.Descendants().Where(x => x.Descendants().Count() == 0 && !string.IsNullOrEmpty(x.Value)).ToList();
                foreach (var nodeToAddOrReplace in nodesToAddOrReplace)
                {
                    var parentNode =
                        parentDoc.Descendants().FirstOrDefault(
                            x => x.GetAbsoluteXPath() == nodeToAddOrReplace.GetAbsoluteXPath());
                    if (parentNode != null)
                        parentNode.ReplaceWith(nodeToAddOrReplace);
                    else XmlProcessor.CreateNode(parentDoc, nodeToAddOrReplace.GetAbsoluteXPath()).ReplaceWith(nodeToAddOrReplace);
                }

                doc = parentDoc;
            }

            return proc.LoadFromXDoc(doc);
        }
    }
}
