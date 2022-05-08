#!/bin/bash

set -e

names=("SmsApp" "SmsVerificationApp")

#'dev', 'test', 'prod', 'kdy', 'kms', 'lsw', 'pjm'
environment=$1

fncappSuffixes=($2 $3)
suffixIndex=0

version=$(curl -H "Accept: application/vnd.github.v3+json" \
https://api.github.com/repos/devrel-kr/nhn-toast-notification-service-custom-connector/releases/latest| \
jq '.tag_name' -r)

urls=$(curl -H "Accept: application/vnd.github.v3+json" \
https://api.github.com/repos/devrel-kr/nhn-toast-notification-service-custom-connector/releases/latest| \
jq '.assets[] | { name: .name, url: .browser_download_url }')

for name in ${names[@]}
do
    filename="$name-$version.zip"
    smszip=$(echo $urls | jq --arg v $filename 'select(.name == $v) | .url' -r)
    fncappName="fncapp-${fncappSuffixes[$suffixIndex]}-$environment-krc"
    suffixIndex=`expr $suffixIndex + 1`
    fncappUrl="https://$fncappName.azurewebsites.net/api/openapi/v3.json"
    smsapp=$(az functionapp deploy -g rg-nt-$environment-krc -n $fncappName --src-url $smszip --type zip)
    az deployment group create -n ApiManagement_Api-$name -g rg-nt-$environment-krc -u https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/infra/provision-apiManagementApi.json -p name=nt -p env=$environment -p apiMgmtNameValueName=$fncappName -p apiMgmtNameValueDisplayName=$fncappName -p apiMgmtNameValueValue=$fncappName -p apiMgmtApiName=$fncappName -p apiMgmtApiDisplayName=$fncappName -p apiMgmtApiDescription=$fncappName -p apiMgmtApiPath=$fncappName -p apiMgmtApiValue=$fncappUrl
done