# Provisions resources based on Flags
Param(
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
    $ApiVersion = "2021-08-01",

    [switch]
    [Parameter(Mandatory=$false)]
    $Help
)

function Show-Usage {
    Write-Output "    This permanently deletes the API Management instance

    Usage: $(Split-Path $MyInvocation.ScriptName -Leaf) ``
            -ResourceName <resource name> ``
            [-Location <location>] ``
            [-ApiVersion <API version>] ``

            [-Help]

    Options:
        -ResourceName   Resource name.
        -Location       Location. Default is `koreacentral`.
        -ApiVersion     REST API version. Default is `2021-08-01`.

        -Help:          Show this message.
"

    Exit 0
}

# Show usage
$needHelp = ($ResourceName -eq "") -or ($Help -eq $true)
if ($needHelp -eq $true) {
    Show-Usage
    Exit 0
}

$account = $(az account show | ConvertFrom-Json)

$url = "https://management.azure.com/subscriptions/$($account.id)/providers/Microsoft.ApiManagement/locations/$($Location)/deletedservices/$($ResourceName)?api-version=$($ApiVersion)"

# Uncomment to debug
# $url

Write-Output "[$(Get-Date -Format "yyyy-MM-dd HH:mm:ss")] Checking whether deleted API Management instance exists ..."
$apim = $(az rest -m get -u $url)

if ($apim -ne $null) {
    Write-Output "[$(Get-Date -Format "yyyy-MM-dd HH:mm:ss")] Purging deleted API Management instance ..."
    az rest -m delete -u $url --verbose

    Write-Output "[$(Get-Date -Format "yyyy-MM-dd HH:mm:ss")] Deleted API Management instance has been purged"
} else {
    Write-Output "[$(Get-Date -Format "yyyy-MM-dd HH:mm:ss")] No deleted API Management instance found to purge"
}
