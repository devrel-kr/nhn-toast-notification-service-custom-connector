param name string
param location string = resourceGroup().location
param env string = 'dev'

param apiMgmtNameValueName string
param apiMgmtNameValueDisplayName string

@secure()
param apiMgmtNameValueValue string

@allowed([
    'http'
    'soap'
    'websocket'
    'graphql'
])
param apiMgmtApiType string = 'http'

param apiMgmtApiName string
param apiMgmtApiDisplayName string
param apiMgmtApiDescription string
param apiMgmtApiPath string
param apiMgmtApiSubscriptionRequired bool = false

@allowed([
    'swagger-json'
    'swagger-link-json'
    'openapi'
    'openapi+json'
    'openapi+json-link'
    'openapi-link'
    'wadl-link-json'
    'wadl-xml'
    'wsdl'
    'wsdl-link'
    'graphql-link'
])
param apiMgmtApiFormat string = 'openapi+json-link'
param apiMgmtApiValue string

@allowed([
    'rawxml'
    'rawxml-link'
    'xml'
    'xml-link'
])
param apiMgmtApiPolicyFormat string = 'xml'
param apiMgmtApiPolicyValue string = '<!--\r\n  IMPORTANT:\r\n  - Policy elements can appear only within the <inbound>, <outbound>, <backend> section elements.\r\n  - To apply a policy to the incoming request (before it is forwarded to the backend service), place a corresponding policy element within the <inbound> section element.\r\n  - To apply a policy to the outgoing response (before it is sent back to the caller), place a corresponding policy element within the <outbound> section element.\r\n  - To add a policy, place the cursor at the desired insertion point and select a policy from the sidebar.\r\n  - To remove a policy, delete the corresponding policy statement from the policy document.\r\n  - Position the <base> element within a section element to inherit all policies from the corresponding section element in the enclosing scope.\r\n  - Remove the <base> element to prevent inheriting policies from the corresponding section element in the enclosing scope.\r\n  - Policies are applied in the order of their appearance, from the top down.\r\n  - Comments within policy elements are not supported and may disappear. Place your comments between policy elements or at a higher level scope.\r\n-->\r\n<policies>\r\n  <inbound>\r\n    <base />\r\n  </inbound>\r\n  <backend>\r\n    <base />\r\n  </backend>\r\n  <outbound>\r\n    <base />\r\n  </outbound>\r\n  <on-error>\r\n    <base />\r\n  </on-error>\r\n</policies>'

param apiMgmtProductName string = 'default'

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

var apiManagement = {
    groupName: format(metadata.longName, 'rg')
    name: format(metadata.longName, 'apim')
    location: location
    type: apiMgmtApiType
    nvName: apiMgmtNameValueName
    nvDisplayName: apiMgmtNameValueDisplayName
    nvValue: apiMgmtNameValueValue
    apiName: apiMgmtApiName
    displayName: apiMgmtApiDisplayName
    description: apiMgmtApiDescription
    path: apiMgmtApiPath
    subscriptionRequired: apiMgmtApiSubscriptionRequired
    format: apiMgmtApiFormat
    value: apiMgmtApiValue
    policyFormat: apiMgmtApiPolicyFormat
    policyValue: apiMgmtApiPolicyValue
    productName: apiMgmtProductName
}

resource apim 'Microsoft.ApiManagement/service@2021-08-01' existing = {
    name: apiManagement.name
    scope: resourceGroup(apiManagement.groupName)
}

resource apimNamedValue 'Microsoft.ApiManagement/service/namedValues@2021-08-01' = {
    name: '${apim.name}/${apiManagement.nvName}'
    properties: {
        displayName: apiManagement.nvDisplayName
        secret: true
        value: apiManagement.nvValue
    }
}

resource apimapi 'Microsoft.ApiManagement/service/apis@2021-08-01' = {
    name: '${apim.name}/${apiManagement.apiName}'
    properties: {
        type: apiManagement.type
        displayName: apiManagement.displayName
        description: apiManagement.description
        path: apiManagement.path
        subscriptionRequired: apiManagement.subscriptionRequired
        format: apiManagement.format
        value: apiManagement.value
    }
}

resource apimapipolicy 'Microsoft.ApiManagement/service/apis/policies@2021-08-01' = {
    name: '${apimapi.name}/policy'
    properties: {
        format: apiManagement.policyFormat
        value: apiManagement.policyValue
    }
}

// resource apimproduct 'Microsoft.ApiManagement/service/products@2021-08-01' existing = {
//     name: '${apim.name}/${apiManagement.productName}'
//     scope: resourceGroup(apiManagement.groupName)
// }

// resource apimproductapi 'Microsoft.ApiManagement/service/products/apis@2021-08-01' = {
//     name: '${apimproduct.name}/${apiManagement.apiName}'
// }

var operations = [
    {
        name: 'openapi-v2-json'
        displayName: 'openapi/v2.json'
        method: 'GET'
        urlTemplate: '/openapi/v2.json'
    }
    {
        name: 'openapi-v3-json'
        displayName: 'openapi/v3.json'
        method: 'GET'
        urlTemplate: '/openapi/v3.json'
    }
    {
        name: 'swagger-json'
        displayName: 'swagger.json'
        method: 'GET'
        urlTemplate: '/swagger.json'
    }
    {
        name: 'swagger-ui'
        displayName: 'swagger/ui'
        method: 'GET'
        urlTemplate: '/swagger/ui'
    }
]

resource apimapioperations 'Microsoft.ApiManagement/service/apis/operations@2021-08-01' = [for op in operations: {
    name: '${apimapi.name}/${op.name}'
    properties: {
        displayName: op.displayName
        method: op.method
        urlTemplate: op.urlTemplate
        templateParameters: []
        responses: []
    }
}]

output id string = apimapi.id
output name string = apimapi.name
output path string = reference(apimapi.id).path
