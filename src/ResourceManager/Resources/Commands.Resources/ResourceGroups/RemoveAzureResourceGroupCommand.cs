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

using Microsoft.Azure.Commands.Resources.Models;
using System.Management.Automation;
using ProjectResources = Microsoft.Azure.Commands.Resources.Properties.Resources;

namespace Microsoft.Azure.Commands.Resources
{
    using System.Linq;

    /// <summary>
    /// Removes a new resource group.
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "AzureRmResourceGroup", SupportsShouldProcess = true), OutputType(typeof(bool))]
    public class RemoveAzureResourceGroupCommand : ResourcesBaseCmdlet
    {
        [Alias("ResourceGroupName")]
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The name of the resource group.")]
        [ValidateNotNullOrEmpty]
        public string Name {get; set;}

        [Alias("ResourceGroupId", "ResourceId")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = false, HelpMessage = "The resource group Id.")]
        [ValidateNotNullOrEmpty]
        public string Id { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Do not ask for confirmation.")]
        public SwitchParameter Force { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter PassThru { get; set; }
        
        protected override void ProcessRecord()
        {
            Name = string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Id)
                ? Id.Split('/').Last()
                : Name;

            ConfirmAction(
                Force.IsPresent,
                string.Format(ProjectResources.RemovingResourceGroup, Name),
                ProjectResources.RemoveResourceGroupMessage,
                Name,
                () => ResourcesClient.DeleteResourceGroup(Name));

            if (PassThru)
            {
                WriteWarning("The PassThru switch parameter is being deprecated and will be removed in a future release.");
                WriteObject(true);
            }
        }
    }
}