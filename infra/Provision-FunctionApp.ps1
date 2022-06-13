# Provisions the Azure Functions app and other relevant resources.
Param(
    [string]
    [Parameter(Mandatory=$false)]
    $DeploymentName = "",

    [string]
    [Parameter(Mandatory=$false)]
    $ResourceGroupName = "",

    [string]
    [Parameter(Mandatory=$false)]
    $ResourceName = "",

    [string]
    [Parameter(Mandatory=$false)]
    $ResourceNameSuffix = "",

    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet(
        "australiacentral",
        "australiaeast",
        "australiasoutheast",
        "brazilsouth",
        "canadacentral",
        "canadaeast",
        "centralindia",
        "centralus",
        "eastasia",
        "eastus",
        "eastus2",
        "francecentral",
        "germanywestcentral",
        "japaneast",
        "japanwest",
        "jioindianorthwest",
        "koreacentral",
        "koreasouth",
        "northcentralus",
        "northeurope",
        "norwayeast",
        "southafricanorth",
        "southcentralus",
        "southindia",
        "southeastasia",
        "swedencentral",
        "switzerlandnorth",
        "uaenorth",
        "uksouth",
        "ukwest",
        "westcentralus",
        "westeurope",
        "westindia",
        "westus",
        "westus2",
        "westus3"
    )]
    $Location = "koreacentral",

    [string]
    [Parameter(Mandatory=$false)]
    $Environment = "dev",

    ### Storage Account ###
    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet("Standard_GRS", "Standard_LRS", "Standard_ZRS", "Standard_GZRS", "Standard_RAGRS", "Standard_RAGZRS", "Premium_LRS", "Premium_ZRS")]
    $StorageAccountSku = "Standard_LRS",

    [string[]]
    [Parameter(Mandatory=$false)]
    $StorageAccountBlobContainers = @(),

    [string[]]
    [Parameter(Mandatory=$false)]
    $StorageAccountTables = @(),
    ### Storage Account ###

    ### Log Analytics ###
    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet("Free", "Standard", "Premium", "Standalone", "LACluster", "PerGB2018", "PerNode", "CapacityReservation")]
    $LogAnalyticsWorkspaceSku = "PerGB2018",
    ### Log Analytics ###

    ### Application Insights ###
    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet("web", "other")]
    $AppInsightsType = "web",

    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet("ApplicationInsights", "ApplicationInsightsWithDiagnosticSettings", "LogAnalytics")]
    $AppInsightsIngestionMode = "LogAnalytics",
    ### Application Insights ###

    ### Consumption Plan ###
    [bool]
    [Parameter(Mandatory=$false)]
    $ConsumptionPlanIsLinux = $false,
    ### Consumption Plan ###

    ### Function App ###
    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet("Development", "Staging", "Production")]
    $FunctionEnvironment = "Production",

    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet("v3", "v4")]
    $FunctionExtensionVersion = "v4",

    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet("dotnet", "dotnet-isolated", "java", "node", "poweshell", "python")]
    $FunctionWorkerRuntime = "dotnet",

    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet("v6.0", "v8", "v11", "v12", "v14", "v16", "v3.7", "v3.8", "v3.9", "v7")]
    $FunctionWorkerRuntimeVersion = "v6.0",
    ### Function App ###

    [switch]
    [Parameter(Mandatory=$false)]
    $ProvisionedResults,

    [switch]
    [Parameter(Mandatory=$false)]
    $WhatIf,

    [switch]
    [Parameter(Mandatory=$false)]
    $Help
)

function Show-Usage {
    Write-Output "    This provisions resources to Azure

    Usage: $(Split-Path $MyInvocation.ScriptName -Leaf) ``
            -DeploymentName <deployment name> ``
            -ResourceGroupName <resource group name> ``
            -ResourceName <resource name> ``
            [-ResourceNameSuffix <resource name suffix>] ``
            [-Location <location>] ``
            [-Environment <environment>] ``

            [-StorageAccountSku <Storage Account SKU>] ``
            [-StorageAccountBlobContainers <Storage Account blob containers>] ``
            [-StorageAccountTables <Storage Account tables>] ``

            [-LogAnalyticsWorkspaceSku <Log Analytics workspace SKU>] ``

            [-AppInsightsType <Application Insights type>] ``
            [-AppInsightsIngestionMode <Application Insights data ingestion mode>] ``

            [-ConsumptionPlanIsLinux <`$true|`$false>] ``

            [-FunctionEnvironment <Function App environment>] ``
            [-FunctionExtensionVersion <Function App extension version>] ``
            [-FunctionWorkerRuntime <Function App worker runtime>] ``
            [-FunctionWorkerRuntimeVersion <Function App worker runtime version>] ``

            [-ProvisionedResults] ``
            [-WhatIf] ``
            [-Help]

    Options:
        -DeploymentName                   Deployment name.
        -ResourceGroupName                Resource group name.
        -ResourceName                     Resource name.
        -ResourceNameSuffix               Resource name suffix.
                                          Default is empty string.
        -Location                         Resource location.
                                          Default is 'koreacentral'.
        -Environment                      environment.
                                          Default is 'dev'.

        -StorageAccountSku                Storage Account SKU.
                                          Default is 'Standard_LRS'.
        -StorageAccountBlobContainers     Storage Account blob containers array.
                                          Default is empty array.
        -StorageAccountTables             Storage Account tables array.
                                          Default is empty array.

        -LogAnalyticsWorkspaceSku         Log Analytics workspace SKU.
                                          Default is 'PerGB2018'.

        -AppInsightsType                  Application Insights type.
                                          Default is 'web'.
        -AppInsightsIngestionMode         Application Insights data ingestion
                                          mode. Default is 'ApplicationInsights'.

        -ConsumptionPlanIsLinux           To enable Linux Consumption Plan
                                          or not. Default is `$false.

        -FunctionEnvironment              Function App environment.
                                          Default is 'Production'.
        -FunctionExtensionVersion         Function App extension version.
                                          Default is 'v4'.
        -FunctionWorkerRuntime            Function App worker runtime.
                                          Default is 'dotnet'.
        -FunctionWorkerRuntimeVersion     Function App worker runtime version.
                                          Default is 'v6.0'.

        -ProvisionedResults               Show provisioned results.
        -WhatIf:                          Show what would happen without
                                          actually provisioning resources.
        -Help:                            Show this message.
"

    Exit 0
}

# Show usage
$needHelp = ($DeploymentName -eq "") -or ($ResourceGroupName -eq "") -or ($ResourceName -eq "") -or ($Help -eq $true)
if ($needHelp -eq $true) {
    Show-Usage
    Exit 0
}

# Build parameters
$params = @{
    name = @{ value = $ResourceName };
    suffix = @{ value = $ResourceNameSuffix };
    location = @{ value = $Location };
    env = @{ value = $Environment };

    storageAccountSku = @{ value = $StorageAccountSku };
    storageAccountBlobContainers = @{ value = $StorageAccountBlobContainers };
    storageAccountTables = @{ value = $StorageAccountTables };

    workspaceSku = @{ value = $LogAnalyticsWorkspaceSku };

    appInsightsType = @{ value = $AppInsightsType };
    appInsightsIngestionMode = @{ value = $AppInsightsIngestionMode };

    consumptionPlanIsLinux = @{ value = $ConsumptionPlanIsLinux };

    functionEnvironment = @{ value = $FunctionEnvironment };
    functionExtensionVersion = @{ value = $FunctionExtensionVersion };
    functionWorkerRuntime = @{ value = $FunctionWorkerRuntime };
    functionWorkerVersion = @{ value = $FunctionWorkerRuntimeVersion };
}

# Uncomment to debug
# $params | ConvertTo-Json
# $params | ConvertTo-Json -Compress
# $params | ConvertTo-Json -Compress | ConvertTo-Json

$stringified = $params | ConvertTo-Json -Compress | ConvertTo-Json

# Provision the resources
if ($WhatIf -eq $true) {
    Write-Output "[$(Get-Date -Format "yyyy-MM-dd HH:mm:ss")] Provisioning resources as a test ..."
    $provisioned = az deployment group create -n $DeploymentName -g $ResourceGroupName `
        -f ./provision-functionApp.bicep `
        -p $stringified `
        -w

        # -u https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/infra/provision-functionApp.json `
} else {
    Write-Output "[$(Get-Date -Format "yyyy-MM-dd HH:mm:ss")] Provisioning resources ..."
    $provisioned = az deployment group create -n $DeploymentName -g $ResourceGroupName `
        -f ./provision-functionApp.bicep `
        -p $stringified `
        --verbose

        # -u https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/infra/provision-functionApp.json `

    if ($ProvisionedResults -eq $true) {
        Write-Output $provisioned
    }
    
    Write-Output "[$(Get-Date -Format "yyyy-MM-dd HH:mm:ss")] Resources have been provisioned"
}

Remove-Variable provisioned
