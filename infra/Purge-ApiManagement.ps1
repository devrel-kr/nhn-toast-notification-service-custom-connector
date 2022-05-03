# Purges the deleted the API Management instance.
Param(
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
            [-ApiVersion <API version>] ``

            [-Help]

    Options:
        -ApiVersion     REST API version. Default is `2021-08-01`.

        -Help:          Show this message.
"

    Exit 0
}

# Show usage
$needHelp = $Help -eq $true
if ($needHelp -eq $true) {
    Show-Usage
    Exit 0
}

$account = $(az account show | ConvertFrom-Json)

$url = "https://management.azure.com/subscriptions/$($account.id)/providers/Microsoft.ApiManagement/deletedservices?api-version=$($ApiVersion)"

# Uncomment to debug
# $url

$apims = $(az rest -m get -u $url --query "value" | ConvertFrom-Json)
if ($apims -eq $null) {
    Write-Output "No soft-deleted API Management instance found to purge"
    Exit 0
}

$options = ""
if ($apims.Count -eq 1) {
    $name = $apims.name
    $options += "    1: $name `n"
} else {
    $apims | ForEach-Object {
        $i = $apims.IndexOf($_)
        $name = $_.name
        $options += "    $($i +1): $name `n"
    }
}
$options += "    q: Quit`n"

$input = Read-Host "Select the number to purge the soft-deleted API Management instance or 'q' to quit: `n`n$options"
if ($input -eq "q") {
    Exit 0
}
$parsed = $input -as [int]
if ($parsed -eq $null) {
    Write-Output "Invalid input"
    Exit 0
}
if ($parsed -gt $apims.Count) {
    Write-Output "Invalid input"
    Exit 0
}
$index = $parsed - 1

$url = "https://management.azure.com$($apims[$index].id)?api-version=$($ApiVersion)"

# Uncomment to debug
# $url

$apim = $(az rest -m get -u $url)
if ($apim -ne $null) {
    $deleted = $(az rest -m delete -u $url)
}
