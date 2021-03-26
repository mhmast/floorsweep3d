
export interface Configuration{
    authentication:AuthenticationConfiguration;
}

export interface AuthenticationConfiguration{
    openIdConfigurationEndpoint:string;
    endpointConfiguration:TokenEndpointConfiguration;
}


export interface TokenEndpointConfiguration{
    
    issuer: string;
    authorization_endpoint:string;
    token_endpoint:string;
  }
  