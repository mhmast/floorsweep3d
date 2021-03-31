export interface TokenEndpointConfiguration{

    issuer: string;
    // eslint-disable-next-line camelcase
    authorization_endpoint:string;
    // eslint-disable-next-line camelcase
    token_endpoint:string;
    // eslint-disable-next-line camelcase
    end_session_endpoint:string;
  }

export interface AuthenticationConfiguration{
    clientSecret: string;
    realm: string;
    clientId: string;
    openIdConfigurationEndpoint:string;
    endpointConfiguration:TokenEndpointConfiguration;
}

export interface Configuration{
    baseUrl: string;
    authentication:AuthenticationConfiguration;
}
