param groupName string
param name string
param location string = ''
param locationCode string = 'wus2'

@allowed([
    'dev'
    'test'
    'prod'
])
param env string = 'dev'

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

var locationResolved = location == '' ? resourceGroup().location : location
var locationCodeMap = {
    koreacentral: 'krc'
    'Korea Central': 'krc'
    westus2: 'wus2'
    'West US 2': 'wus2'
}
var locationCodeResolved = locationCode == '' ? locationCodeMap[locationResolved] : locationCode

var metadata = {
    groupName: '{0}-${groupName}-${env}-${locationCodeResolved}'
    longName: '{0}-${name}-${env}-${locationCodeResolved}'
    shortName: '{0}${name}${env}${locationCodeResolved}'
}

var apiManagement = {
    groupName: format(metadata.groupName, 'apim')
    name: format(metadata.longName, 'apim')
    location: locationResolved
    type: apiMgmtApiType
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

resource apimproduct 'Microsoft.ApiManagement/service/products@2021-08-01' existing = {
    name: '${apim.name}/${apiManagement.productName}'
    scope: resourceGroup(apiManagement.groupName)
}

resource apimproductapi 'Microsoft.ApiManagement/service/products/apis@2021-08-01' = {
    name: '${apimproduct.name}/${apiManagement.apiName}'
}

output id string = apim.id
output name string = apim.name
