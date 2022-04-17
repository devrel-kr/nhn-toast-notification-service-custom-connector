param logAnalyticsWorkspaceName string

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' existing = {
  name: logAnalyticsWorkspaceName
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: 'nhnToast-ApplicationInsights'
  location: 'koreacentral'
  kind: 'web'
  dependsOn: [
    logAnalyticsWorkspace
  ]
  properties: {
    Application_Type: 'web'
    Flow_Type: 'Redfield'
    IngestionMode: 'LogAnalytics'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
    Request_Source: 'IbizaAIExtension'
    RetentionInDays: 90
    WorkspaceResourceId: logAnalyticsWorkspace.id
  }
}

output applicationInsightsName string = applicationInsights.name
