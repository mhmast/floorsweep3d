
export interface Configuration{
    baseUrl: string;
    authentication:AuthenticationConfiguration;
}

export interface AuthenticationConfiguration{
    realm: string;
    clientId: string;
    openIdConfigurationEndpoint:string;
    endpointConfiguration:TokenEndpointConfiguration;
}


export interface TokenEndpointConfiguration{
    
    issuer: string;
    authorization_endpoint:string;
    token_endpoint:string;
  }
  