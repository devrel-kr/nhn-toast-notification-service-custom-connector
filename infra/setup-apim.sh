#!/bin/bash

set -e

declare -a fncapp_suffixes
suffix_index=1

# Delimit by comma (',')
while [ "$(echo $AZ_APP_SUFFIXES | cut -d ',' -f $suffix_index)" != ""  ]
do
    fncapp_suffixes[$suffix_index-1]=$(echo $AZ_APP_SUFFIXES | cut -d ',' -f $suffix_index)
    suffix_index=`expr $suffix_index + 1`
done
suffix_index=`expr $suffix_index - 2`

urls=$(curl -H "Accept: application/vnd.github.v3+json" \
    https://api.github.com/repos/devrel-kr/nhn-toast-notification-service-custom-connector/releases/latest | \
    jq '[.assets[] | .browser_download_url]')
resource_group="rg-$AZ_RESOURCE_NAME-$AZ_ENVIRONMENT_CODE-$AZ_LOCATION_CODE"
bicep_url="https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/infra/provision-apiManagementApi.json"

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
    api_policy_url="https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/infra/apim-api-policy-${fncapp_suffixes[$value]}.xml"

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
        -p apiMgmtApiFormat="openapi+json-link" \
        -p apiMgmtApiValue=$fncapp_url \
        -p apiMgmtApiPolicyFormat="xml-link" \
        -p apiMgmtApiPolicyValue=$api_policy_url

    # Update hostnames on function apps
    apim_url="https://apim-$AZ_RESOURCE_NAME-$AZ_ENVIRONMENT_CODE-$AZ_LOCATION_CODE.azure-api.net/$api_path"
    appsettings_hostnames=$(az functionapp config appsettings list -g $resource_group -n $fncapp_name | jq '.[] | select(.name == "OpenApi__HostNames") | .value' -r)
    if [ "$appsettings_hostnames" == "" ]
    then
        openapi_hostnames=$apim_url
    else
        openapi_hostnames=$apim_url,$appsettings_hostnames
    fi

    appsettings_updated=$(az functionapp config appsettings set -g $resource_group -n $fncapp_name --settings OpenApi__HostNames=$openapi_hostnames)

    # Update OpenAPI document version
    filename=$(echo ${api_zip##*/})
    version_extension=$(echo ${filename##*-})
    openapi_docversion=$(echo ${version_extension%.*})

    appsettings_updated=$(az functionapp config appsettings set -g $resource_group -n $fncapp_name --settings OpenApi__DocVersion=$openapi_docversion)

    # Update NHN Toast endpoints on function apps
    appsettings=$(curl https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/infra/appsettings-${fncapp_suffixes[$value]}.json)
    appsettings_length=$(echo $appsettings | jq '. | length')
    for (( i=0; i<$appsettings_length; i++ ))
    do
        appsettings_name=$(echo $appsettings | jq --arg i "$i" '.[$i|fromjson].name' -r)
        appsettings_value=$(echo $appsettings | jq --arg i "$i" '.[$i|fromjson].value' -r)

        appsettings_updated=$(az functionapp config appsettings set -g $resource_group -n $fncapp_name --settings $appsettings_name="$appsettings_value")
    done
done