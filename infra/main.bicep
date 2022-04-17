module storageAccount 'StorageAccount.bicep' = {
  name: 'nhntoaststorageaccount'
}

module appServicePlan 'ConsumptionPlan.bicep' = {
  name: 'nhnToast-AppServicePlan'
}

module functionApp 'FunctionApp.bicep' = {
  name: 'nhnToast-Functions'
  params: {
    appServicePlanName: appServicePlan.outputs.appServicePlanName
    applicationInsightsName: applicationInsights.outputs.applicationInsightsName
    storageAccountName: storageAccount.outputs.storageAccountName
  }
}

module logAnalyticsWorkspace 'LogAnalytics.bicep' = {
  name: 'nhnToast-LogAnalyticsWorkspace'
}

module applicationInsights 'ApplicationInsights.bicep' = {
  name: 'nhnToast-ApplicationInsights'
  params: {
    logAnalyticsWorkspaceName: logAnalyticsWorkspace.outputs.logAnalyticsWorkspaceName
  }
}


