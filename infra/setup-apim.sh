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

subscription_id=$(az account show --query id -o tsv)
resource_group="rg-$AZ_RESOURCE_NAME-$AZ_ENVIRONMENT_CODE-$AZ_LOCATION_CODE"
st_name="st$AZ_RESOURCE_NAME$AZ_ENVIRONMENT_CODE$AZ_LOCATION_CODE"
apim_name="apim-$AZ_RESOURCE_NAME-$AZ_ENVIRONMENT_CODE-$AZ_LOCATION_CODE"
bicep_url="https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/infra/provision-apiManagementApi.json"

for value in `eval echo {0..$suffix_index}`
do
    api_zip=$(echo $urls | jq --argjson value $value '.[$value]' -r)
    fncapp_name="fncapp-$AZ_RESOURCE_NAME-${fncapp_suffixes[$value]}-$AZ_ENVIRONMENT_CODE-$AZ_LOCATION_CODE"
    fncapp_url="https://$fncapp_name.azurewebsites.net/api/openapi/v3.json"

    # Deploy function apps
    api_app=$(az functionapp deploy -g $resource_group -n $fncapp_name --src-url $api_zip --type zip)

    api_path=$(echo ${fncapp_suffixes[$value]} | tr '-' '/')
    api_upper_suffix=$(echo ${fncapp_suffixes[$value]} | tr '[a-z]' '[A-Z]' | tr '-' '_')
    api_nv_name="X_FUNCTIONS_KEY_$api_upper_suffix"
    api_nv_value=$(az functionapp keys list -g $resource_group -n $fncapp_name --query "functionKeys.default" -o tsv)
    api_policy_url="https://raw.githubusercontent.com/devrel-kr/nhn-toast-notification-service-custom-connector/main/infra/apim-api-policy-${fncapp_suffixes[$value]}.xml"

    # Provision APIs to APIM
    az deployment group create \
        -n ApiManagement_Api-${fncapp_suffixes[$value]} \
        -g $resource_group \
        -u $bicep_url \
        -p name=$AZ_RESOURCE_NAME \
        -p env=$AZ_ENVIRONMENT_CODE \
        -p apiMgmtNameValueName=$api_nv_name \
        -p apiMgmtNameValueDisplayName=$api_nv_name \
        -p apiMgmtNameValueValue=$api_nv_value \
        -p apiMgmtApiName=$api_upper_suffix \
        -p apiMgmtApiDisplayName=$api_upper_suffix \
        -p apiMgmtApiDescription=$api_upper_suffix \
        -p apiMgmtApiPath=$api_path \
        -p apiMgmtApiFormat="openapi+json-link" \
        -p apiMgmtApiValue=$fncapp_url \
        -p apiMgmtApiPolicyFormat="xml-link" \
        -p apiMgmtApiPolicyValue=$api_policy_url

    # Update hostnames on function apps
    apim_url="https://$apim_name.azure-api.net/$api_path"
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

# Export swagger.json from APIM API
api_version="2021-12-01-preview"
api_name="sms"
apim_export_url="https://management.azure.com/subscriptions/$subscription_id/resourceGroups/$resource_group/providers/Microsoft.ApiManagement/service/$apim_name/apis/$api_name\?api-version=$api_version&export=true&format=swagger-json"
swaggerdoc=$(az rest --method get --url $apim_export_url)
swaggerjson="$api_name.json"
echo $swaggerdoc | jq 'del(.paths."/openapi/v2.json") |
                       del(.paths."/openapi/v3.json") |
                       del(.paths."/swagger.json") |
                       del(.paths."/swagger/ui") |
                       del(.securityDefinitions) |
                       del(.security) |
                       .info.title |= "NHN Cloud SMS API" |
                       . += { "securityDefinitions": { "app_auth": { "type": "basic", "description": "Toast API basic auth" } } } |
                       . += { "security": [ { "app_auth": [] } ] }' > $swaggerjson

# Upload swagger.json to Azure Blob
st_container_name="swaggers"
st_connstring=$(az storage account show-connection-string -g $resource_group -n $st_name --query "connectionString" -o tsv)
blob_updated=$(az storage blob upload --connection-string $st_connstring -f $swaggerjson -c $st_container_name -n "$swaggerjson" --overwrite)
