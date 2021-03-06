﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Management.Test.Utilities.Websites
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Management.Test.Utilities.Common;
    using Microsoft.WindowsAzure.Management.Utilities.Common;

    [TestClass]
    public class WebsitesTestBase : TestBase
    {
        protected string subscriptionName = "foo";

        [TestInitialize]
        public virtual void SetupTest()
        {
            GlobalPathInfo.AzureAppDir = Path.Combine(Directory.GetCurrentDirectory(), "Windows Azure Powershell");
            CmdletSubscriptionExtensions.SessionManager = new InMemorySessionManager();
            new FileSystemHelper(this).CreateAzureSdkDirectoryAndImportPublishSettings();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            GlobalPathInfo.AzureAppDir = Path.Combine(Directory.GetCurrentDirectory(), "Windows Azure Powershell");

            string webSpacesFile = Path.Combine(GlobalPathInfo.AzureAppDir,
                                                          string.Format("spaces.{0}.json", subscriptionName));

            string sitesFile = Path.Combine(GlobalPathInfo.AzureAppDir,
                                                          string.Format("sites.{0}.json", subscriptionName));

            if (File.Exists(webSpacesFile))
            {
                File.Delete(webSpacesFile);
            }

            if (File.Exists(sitesFile))
            {
                File.Delete(sitesFile);
            }
        }
    }
}
