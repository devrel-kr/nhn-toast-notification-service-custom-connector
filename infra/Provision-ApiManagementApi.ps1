# Provisions API to the API Management service instance.
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

    ### API Management ###
    [string]
    [Parameter(Mandatory=$false)]
    $ApiManagementNameValueName = "",

    [string]
    [Parameter(Mandatory=$false)]
    $ApiManagementNameValueDisplayName = "",

    [string]
    [Parameter(Mandatory=$false)]
    $ApiManagementNameValueValue = "",

    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet("http", "soap", "websocket", "graphql")]
    $ApiManagementApiType = "http",

    [string]
    [Parameter(Mandatory=$false)]
    $ApiManagementApiName = "",

    [string]
    [Parameter(Mandatory=$false)]
    $ApiManagementApiDisplayName = "",

    [string]
    [Parameter(Mandatory=$false)]
    $ApiManagementApiDescription = "",

    [string]
    [Parameter(Mandatory=$false)]
    $ApiManagementApiPath = "",

    [bool]
    [Parameter(Mandatory=$false)]
    $ApiManagementApiSubscriptionRequired = $false,

    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet("swagger-json", "swagger-link-json", "openapi", "openapi+json", "openapi+json-link", "openapi-link", "wadl-link-json", "wadl-xml", "wsdl", "wsdl-link", "graphql-link")]
    $ApiManagementApiFormat = "openapi+json-link",

    [string]
    [Parameter(Mandatory=$false)]
    $ApiManagementApiValue = "",

    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet("rawxml", "rawxml-link", "xml", "xml-link")]
    $ApiManagementApiPolicyFormat = "rawxml-link",

    [string]
    [Parameter(Mandatory=$false)]
    $ApiManagementApiPolicyValue = "",
    ### API Management ###

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
            [-Location <location>] ``
            [-Environment <environment>] ``

            [-ApiManagementNameValueName <API Management NameValue name>] ``
            [-ApiManagementNameValueDisplayName <API Management NameValue display name>] ``
            [-ApiManagementNameValueValue <API Management NameValue value>] ``
            [-ApiManagementApiType <API Management API type>] ``
            [-ApiManagementApiName <API Management API name>] ``
            [-ApiManagementApiDisplayName <API Management API display name>] ``
            [-ApiManagementApiDescription <API Management API description>] ``
            [-ApiManagementApiPath <API Management API path>] ``
            [-ApiManagementApiSubscriptionRequired <`$true|`$false>] ``
            [-ApiManagementApiFormat <API Management API format>] ``
            [-ApiManagementApiValue <API Management API value>] ``
            [-ApiManagementApiPolicyFormat <API Management API policy format>] ``
            [-ApiManagementApiPolicyValue <API Management API policy value>] ``

            [-ProvisionedResults] ``
            [-WhatIf] ``
            [-Help] ``

    Options:
        -DeploymentName                   Deployment name.
        -ResourceGroupName                Resource group name.
        -ResourceName                     Resource name.
        -Location                         Resource location.
                                          Default is 'koreacentral'.
        -Environment                      environment.
                                          Default is 'dev'.

        -ApiManagementNameValueName       API Management name/value name.
                                          Default is empty string.
        -ApiManagementNameValueDisplayName
                                          API Management name/value display name.
                                          Default is empty string.
        -ApiManagementNameValueValue      API Management name/value value.
                                          Default is empty string.
        -ApiManagementApiType             API Management API type.
                                          Default is 'http'.
        -ApiManagementApiName             API Management API name.
                                          Default is empty string.
        -ApiManagementApiDisplayName      API Management API display name.
                                          Default is empty string.
        -ApiManagementApiDescription      API Management API description.
                                          Default is empty string.
        -ApiManagementApiPath             API Management API path.
                                          Default is empty string.
        -ApiManagementApiSubscriptionRequired
                                          Value indicating whether the API
                                          subscription is required or not.
                                          Default is `$false.
        -ApiManagementApiFormat           API Management API format.
                                          Default is 'openapi+json-link'.
        -ApiManagementApiValue            API Management API value.
                                          Default is empty string.
        -ApiManagementApiPolicyFormat     API Management API format.
                                          Default is 'rawxml-link'.
        -ApiManagementApiPolicyValue      API Management API value.
                                          Default is empty string.
        
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
$needHelp = ($ApiManagementNameValueName -eq "") -or ($ApiManagementNameValueDisplayName -eq "") -or ($ApiManagementNameValueValue -eq "")
if ($needHelp -eq $true) {
    Show-Usage
    Exit 0
}
$needHelp = ($ApiManagementApiName -eq "") -or ($ApiManagementApiDisplayName -eq "") -or ($ApiManagementApiDescription -eq "") -or ($ApiManagementApiPath -eq "") -or ($ApiManagementApiValue -eq "") -or ($ApiManagementApiPolicyValue -eq "")
if ($needHelp -eq $true) {
    Show-Usage
    Exit 0
}

# Build parameters
$params = @{
    name = @{ value = $ResourceName };
    location = @{ value = $Location };
    env = @{ value = $Environment };

    apiMgmtNameValueName = @{ value = $ApiManagementNameValueName };
    apiMgmtNameValueDisplayName = @{ value = $ApiManagementNameValueDisplayName };
    apiMgmtNameValueValue = @{ value = $ApiManagementNameValueValue };
    apiMgmtApiType = @{ value = $ApiManagementApiType };
    apiMgmtApiName = @{ value = $ApiManagementApiName };
    apiMgmtApiDisplayName = @{ value = $ApiManagementApiDisplayName };
    apiMgmtApiDescription = @{ value = $ApiManagementApiDescription };
    apiMgmtApiPath = @{ value = $ApiManagementApiPath };
    apiMgmtApiSubscriptionRequired = @{ value = $ApiManagementApiSubscriptionRequired };
    apiMgmtApiFormat = @{ value = $ApiManagementApiFormat };
    apiMgmtApiValue = @{ value = $ApiManagementApiValue };
    apiMgmtApiPolicyFormat = @{ value = $ApiManagementApiPolicyFormat };
    apiMgmtApiPolicyValue = @{ value = $ApiManagementApiPolicyValue };
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
        -f ./provision-apiManagementApi.bicep `
        -p $stringified `
        -w

        # -u https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/infra/provision-apiManagementApi.json `
} else {
    Write-Output "[$(Get-Date -Format "yyyy-MM-dd HH:mm:ss")] Provisioning resources ..."
    $provisioned = az deployment group create -n $DeploymentName -g $ResourceGroupName `
        -f ./provision-apiManagementApi.bicep `
        -p $stringified `
        --verbose

        # -u https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/infra/provision-apiManagementApi.json `

    if ($ProvisionedResults -eq $true) {
        Write-Output $provisioned
    }

    Write-Output "[$(Get-Date -Format "yyyy-MM-dd HH:mm:ss")] Resources have been provisioned"
}

Remove-Variable provisioned
