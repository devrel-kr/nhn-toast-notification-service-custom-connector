#!/bin/bash

bicepUrl="https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/infra/provision-apiManagementApi.json"

resourceGroup="rg-$AZ_RESOURCE_NAME-$AZ_ENVIRONMENT_CODE-$AZ_LOCATION_CODE"

declare -a fncappSuffixes
Suffixindex=1

# ','기준으로 나누기
while [ "$(echo $AZ_APP_SUFFIXES | cut -d ',' -f $Suffixindex)" != ""  ]
do
    fncappSuffixes[$Suffixindex-1]=$(echo $AZ_APP_SUFFIXES | cut -d ',' -f $Suffixindex)
    Suffixindex=`expr $Suffixindex + 1`
done
Suffixindex=`expr $Suffixindex - 2`

urls=$(curl -H "Accept: application/vnd.github.v3+json" \
https://api.github.com/repos/devrel-kr/nhn-toast-notification-service-custom-connector/releases/latest| \
jq '[.assets[] | .browser_download_url]')

for value in `eval echo {0..$Suffixindex}`
do
    apizip=$(echo $urls | jq --argjson value $value '.[$value]' -r)
    fncappName="fncapp-$AZ_RESOURCE_NAME-${fncappSuffixes[$value]}-$AZ_ENVIRONMENT_CODE-$AZ_LOCATION_CODE"
    fncappUrl="https://$fncappName.azurewebsites.net/api/openapi/v3.json"

    # Deploy function apps
    apiapp=$(az functionapp deploy -g $resourceGroup -n $fncappName --src-url $apizip --type zip)

    apiKey=$(az functionapp keys list -g $resourceGroup -n $fncappName --query "functionKeys.default" -o tsv)
    apiPath=$(echo ${fncappSuffixes[$value]} | tr '-' '/')
    apiValue=$(echo ${fncappSuffixes[$value]} | tr '[a-z]' '[A-Z]')
    apiValue=$(echo $apiValue | tr '-' '_')
    apiValue="X_FUNCTIONS_KEY_$apiValue"

    # Provision APIs to APIM
    az deployment group create \
    -n ApiManagement_Api-${fncappSuffixes[$value]} \
    -g $resourceGroup \
    -u $bicepUrl \
    -p name=$AZ_RESOURCE_NAME \
    -p env=$AZ_ENVIRONMENT_CODE \
    -p apiMgmtNameValueName=$apiValue \
    -p apiMgmtNameValueDisplayName=$apiValue \
    -p apiMgmtNameValueValue=$apiKey \
    -p apiMgmtApiName=$apiValue \
    -p apiMgmtApiDisplayName=$apiValue \
    -p apiMgmtApiDescription=$apiValue \
    -p apiMgmtApiPath=$apiPath \
    -p apiMgmtApiValue=$fncappUrl

    apiUrl="https://apim-$AZ_RESOURCE_NAME-$AZ_ENVIRONMENT_CODE-$AZ_LOCATION_CODE.azure-api.net/$apiPath"
    settingList=$(az functionapp config appsettings list -g $resourceGroup -n $fncappName | jq '.[] | select(.name == "OpenApi__HostNames") | .value' -r)
    if [ "$settingList" == "" ]
    then
        apihostnames=$apiUrl
    else
        apihostnames=$apiUrl,$settingList
    fi

    # Update app settings on function apps
    apisettings=$(az functionapp config appsettings set -g $resourceGroup -n $fncappName --settings OpenApi__HostNames=$apihostnames)
done