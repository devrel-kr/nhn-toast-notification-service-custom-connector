param appServicePlanName string
param applicationInsightsName string
param storageAccountName string

resource appServicePlan 'Microsoft.Web/serverfarms@2021-03-01' existing = {
  name: appServicePlanName
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: applicationInsightsName
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-08-01' existing = {
  name: storageAccountName
}

resource functionApp 'Microsoft.Web/sites@2021-03-01' = {
  name: 'nhnToast-Functions'
  location: 'Korea Central'
  dependsOn:[
    appServicePlan
    applicationInsights
    storageAccount
  ]

  kind: 'functionapp'
  properties: {
    clientAffinityEnabled: false
    clientCertEnabled: false
    clientCertMode: 'Required'
    containerSize: 1536
    customDomainVerificationId: 'EBCB4691550F7B09989538D97547F61F7EB577227FF19E4976958E7EC5D43C0D'
    dailyMemoryTimeQuota: 0
    enabled: true
    hostNamesDisabled: false
    hostNameSslStates: [
      {
        hostType: 'Standard'
        name: 'nhntoast-functions.azurewebsites.net'
        sslState: 'Disabled'
      }
      {
        hostType: 'Repository'
        name: 'nhntoast-functions.scm.azurewebsites.net'
        sslState: 'Disabled'
      }
    ]
    httpsOnly: false
    hyperV: false
    isXenon: false
    keyVaultReferenceIdentity: 'SystemAssigned'
    redundancyMode: 'None'
    reserved: false
    scmSiteAlsoStopped: false
    serverFarmId: appServicePlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: applicationInsights.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: 'InstrumentationKey=${applicationInsights.properties.InstrumentationKey}'
        }
      ]
      acrUseManagedIdentityCreds: false
      alwaysOn: false
      functionAppScaleLimit: 200
      http20Enabled: false
      minimumElasticInstanceCount: 0
      numberOfWorkers: 1
    }
    storageAccountRequired: false
  }
}
