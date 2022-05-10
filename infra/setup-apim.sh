#!/bin/bash

bicep_url="https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/infra/provision-apiManagementApi.json"

resource_group="rg-$AZ_RESOURCE_NAME-$AZ_ENVIRONMENT_CODE-$AZ_LOCATION_CODE"

declare -a fncapp_suffixes
suffix_index=1

# ','기준으로 나누기
while [ "$(echo $AZ_APP_SUFFIXES | cut -d ',' -f $suffix_index)" != ""  ]
do
    fncapp_suffixes[$suffix_index-1]=$(echo $AZ_APP_SUFFIXES | cut -d ',' -f $suffix_index)
    suffix_index=`expr $suffix_index + 1`
done
suffix_index=`expr $suffix_index - 2`

urls=$(curl -H "Accept: application/vnd.github.v3+json" \
https://api.github.com/repos/devrel-kr/nhn-toast-notification-service-custom-connector/releases/latest| \
jq '[.assets[] | .browser_download_url]')

for value in `eval echo {0..$suffix_index}`
do
    api_zip=$(echo $urls | jq --argjson value $value '.[$value]' -r)
    fncapp_name="fncapp-$AZ_RESOURCE_NAME-${fncapp_suffixes[$value]}-$AZ_ENVIRONMENT_CODE-$AZ_LOCATION_CODE"
    fncapp_url="https://$fncapp_name.azurewebsites.net/api/openapi/v3.json"

    # Deploy function apps
    api_app=$(az functionapp deploy -g $resource_group -n $fncapp_name --src-url $api_zip --type zip)

    api_key=$(az functionapp keys list -g $resource_group -n $fncapp_name --query "functionKeys.default" -o tsv)
    api_path=$(echo ${fncapp_suffixes[$value]} | tr '-' '/')
    api_upper_suffix=$(echo ${fncapp_suffixes[$value]} | tr '[a-z]' '[A-Z]' | tr '-' '_')
    api_name="X_FUNCTIONS_KEY_$api_upper_suffix"

    # Provision APIs to APIM
    az deployment group create \
    -n ApiManagement_Api-${fncapp_suffixes[$value]} \
    -g $resource_group \
    -u $bicep_url \
    -p name=$AZ_RESOURCE_NAME \
    -p env=$AZ_ENVIRONMENT_CODE \
    -p apiMgmtNameValueName=$api_name \
    -p apiMgmtNameValueDisplayName=$api_name \
    -p apiMgmtNameValueValue=$api_key \
    -p apiMgmtApiName=$api_upper_suffix \
    -p apiMgmtApiDisplayName=$api_upper_suffix \
    -p apiMgmtApiDescription=$api_upper_suffix \
    -p apiMgmtApiPath=$api_path \
    -p apiMgmtApiValue=$fncapp_url

    apim_url="https://apim-$AZ_RESOURCE_NAME-$AZ_ENVIRONMENT_CODE-$AZ_LOCATION_CODE.azure-api.net/$api_path"
    setting_list=$(az functionapp config appsettings list -g $resource_group -n $fncapp_name | jq '.[] | select(.name == "OpenApi__HostNames") | .value' -r)
    if [ "$setting_list" == "" ]
    then
        api_host_names=$apim_url
    else
        api_host_names=$apim_url,$setting_list
    fi

    # Update app settings on function apps
    api_settings=$(az functionapp config appsettings set -g $resource_group -n $fncapp_name --settings OpenApi__HostNames=$api_host_names)
done