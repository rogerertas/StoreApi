param location string = 'West Europe'

resource appServicePlan 'Microsoft.Web/serverfarms@2021-02-01' = {
  name: 'storeapi-dev-43451'
  location: location
  sku: {
    name: 'F1' // Free tier
  }
  properties: {
    reserved: false 
  }
}

resource webApp 'Microsoft.Web/sites@2021-02-01' = {
  name: 'storeapi-dev-43451'
  location: location
  properties: {
    serverFarmId: appServicePlan.id
  }
}
