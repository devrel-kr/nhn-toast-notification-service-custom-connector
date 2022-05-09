#!/bin/bash

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
    smszip=$(echo $urls | jq --argjson value $value '.[$value]' -r)
    fncappName="fncapp-${fncappSuffixes[$value]}-$AZ_ENVIRONMENT_CODE-$AZ_LOCATION_CODE"
    fncappUrl="https://$fncappName.azurewebsites.net/api/openapi/v3.json"
    smsapp=$(az functionapp deploy -g rg-nt-$AZ_ENVIRONMENT_CODE-$AZ_LOCATION_CODE -n $fncappName --src-url $smszip --type zip)
    az deployment group create -n ApiManagement_Api-${fncappSuffixes[$value]} -g rg-nt-$AZ_ENVIRONMENT_CODE-$AZ_LOCATION_CODE -u https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/infra/provision-apiManagementApi.json -p name=nt -p env=$AZ_ENVIRONMENT_CODE -p apiMgmtNameValueName=$fncappName -p apiMgmtNameValueDisplayName=$fncappName -p apiMgmtNameValueValue=$fncappName -p apiMgmtApiName=$fncappName -p apiMgmtApiDisplayName=$fncappName -p apiMgmtApiDescription=$fncappName -p apiMgmtApiPath=$fncappName -p apiMgmtApiValue=$fncappUrl
done