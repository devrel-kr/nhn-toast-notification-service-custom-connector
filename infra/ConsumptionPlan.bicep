resource appServicePlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: 'nhnToast-AppServicePlan'
  location: 'Korea Central'
  sku: {
    capacity: 0
    family: 'Y'
    name: 'Y1'
    size: 'Y1'
    tier: 'Dynamic'
  }
  kind: 'functionapp'
  properties: {
    elasticScaleEnabled: false
    hyperV: false
    isSpot: false
    isXenon: false
    maximumElasticWorkerCount: 1
    perSiteScaling: false
    reserved: false
    targetWorkerCount: 0
    targetWorkerSizeId: 0
    zoneRedundant: false
  }
}

output appServicePlanName string = appServicePlan.name
