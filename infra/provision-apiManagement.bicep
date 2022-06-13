param name string
param suffix string = ''
param location string = resourceGroup().location
param env string = 'dev'

// Storage
@allowed([
    'Standard_LRS'
    'Standard_ZRS'    
    'Standard_GRS'
    'Standard_GZRS'
    'Standard_RAGRS'
    'Standard_RAGZRS'
    'Premium_LRS'
    'Premium_ZRS'
])
param storageAccountSku string = 'Standard_LRS'
// Array item should be in the form of:
// {
//     name: '<container_name>'
//     publicAccess: '<None|Blob|Container>'
// }
param storageAccountBlobContainers array = [
    {
        name: 'swaggers'
        publicAccess: 'Blob'
    }
    {
        name: 'openapis'
        publicAccess: 'Blob'
    }
]
param storageAccountTables array = []

// Log Analytics Workspace
@allowed([
    'Free'
    'Standard'
    'Premium'
    'Standalone'
    'LACluster'
    'PerGB2018'
    'PerNode'
    'CapacityReservation'
])
param workspaceSku string = 'PerGB2018'

// Application Insights
@allowed([
    'web'
    'other'
])
param appInsightsType string = 'web'

@allowed([
    'ApplicationInsights'
    'ApplicationInsightsWithDiagnosticSettings'
    'LogAnalytics'
])
param appInsightsIngestionMode string = 'LogAnalytics'

// API Management
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

module st './storageAccount.bicep' = {
    name: 'StorageAccount'
    params: {
        name: name
        suffix: suffix
        location: location
        env: env
        storageAccountSku: storageAccountSku
        storageAccountBlobContainers: storageAccountBlobContainers
        storageAccountTables: storageAccountTables
    }
}

module wrkspcapimgmt './logAnalyticsWorkspace.bicep' = {
    name: 'LogAnalyticsWorkspace'
    params: {
        name: name
        location: location
        env: env
        workspaceSku: workspaceSku
    }
}

module appinsapimgmt './appInsights.bicep' = {
    name: 'ApplicationInsights'
    params: {
        name: name
        location: location
        env: env
        appInsightsType: appInsightsType
        appInsightsIngestionMode: appInsightsIngestionMode
        workspaceId: wrkspcapimgmt.outputs.id
    }
}

module apim './apiManagement.bicep' = {
    name: 'ApiManagement'
    params: {
        name: name
        location: location
        env: env
        appInsightsId: appinsapimgmt.outputs.id
        appInsightsInstrumentationKey: appinsapimgmt.outputs.instrumentationKey
        apiMgmtSkuName: apiMgmtSkuName
        apiMgmtSkuCapacity: apiMgmtSkuCapacity
        apiMgmtPublisherName: apiMgmtPublisherName
        apiMgmtPublisherEmail: apiMgmtPublisherEmail
        apiMgmtPolicyFormat: apiMgmtPolicyFormat
        apiMgmtPolicyValue: apiMgmtPolicyValue
    }
}
