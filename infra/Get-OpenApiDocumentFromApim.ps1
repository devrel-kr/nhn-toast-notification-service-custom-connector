# Download OpenAPI document from Azure API Management.
Param(
    [string]
    [Parameter(Mandatory=$false)]
    $ResourceGroupName = "",

    [string]
    [Parameter(Mandatory=$false)]
    $ServiceName = "",

    [string]
    [Parameter(Mandatory=$false)]
    $ApiId = "",

    [string]
    [Parameter(Mandatory=$false)]
    $ApiVersion = "2021-08-01",

    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet("v2", "v3")]
    $OpenApiVersion = "v3",

    [string]
    [Parameter(Mandatory=$false)]
    $OutputDirectory = ".",

    [string]
    [Parameter(Mandatory=$false)]
    $OutputFilename = "openapi.json",

    [switch]
    [Parameter(Mandatory=$false)]
    $Help
)

function Show-Usage {
    Write-Output "    This downloads the OpenAPI document from Azure API Management.

    Usage: $(Split-Path $MyInvocation.ScriptName -Leaf) ``
            -ResourceGroupName <resource group name> ``
            -ServiceName <APIM instance name> ``
            -ApiId <API ID on the APIM instance> ``
            [-ApiVersion <API version to call APIM REST API> ``
            [-OpenApiVersion <OpenAPI spec version>] ``
            [-OutputDirectory <directory path>] ``
            [-OutputFilename <filename>] ``

            [-Help]

    Options:
        -ResourceGroupName      Resource group name.
        -ServiceName            Azure API Management instance name.
        -ApiId                  API ID on the Azure API Management instance.
        -ApiVersion             API version to call the REST API for Azure API Management.
                                Default is '2021-08-01'.
        -OpenApiVersion         OpenAPI spec version for download.
                                Default is 'v3'.
        -OutputDirectory        Directory path for download.
                                Default is the current directory.
        -OutputFilename         Filename to store.
                                Default is 'openapi.json'.
        -Help:                  Show this message.
"

    Exit 0
}

# Show usage
$needHelp = ($ResourceGroupName -eq "") -or ($ServiceName -eq "") -or ($ApiId -eq "") -or ($Help -eq $true)
if ($needHelp -eq $true) {
    Show-Usage
    Exit 0
}

# Get the OpenAPI document download link
Write-Output "[$(Get-Date -Format "yyyy-MM-dd HH:mm:ss")] Accessing OpenAPI document link ..."

$subscriptionId = az account show --query "id" -o tsv
$url = "https://management.azure.com/subscriptions/$subscriptionId/resourceGroups/$ResourceGroupName/providers/Microsoft.ApiManagement/service/$ServiceName/apis/$ApiId$("?api-version=$ApiVersion")"

$format = $OpenApiVersion -eq "v2" ? "swagger-link" : "openapi+json-link"
$parameters = @{ export = "true"; format = $format; } | ConvertTo-Json -Compress | ConvertTo-Json

$link = az rest -m get -u $url --url-parameters $parameters --query "link" -o tsv

# Download the OpenAPI document
$filepath = "$((Resolve-Path $OutputDirectory).Path)/$OutputFilename"

Write-Output "[$(Get-Date -Format "yyyy-MM-dd HH:mm:ss")] Downloading OpenAPI document to $filepath ..."

Invoke-RestMethod -Method Get -Uri $link | ConvertTo-Json -Depth 100 -Compress | Out-File -FilePath $filepath -Force

Write-Output "[$(Get-Date -Format "yyyy-MM-dd HH:mm:ss")] OpenAPI document downloaded to $filepath successfully."
