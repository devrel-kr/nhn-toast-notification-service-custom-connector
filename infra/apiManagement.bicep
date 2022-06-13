param name string
param location string = resourceGroup().location
param env string = 'dev'

param appInsightsId string

@secure()
param appInsightsInstrumentationKey string

@allowed([
    'Consumption'
    'Isolated'
    'Developer'
    'Basic'
    'Standard'
    'Premium'
])
param apiMgmtSkuName string = 'Consumption'

param apiMgmtSkuCapacity int = 0

param apiMgmtPublisherName string
param apiMgmtPublisherEmail string

@allowed([
    'rawxml'
    'rawxml-link'
    'xml'
    'xml-link'
])
param apiMgmtPolicyFormat string = 'xml'
param apiMgmtPolicyValue string = '<!--\r\n    IMPORTANT:\r\n    - Policy elements can appear only within the <inbound>, <outbound>, <backend> section elements.\r\n    - Only the <forward-request> policy element can appear within the <backend> section element.\r\n    - To apply a policy to the incoming request (before it is forwarded to the backend service), place a corresponding policy element within the <inbound> section element.\r\n    - To apply a policy to the outgoing response (before it is sent back to the caller), place a corresponding policy element within the <outbound> section element.\r\n    - To add a policy position the cursor at the desired insertion point and click on the round button associated with the policy.\r\n    - To remove a policy, delete the corresponding policy statement from the policy document.\r\n    - Policies are applied in the order of their appearance, from the top down.\r\n-->\r\n<policies>\r\n  <inbound></inbound>\r\n  <backend>\r\n    <forward-request />\r\n  </backend>\r\n  <outbound></outbound>\r\n</policies>'

var locationCodeMap = {
    australiacentral: 'auc'
    'Australia Central': 'auc'
    australiaeast: 'aue'
    'Australia East': 'aue'
    australiasoutheast: 'ause'
    'Australia Southeast': 'ause'
    brazilsouth: 'brs'
    'Brazil South': 'brs'
    canadacentral: 'cac'
    'Canada Central': 'cac'
    canadaeast: 'cae'
    'Canada East': 'cae'
    centralindia: 'cin'
    'Central India': 'cin'
    centralus: 'cus'
    'Central US': 'cus'
    eastasia: 'ea'
    'East Asia': 'ea'
    eastus: 'eus'
    'East US': 'eus'
    eastus2: 'eus2'
    'East US 2': 'eus2'
    francecentral: 'frc'
    'France Central': 'frc'
    germanywestcentral: 'dewc'
    'Germany West Central': 'dewc'
    japaneast: 'jpe'
    'Japan East': 'jpe'
    japanwest: 'jpw'
    'Japan West': 'jpw'
    jioindianorthwest: 'jinw'
    'Jio India North West': 'jinw'
    koreacentral: 'krc'
    'Korea Central': 'krc'
    koreasouth: 'krs'
    'Korea South': 'krs'
    northcentralus: 'ncus'
    'North Central US': 'ncus'
    northeurope: 'neu'
    'North Europe': 'neu'
    norwayeast: 'noe'
    'Norway East': 'noe'
    southafricanorth: 'zan'
    'South Africa North': 'zan'
    southcentralus: 'scus'
    'South Central US': 'scus'
    southindia: 'sin'
    'South India': 'sin'
    southeastasia: 'sea'
    'Southeast Asia': 'sea'
    swedencentral: 'sec'
    'Sweden Central': 'sec'
    switzerlandnorth: 'chn'
    'Switzerland North': 'chn'
    uaenorth: 'uaen'
    'UAE North': 'uaen'
    uksouth: 'uks'
    'UK South': 'uks'
    ukwest: 'ukw'
    'UK West': 'ukw'
    westcentralus: 'wcus'
    'West Central US': 'wcus'
    westeurope: 'weu'
    'West Europe': 'weu'
    westindia: 'win'
    'West India': 'win'
    westus: 'wus'
    'West US': 'wus'
    westus2: 'wus2'
    'West US 2': 'wus2'
    westus3: 'wus3'
    'West US 3': 'wus3'
}
var locationCode = locationCodeMap[location]

var metadata = {
    longName: '{0}-${name}-${env}-${locationCode}'
    shortName: replace('{0}${name}{1}${env}${locationCode}', '-', '')
}

var appInsights = {
    id: appInsightsId
    name: format(metadata.longName, 'appins')
    instrumentationKey: appInsightsInstrumentationKey
}
var apiManagement = {
    name: format(metadata.longName, 'apim')
    location: location
    skuName: apiMgmtSkuName
    skuCapacity: apiMgmtSkuCapacity
    publisherName: apiMgmtPublisherName
    publisherEmail: apiMgmtPublisherEmail
    policyFormat: apiMgmtPolicyFormat
    policyValue: apiMgmtPolicyValue
}

resource apim 'Microsoft.ApiManagement/service@2021-08-01' = {
    name: apiManagement.name
    location: apiManagement.location
    sku: {
        name: apiManagement.skuName
        capacity: apiManagement.skuCapacity
    }
    properties: {
        publisherName: apiManagement.publisherName
        publisherEmail: apiManagement.publisherEmail
    }
}

resource apimNamedValue 'Microsoft.ApiManagement/service/namedValues@2021-08-01' = {
    name: '${apim.name}/RESOURCE_NAME'
    properties: {
        displayName: 'RESOURCE_NAME'
        secret: true
        value: apim.name
    }
}

resource apimlogger 'Microsoft.ApiManagement/service/loggers@2021-08-01' = {
    name: '${apim.name}/${appInsights.name}'
    properties: {
        loggerType: 'applicationInsights'
        resourceId: appInsights.id
        credentials: {
            instrumentationKey: appInsights.instrumentationKey
        }
    }
}

resource apimpolicy 'Microsoft.ApiManagement/service/policies@2021-08-01' = {
    name: '${apim.name}/policy'
    properties: {
        format: apiManagement.policyFormat
        value: apiManagement.policyValue
    }
}

resource apimproduct 'Microsoft.ApiManagement/service/products@2021-08-01' = {
    name: '${apim.name}/default'
    properties: {
        displayName: 'Default Product'
        description: 'This is the default product created by the template, which includes all APIs.'
        state: 'published'
        subscriptionRequired: false
    }
}

output id string = apim.id
output name string = apim.name
